﻿using Core.DTOs.Maintenance;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Core.Entities;

namespace Service
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IRepository<Maintenance> _maintenanceRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<TechCompany> _techCompanyRepo;
        private readonly IRepository<Warranty> _warrantyRepo;
        private readonly IRepository<ServiceUsage> _serviceUsageRepo;

        public MaintenanceService(
            IRepository<Maintenance> maintenanceRepo, IRepository<Customer> customerRepo, IRepository<TechCompany> techCompanyRepo, 
            IRepository<Warranty> warrantyRepo, IRepository<ServiceUsage> serviceUsageRepo)
        {
            _maintenanceRepo = maintenanceRepo;
            _customerRepo = customerRepo;
            _techCompanyRepo = techCompanyRepo;
            _warrantyRepo = warrantyRepo;
            _serviceUsageRepo = serviceUsageRepo;
        }

        public async Task<IEnumerable<MaintenanceDTO>> GetAllAsync()
        {
            var maintenances = await _maintenanceRepo.GetAllAsync();
            return maintenances.Select(m => new MaintenanceDTO
            {
                Id = m.Id
            });
        }

        public async Task<MaintenanceDetailsDTO?> GetByIdAsync(string id)
        {
            var maintenance = await _maintenanceRepo.GetByIdAsync(id);
            if (maintenance == null) return null;

            var warranty = maintenance.Warranty;
            var product = warranty?.Product;

            return MapToMaintenanceDetailsDTO(maintenance);
        }

        public async Task<MaintenanceDTO> AddAsync(MaintenanceCreateDTO dto)
        {
            var entity = new Maintenance
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = dto.CustomerId,
                TechCompanyId = dto.TechCompanyId,
                WarrantyId = dto.WarrantyId,
                ServiceUsageId = dto.ServiceUsageId
            };

            await _maintenanceRepo.AddAsync(entity);
            await _maintenanceRepo.SaveChangesAsync();

            var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
            var techCompany = await _techCompanyRepo.GetByIdAsync(dto.TechCompanyId);
            var warranty = await _warrantyRepo.GetByIdAsync(dto.WarrantyId);
            var serviceUsage = await _serviceUsageRepo.GetByIdAsync(dto.ServiceUsageId);

            return new MaintenanceDTO
            {
                Id = entity.Id,
                CustomerName = customer?.User?.FullName ?? "Unknown",
                TechCompanyName = techCompany?.User?.FullName ?? "Unknown",
                ProductName = warranty?.Product?.Name ?? "Unknown",
                ServiceType = serviceUsage?.ServiceType ?? "Unknown",
                WarrantyStart = warranty?.StartDate ?? DateTime.MinValue,
                WarrantyEnd = warranty?.EndDate ?? DateTime.MinValue
            };
        }


        //public async Task<bool> UpdateAsync(string id, MaintenanceUpdateDTO dto)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;

        //    var maintenance = await _maintenanceRepo.GetByIdAsync(id);
        //    if (maintenance == null) return false;

        //    if (await _customerRepo.GetByIdAsync(dto.CustomerId) == null) return false;
        //    if (await _techCompanyRepo.GetByIdAsync(dto.TechCompanyId) == null) return false;
        //    if (await _warrantyRepo.GetByIdAsync(dto.WarrantyId) == null) return false;
        //    if (await _serviceUsageRepo.GetByIdAsync(dto.ServiceUsageId) == null) return false;

        //    maintenance.CustomerId = dto.CustomerId;
        //    maintenance.TechCompanyId = dto.TechCompanyId;
        //    maintenance.WarrantyId = dto.WarrantyId;
        //    maintenance.ServiceUsageId = dto.ServiceUsageId;

        //    _maintenanceRepo.Update(maintenance);
        //    await _maintenanceRepo.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> UpdateAsync(string id, MaintenanceUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            var maintenance = await _maintenanceRepo.GetByIdAsync(id);
            if (maintenance == null) return false;

            var customerTask = _customerRepo.GetByIdAsync(dto.CustomerId);
            var techCompanyTask = _techCompanyRepo.GetByIdAsync(dto.TechCompanyId);
            var warrantyTask = _warrantyRepo.GetByIdAsync(dto.WarrantyId);
            var serviceUsageTask = _serviceUsageRepo.GetByIdAsync(dto.ServiceUsageId);

            await Task.WhenAll(customerTask, techCompanyTask, warrantyTask, serviceUsageTask);

            if (customerTask.Result == null || techCompanyTask.Result == null ||
                warrantyTask.Result == null || serviceUsageTask.Result == null)
            {
                return false;
            }

            maintenance.CustomerId = dto.CustomerId;
            maintenance.TechCompanyId = dto.TechCompanyId;
            maintenance.WarrantyId = dto.WarrantyId;
            maintenance.ServiceUsageId = dto.ServiceUsageId;

            _maintenanceRepo.Update(maintenance);
            await _maintenanceRepo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _maintenanceRepo.GetByIdAsync(id);
            if (entity == null)
                return false;

            _maintenanceRepo.Remove(entity);
            await _maintenanceRepo.SaveChangesAsync();
            return true;
        }

        private MaintenanceDetailsDTO MapToMaintenanceDetailsDTO(Maintenance maintenance)
        {
            return new MaintenanceDetailsDTO
            {
                Id = maintenance.Id,

                Customer = maintenance.Customer == null ? null : new MaintenanceCustomerDTO
                {
                    Id = maintenance.Customer.Id,
                    FullName = maintenance.Customer.User.FullName
                },

                TechCompany = maintenance.TechCompany == null ? null : new MaintenanceTechCompanyDTO
                {
                    Id = maintenance.TechCompany.Id,
                    FullName = maintenance.TechCompany.User.FullName
                },

                Warranty = maintenance.Warranty == null ? null : new MaintenanceWarrantyDTO
                {
                    Id = maintenance.Warranty.Id,
                    Description = maintenance.Warranty.Description,
                    StartDate = maintenance.Warranty.StartDate,
                    EndDate = maintenance.Warranty.EndDate,
                    ProductName = maintenance.Warranty.Product?.Name
                },

                Product = maintenance.Warranty?.Product == null ? null : new MaintenanceProductDTO
                {
                    Id = maintenance.Warranty.Product.Id,
                    Name = maintenance.Warranty.Product.Name,
                    Price = maintenance.Warranty.Product.Price
                },

                ServiceUsage = maintenance.serviceUsage == null ? null : new MaintenanceServiceUsageDTO
                {
                    Id = maintenance.serviceUsage.Id,
                    ServiceType = maintenance.serviceUsage.ServiceType,
                    UsedOn = maintenance.serviceUsage.UsedOn,
                    CallCount = maintenance.serviceUsage.CallCount
                }
            };
        }
    }
}
