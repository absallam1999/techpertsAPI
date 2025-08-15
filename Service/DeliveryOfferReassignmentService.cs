using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class DeliveryReassignmentService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DeliveryReassignmentService> _logger;
    private readonly DeliveryAssignmentSettings _settings;

    public DeliveryReassignmentService(
        IServiceScopeFactory scopeFactory,
        ILogger<DeliveryReassignmentService> logger,
        IOptions<DeliveryAssignmentSettings> settings)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _settings = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "DeliveryReassignmentService started with settings: Interval={IntervalSeconds}s, MaxRetries={MaxRetries}, RetryDelay={RetryDelaySeconds}s",
            _settings.CheckIntervalSeconds,
            _settings.MaxRetries,
            _settings.RetryDelaySeconds
        );

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var clusterService = scope.ServiceProvider.GetRequiredService<IDeliveryClusterService>();
                var deliveryPersonService = scope.ServiceProvider.GetRequiredService<IDeliveryPersonService>();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                // Get unassigned clusters (still needs implementation in service)
                var unassignedClustersResponse = await clusterService.GetUnassignedClustersAsync();
                if (!unassignedClustersResponse.Success || unassignedClustersResponse.Data == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(_settings.CheckIntervalSeconds), stoppingToken);
                    continue;
                }

                foreach (var cluster in unassignedClustersResponse.Data)
                {
                    // Ensure we only retry the same order's cluster if not accepted
                    if (cluster.RetryCount >= _settings.MaxRetries)
                    {
                        await notificationService.SendNotificationToRoleAsync(
                            "Admin",
                            $"Cluster #{cluster.Id} could not be assigned after {_settings.MaxRetries} attempts.",
                            NotificationType.SystemAlert,
                            cluster.Id,
                            "DeliveryCluster"
                        );
                        continue;
                    }

                    // Wait between retries for this cluster
                    if (cluster.LastRetryTime.HasValue && (DateTime.UtcNow - cluster.LastRetryTime.Value).TotalSeconds < _settings.RetryDelaySeconds)
                    {
                        continue; // Not yet time for the next retry
                    }

                    var availableDriversResponse = await deliveryPersonService.GetAvailableDeliveryPersonsAsync();
                    if (!availableDriversResponse.Success || availableDriversResponse.Data == null || !availableDriversResponse.Data.Any())
                    {
                        await notificationService.SendNotificationToRoleAsync(
                            "Admin",
                            $"No available drivers for cluster #{cluster.Id}.",
                            NotificationType.SystemAlert,
                            cluster.Id,
                            "DeliveryCluster"
                        );
                        continue;
                    }

                    var chosenDriver = availableDriversResponse.Data.FirstOrDefault();
                    if (chosenDriver == null)
                        continue;

                    var assignResult = await clusterService.AssignDriverAsync(cluster.Id, chosenDriver.Id);
                    if (!assignResult.Success)
                    {
                        _logger.LogWarning($"Failed to assign driver to cluster {cluster.Id}: {assignResult.Message}");
                        continue;
                    }

                    // Update retry count and last retry time in DB
                    cluster.RetryCount++;
                    cluster.LastRetryTime = DateTime.UtcNow;
                    await clusterService.UpdateClusterAsync(cluster.Id, cluster);

                    await notificationService.NotifyDeliveryPersonAsync(
                        chosenDriver.Id,
                        $"A delivery cluster #{cluster.Id} has been assigned to you."
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in cluster reassignment service");
            }

            await Task.Delay(TimeSpan.FromSeconds(_settings.CheckIntervalSeconds), stoppingToken);
        }
    }
}
