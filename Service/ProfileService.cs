using Core.DTOs;
using Core.DTOs.CartDTOs;
using Core.DTOs.DeliveryDTOs;
using Core.DTOs.MaintenanceDTOs;
using Core.DTOs.OrderDTOs;
using Core.DTOs.PCAssemblyDTOs;
using Core.DTOs.ProductDTOs;
using Core.DTOs.ProfileDTOs;
using Core.DTOs.WishListDTOs;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechpertsSolutions.Core.Entities;

namespace Service
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<TechCompany> _techCompanyRepo;
        private readonly IRepository<DeliveryPerson> _deliveryPersonRepo;

        public ProfileService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IRepository<Customer> customerRepo,
            IRepository<TechCompany> techCompanyRepo,
            IRepository<DeliveryPerson> deliveryPersonRepo
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _customerRepo = customerRepo;
            _techCompanyRepo = techCompanyRepo;
            _deliveryPersonRepo = deliveryPersonRepo;
        }

        public async Task<GeneralResponse<IEnumerable<GeneralProfileReadDTO>>> GetAllProfilesAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();

                var profileDtos = new List<GeneralProfileReadDTO>();
                foreach (var u in users)
                {
                    var roles = await _userManager.GetRolesAsync(u);
                    profileDtos.Add(MapToProfileDTO(u, roles));
                }

                return new GeneralResponse<IEnumerable<GeneralProfileReadDTO>>
                {
                    Success = true,
                    Message = "Profiles retrieved successfully.",
                    Data = profileDtos,
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<GeneralProfileReadDTO>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving profiles: {ex.Message}",
                    Data = null,
                };
            }
        }

        public async Task<GeneralResponse<GeneralProfileReadDTO>> GetProfileByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new GeneralResponse<GeneralProfileReadDTO>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null,
                    };
                }

                var roles = await _userManager.GetRolesAsync(user);
                var dto = MapToProfileDTO(user, roles);

                return new GeneralResponse<GeneralProfileReadDTO>
                {
                    Success = true,
                    Message = "Profile retrieved successfully.",
                    Data = dto,
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<GeneralProfileReadDTO>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving profile: {ex.Message}",
                    Data = null,
                };
            }
        }

        //public async Task<GeneralResponse<CustomerProfileDTO>> GetCustomerRelatedInfoAsync(string userId)
        //{
        //    try
        //    {
        //        var customer = await _customerRepo.GetFirstOrDefaultAsync(
        //            c => c.UserId == userId,
        //                 query => query
        //                .Include(c => c.Cart)
        //                .Include(c => c.WishList)
        //                .Include(c => c.PCAssembly)
        //                .Include(c => c.Orders)
        //                .Include(c => c.Deliveries)
        //                .Include(c => c.Maintenances)
        //                .Include(c => c.User)
        //        );

        //        if (customer == null)
        //        {
        //            return new GeneralResponse<CustomerProfileDTO>
        //            {
        //                Success = false,
        //                Message = "Customer not found"
        //            };
        //        }

        //        var dto = new CustomerProfileDTO
        //        {
        //            UserId = customer.UserId,
        //            FullName = customer.User?.FullName,
        //            Cart = customer.Cart != null ? new CartReadDTO(customer.Cart) : null,
        //            WishList = customer.WishList != null ? new WishListReadDTO(customer.WishList) : null,
        //            PCAssemblies = customer.PCAssembly?.Select(x => new PCAssemblyReadDTO(x)).ToList(),
        //            Orders = customer.Orders?.Select(x => new OrderReadDTO(x)).ToList(),
        //            Deliveries = customer.Deliveries?.Select(x => new DeliveryReadDTO(x)).ToList(),
        //            //Maintenances = customer.Maintenances?.Select(x => new MaintenanceDetailsDTO(x)).ToList()
        //        };

        //        return new GeneralResponse<CustomerProfileDTO>
        //        {
        //            Success = true,
        //            Message = "Customer profile retrieved successfully",
        //            Data = dto
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralResponse<CustomerProfileDTO>
        //        {
        //            Success = false,
        //            Message = $"Error while retrieving customer profile: {ex.Message}"
        //        };
        //    }
        //}

        //public async Task<GeneralResponse<TechCompanyProfileDTO>> GetTechCompanyRelatedInfoAsync(string userId)
        //{
        //    try
        //    {
        //        var company = await _techCompanyRepo.GetFirstOrDefaultAsync(
        //            c => c.UserId == userId,
        //                 query => query
        //                .Include(c => c.Products)
        //                .Include(c => c.Maintenances)
        //                .Include(c => c.Deliveries)
        //                .Include(c => c.PCAssemblies)
        //                .Include(c => c.User)
        //        );

        //        if (company == null)
        //        {
        //            return new GeneralResponse<TechCompanyProfileDTO>
        //            {
        //                Success = false,
        //                Message = "Tech company not found"
        //            };
        //        }

        //        var dto = new TechCompanyProfileDTO
        //        {
        //            UserId = company.UserId,
        //            FullName = company.User?.FullName,
        //            Website = company.Website,
        //            Description = company.Description,
        //            Products = company.Products?.Select(x => new ProductCardDTO(x)).ToList(),
        //            //Maintenances = company.Maintenances?.Select(x => new MaintenanceDetailsDTO(x)).ToList(),
        //            Deliveries = company.Deliveries?.Select(x => new DeliveryReadDTO(x)).ToList(),
        //            PCAssemblies = company.PCAssemblies?.Select(x => new PCAssemblyReadDTO(x)).ToList()
        //        };

        //        return new GeneralResponse<TechCompanyProfileDTO>
        //        {
        //            Success = true,
        //            Message = "Tech company profile retrieved successfully",
        //            Data = dto
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralResponse<TechCompanyProfileDTO>
        //        {
        //            Success = false,
        //            Message = $"Error while retrieving tech company profile: {ex.Message}"
        //        };
        //    }
        //}

        //public async Task<GeneralResponse<DeliveryPersonProfileDTO>> GetDeliveryPersonRelatedInfoAsync(string userId)
        //{
        //    try
        //    {
        //        var deliveryPerson = await _deliveryPersonRepo.GetFirstOrDefaultAsync(
        //            d => d.UserId == userId,
        //                 query => query
        //                .Include(d => d.Deliveries)
        //                .Include(d => d.Offers)
        //                .Include(d => d.User)
        //        );

        //        if (deliveryPerson == null)
        //        {
        //            return new GeneralResponse<DeliveryPersonProfileDTO>
        //            {
        //                Success = false,
        //                Message = "Delivery person not found"
        //            };
        //        }

        //        var dto = new DeliveryPersonProfileDTO
        //        {
        //            UserId = deliveryPerson.UserId,
        //            FullName = deliveryPerson.User?.FullName,
        //            VehicleNumber = deliveryPerson.VehicleNumber,
        //            VehicleType = deliveryPerson.VehicleType,
        //            VehicleImage = deliveryPerson.VehicleImage,
        //            License = deliveryPerson.License,
        //            IsAvailable = deliveryPerson.IsAvailable,
        //            LastOnline = deliveryPerson.LastOnline,
        //            Deliveries = deliveryPerson.Deliveries?.Select(x => new DeliveryDTO(x)).ToList(),
        //            Offers = deliveryPerson.Offers?.Select(x => new DeliveryOfferDTO(x)).ToList()
        //        };

        //        return new GeneralResponse<DeliveryPersonProfileDTO>
        //        {
        //            Success = true,
        //            Message = "Delivery person profile retrieved successfully",
        //            Data = dto
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralResponse<DeliveryPersonProfileDTO>
        //        {
        //            Success = false,
        //            Message = $"Error while retrieving delivery person profile: {ex.Message}"
        //        };
        //    }
        //}

        private GeneralProfileReadDTO MapToProfileDTO(AppUser u, IList<string> roles)
        {
            return new GeneralProfileReadDTO
            {
                UserId = u.Id,
                UserName = u.UserName ?? string.Empty,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,

                FullName = u.FullName,
                Address = u.Address,
                City = u.City,
                Country = u.Country,
                PostalCode = u.PostalCode,
                Latitude = u.Latitude,
                Longitude = u.Longitude,
                ProfilePhotoUrl = u.ProfilePhotoUrl,
                IsActive = u.IsActive,

                RoleNames = roles,

                // only if AppUser actually has these props
                CreatedAt = u.CreatedAt,
                LastLogin = u.LastLoginDate,
            };
        }
    }
}
