using Core.DTOs;
using Core.DTOs.DeliveryDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DeliveryClusterService : IDeliveryClusterService
    {
        private readonly IRepository<DeliveryCluster> _clusterRepo;

        public DeliveryClusterService(IRepository<DeliveryCluster> clusterRepo)
        {
            _clusterRepo = clusterRepo;
        }

        public async Task<GeneralResponse<DeliveryClusterDTO>> CreateClusterAsync(string deliveryId, DeliveryClusterCreateDTO dto)
        {
            if (dto == null)
                return new GeneralResponse<DeliveryClusterDTO>
                {
                    Success = false,
                    Message = "Cluster not found.",
                    Data = null
                };
            var entity = DeliveryClusterMapper.ToEntity(dto);
            entity.DeliveryId = deliveryId;

            await _clusterRepo.AddAsync(entity);
            await _clusterRepo.SaveChangesAsync();

            return new GeneralResponse<DeliveryClusterDTO>
            {
                Success = true,
                Message = "Cluster created successfully.",
                Data = DeliveryClusterMapper.ToDTO(entity)
            };
        }

        public async Task<GeneralResponse<DeliveryClusterDTO>> GetByIdAsync(string clusterId)
        {
            var entity = await _clusterRepo.GetByIdWithIncludesAsync(clusterId,
                c => c.TechCompany.User,
                c => c.AssignedDriver.User);

            if (entity == null)
                return new GeneralResponse<DeliveryClusterDTO> { Success = false, Message = "Cluster not found." };

            return new GeneralResponse<DeliveryClusterDTO>
            {
                Success = true,
                Data = DeliveryClusterMapper.ToDTO(entity)
            };
        }

        public async Task<GeneralResponse<IEnumerable<DeliveryClusterDTO>>> GetByDeliveryIdAsync(string deliveryId)
        {
            var clusters = await _clusterRepo.FindWithIncludesAsync(
                c => c.DeliveryId == deliveryId,
                c => c.TechCompany.User,
                c => c.AssignedDriver.User
            );

            return new GeneralResponse<IEnumerable<DeliveryClusterDTO>>
            {
                Success = true,
                Data = clusters.Select(DeliveryClusterMapper.ToDTO)
            };
        }

        public async Task<GeneralResponse<DeliveryClusterDTO>> UpdateClusterAsync(string clusterId, DeliveryClusterDTO dto)
        {
            var entity = await _clusterRepo.GetByIdAsync(clusterId);
            if (entity == null)
                return new GeneralResponse<DeliveryClusterDTO> { Success = false, Message = "Cluster not found." };

            DeliveryClusterMapper.UpdateEntity(entity, dto);
            _clusterRepo.Update(entity);
            await _clusterRepo.SaveChangesAsync();

            return new GeneralResponse<DeliveryClusterDTO>
            {
                Success = true,
                Message = "Cluster updated successfully.",
                Data = DeliveryClusterMapper.ToDTO(entity)
            };
        }

        public async Task<GeneralResponse<DeliveryClusterDTO>> UpdateClusterTrackingAsync(
            string clusterId,
            DeliveryClusterTrackingDTO trackingDto)
        {
            var cluster = await _clusterRepo.GetByIdAsync(clusterId);
            if (cluster == null)
            {
                return new GeneralResponse<DeliveryClusterDTO>
                {
                    Success = false,
                    Message = "Cluster not found.",
                    Data = null
                };
            }

            if (cluster.Tracking == null)
            {
                cluster.Tracking = new DeliveryClusterTracking
                {
                    clusterId = cluster.Id
                };
            }

            cluster.Tracking.Location = trackingDto.Location ?? cluster.Tracking.Location;
            cluster.Tracking.LastUpdated = trackingDto.LastUpdated == default ? DateTime.UtcNow : trackingDto.LastUpdated;
            cluster.Tracking.Status = trackingDto.Status;
            cluster.Tracking.Driver = trackingDto.DriverName ?? cluster.Tracking.Driver;

            cluster.UpdatedAt = DateTime.UtcNow;

            _clusterRepo.Update(cluster);

            var dto = DeliveryClusterMapper.ToDTO(cluster);
            return new GeneralResponse<DeliveryClusterDTO>()
            {
                Success = true,
                Message = "Tracking updated successfully.",
                Data = dto
            };
        }


        public async Task<GeneralResponse<bool>> DeleteClusterAsync(string clusterId)
        {
            var entity = await _clusterRepo.GetByIdAsync(clusterId);
            if (entity == null)
                return new GeneralResponse<bool> { Success = false, Message = "Cluster not found." };

            _clusterRepo.Remove(entity);
            await _clusterRepo.SaveChangesAsync();

            return new GeneralResponse<bool>
            {
                Success = true,
                Message = "Cluster deleted successfully.",
                Data = true
            };
        }

        public async Task<GeneralResponse<DeliveryClusterTrackingDTO>> GetTrackingAsync(string clusterId)
        {
            var entity = await _clusterRepo.GetByIdAsync(clusterId);
            if (entity?.Tracking == null)
                return new GeneralResponse<DeliveryClusterTrackingDTO> { Success = false, Message = "Tracking not found." };

            return new GeneralResponse<DeliveryClusterTrackingDTO>
            {
                Success = true,
                Data = new DeliveryClusterTrackingDTO
                {
                    Location = entity.Tracking.Location,
                    LastUpdated = entity.Tracking.LastUpdated,
                    Status = entity.Tracking.Status
                }
            };
        }

        public async Task<GeneralResponse<DeliveryClusterDTO>> AssignDriverAsync(string clusterId, string driverId)
        {
            var entity = await _clusterRepo.GetByIdAsync(clusterId);
            if (entity == null)
                return new GeneralResponse<DeliveryClusterDTO> { Success = false, Message = "Cluster not found." };

            entity.AssignedDriverId = driverId;
            _clusterRepo.Update(entity);
            await _clusterRepo.SaveChangesAsync();

            return new GeneralResponse<DeliveryClusterDTO>
            {
                Success = true,
                Message = "Driver assigned successfully.",
                Data = DeliveryClusterMapper.ToDTO(entity)
            };
        }

        public async Task<GeneralResponse<int>> BulkAssignDriverAsync(IEnumerable<string> clusterIds, string driverId)
        {
            var clusters = await _clusterRepo.FindAsync(c => clusterIds.Contains(c.Id));
            foreach (var cluster in clusters)
                cluster.AssignedDriverId = driverId;

            await _clusterRepo.UpdateRangeAsync(clusters);
            await _clusterRepo.SaveChangesAsync();

            return new GeneralResponse<int>
            {
                Success = true,
                Message = "Driver assigned to multiple clusters.",
                Data = clusters.Count()
            };
        }

        public async Task<GeneralResponse<IEnumerable<DeliveryClusterDTO>>> GetUnassignedClustersAsync()
        {
            var clusters = await _clusterRepo.FindAsync(
                c => c.DeliveryId == null
            );

            var dtoList = clusters.Select(DeliveryClusterMapper.ToDTO).ToList();

            return new GeneralResponse<IEnumerable<DeliveryClusterDTO>>
            {
                Success = true,
                Data = dtoList
            };
        }
    }
}
