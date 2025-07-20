using Core.DTOs.Product;
using TechpertsSolutions.Core.DTOs;
using Core.Enums;
using Core.Entities;
using Core.Interfaces;
using TechpertsSolutions.Core.Entities;
using Core.Interfaces.Services;
using Service.Utilities;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepo;

        public ProductService(IRepository<Product> productRepo,
            IRepository<Specification> specRepo,
            IRepository<Warranty> warrantyRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<GeneralResponse<PaginatedDTO<ProductCardDTO>>> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 10,
            ProductPendingStatus? status = null,
            string? categoryId = null,
            string? subCategoryId = null,
            string? nameSearch = null,
            string? sortBy = null,
            bool sortDescending = false)
        {
            if (pageNumber < 1)
            {
                return new GeneralResponse<PaginatedDTO<ProductCardDTO>>
                {
                    Success = false,
                    Message = "Page number must be greater than 0.",
                    Data = null
                };
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return new GeneralResponse<PaginatedDTO<ProductCardDTO>>
                {
                    Success = false,
                    Message = "Page size must be between 1 and 100.",
                    Data = null
                };
            }

            if (!string.IsNullOrWhiteSpace(categoryId) && !Guid.TryParse(categoryId, out _))
            {
                return new GeneralResponse<PaginatedDTO<ProductCardDTO>>
                {
                    Success = false,
                    Message = "Invalid Category ID format. Expected GUID format.",
                    Data = null
                };
            }

            if (!string.IsNullOrWhiteSpace(subCategoryId) && !Guid.TryParse(subCategoryId, out _))
            {
                return new GeneralResponse<PaginatedDTO<ProductCardDTO>>
                {
                    Success = false,
                    Message = "Invalid SubCategory ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                var allProducts = await _productRepo.GetAllWithIncludesAsync(p=>p.Category, p => p.SubCategory, p => p.StockControlManager, p => p.TechManager);

                // Apply filters
                if (status.HasValue)
                    allProducts = allProducts.Where(p => p.status == status.Value);

                if (!string.IsNullOrWhiteSpace(categoryId))
                    allProducts = allProducts.Where(p => p.CategoryId == categoryId);

                if (!string.IsNullOrWhiteSpace(subCategoryId))
                    allProducts = allProducts.Where(p => p.SubCategoryId == subCategoryId);


                if (!string.IsNullOrWhiteSpace(nameSearch))
                    allProducts = allProducts.Where(p => p.Name.Contains(nameSearch, StringComparison.OrdinalIgnoreCase));

                // Sorting
                allProducts = sortBy?.ToLower() switch
                {
                    "price" => sortDescending ? allProducts.OrderByDescending(p => p.Price) : allProducts.OrderBy(p => p.Price),
                    "name" => sortDescending ? allProducts.OrderByDescending(p => p.Name) : allProducts.OrderBy(p => p.Name),
                    "stock" => sortDescending ? allProducts.OrderByDescending(p => p.Stock) : allProducts.OrderBy(p => p.Stock),
                    _ => allProducts.OrderBy(p => p.Id)
                };

                int totalItems = allProducts.Count();

                var items = allProducts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(ProductMapper.MapToProductCardDTO)
                    .ToList();

                var result = new PaginatedDTO<ProductCardDTO>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    Items = items
                };

                return new GeneralResponse<PaginatedDTO<ProductCardDTO>>
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PaginatedDTO<ProductCardDTO>>
                {
                    Success = false,
                    Message = $"An unexpected error occurred while retrieving products:{ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<IEnumerable<ProductDTO>>> GetProductsByCategoryAsync(string categoryId, int page = 1, int size = 10)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                return new GeneralResponse<IEnumerable<ProductDTO>>
                {
                    Success = false,
                    Message = "Category ID cannot be null or empty.",
                    Data = null
                };
            }

            if (!Guid.TryParse(categoryId, out _))
            {
                return new GeneralResponse<IEnumerable<ProductDTO>>
                {
                    Success = false,
                    Message = "Invalid Category ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                var products = await _productRepo.FindWithIncludesAsync(
                    p => p.CategoryId == categoryId,
                    p => p.Category,
                    p => p.SubCategory,
                    p => p.TechManager,
                    p => p.StockControlManager,
                    p => p.Specifications,
                    p => p.Warranties
                );

                var pagedProducts = products
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();

                return new GeneralResponse<IEnumerable<ProductDTO>>
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    Data = ProductMapper.MapToProductDTOList(pagedProducts)
                };
            }
            catch (Exception)
            {
                return new GeneralResponse<IEnumerable<ProductDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving products.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<ProductDTO>> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Product ID cannot be null or empty.",
                    Data = null
                };
            }

            if (!Guid.TryParse(id, out _))
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Invalid Product ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                var product = await _productRepo.GetByIdWithIncludesAsync(id,
                    p => p.Category, 
                    p => p.SubCategory, 
                    p => p.StockControlManager, 
                    p => p.TechManager,
                    p => p.StockControlManager.User,
                    p => p.TechManager.User,
                    p => p.Warranties,
                    p => p.Specifications);
                
                if (product == null)
                {
                    return new GeneralResponse<ProductDTO>
                    {
                        Success = false,
                        Message = $"Product with ID '{id}' not found.",
                        Data = null
                    };
                }

                return new GeneralResponse<ProductDTO>
                {
                    Success = true,
                    Message = "Product retrieved successfully.",
                    Data = ProductMapper.MapToProductDTO(product)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = $"An unexpected error occurred while retrieving the product: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<ProductDTO>> AddAsync(ProductCreateDTO dto)
        {
            if (dto == null)
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Product data cannot be null.",
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Product name is required.",
                    Data = null
                };
            }

            if (dto.Price <= 0)
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Product price must be greater than 0.",
                    Data = null
                };
            }

            if (dto.Stock < 0)
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Product stock cannot be negative.",
                    Data = null
                };
            }

            if (!string.IsNullOrWhiteSpace(dto.CategoryId) && !Guid.TryParse(dto.CategoryId, out _))
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Invalid Category ID format. Expected GUID format.",
                    Data = null
                };
            }

            if (!string.IsNullOrWhiteSpace(dto.SubCategoryId) && !Guid.TryParse(dto.SubCategoryId, out _))
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Invalid SubCategory ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                var product = ProductMapper.MapToProduct(dto);
                await _productRepo.AddAsync(product);
                await _productRepo.SaveChangesAsync();
                var addedProductWithIncludes = await _productRepo.GetByIdWithIncludesAsync(
                 product.Id, 
                 p => p.Category,
                 p => p.SubCategory,
                 p => p.StockControlManager,
                 p => p.TechManager,
                 p => p.StockControlManager.User,
                 p => p.TechManager.User,
                 p => p.Warranties,
                 p => p.Specifications
                );
                
                return new GeneralResponse<ProductDTO>
                {
                    Success = true,
                    Message = "Product created successfully.",
                    Data = ProductMapper.MapToProductDTO(product)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<ProductDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while creating the product.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<bool>> UpdateAsync(string id, ProductUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GeneralResponse<bool> { Success = false, Message = "Product ID cannot be null or empty.", Data = false };
            }

            if (!Guid.TryParse(id, out _))
            {
                return new GeneralResponse<bool> { Success = false, Message = "Invalid Product ID format. Expected GUID format.", Data = false };
            }

            if (dto == null)
            {
                return new GeneralResponse<bool> { Success = false, Message = "Update data cannot be null.", Data = false };
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return new GeneralResponse<bool> { Success = false, Message = "Product name is required.", Data = false };
            }

            if (dto.Price <= 0)
            {
                return new GeneralResponse<bool> { Success = false, Message = "Product price must be greater than 0.", Data = false };
            }

            if (dto.Stock < 0)
            {
                return new GeneralResponse<bool> { Success = false, Message = "Product stock cannot be negative.", Data = false };
            }

            try
            {
                var product = await _productRepo.GetByIdWithIncludesAsync(
                    id,
                    p => p.Category,
                    p => p.SubCategory,
                    p => p.StockControlManager,
                    p => p.TechManager,
                    p => p.StockControlManager.User,
                    p => p.TechManager.User,
                    p => p.Warranties,
                    p => p.Specifications
                );

                if (product == null)
                {
                    return new GeneralResponse<bool> { Success = false, Message = $"Product with ID '{id}' not found.", Data = false };
                }

                // Update product fields
                product.Name = dto.Name;
                product.Price = dto.Price;
                product.DiscountPrice = dto.DiscountPrice;
                product.Description = dto.Description;
                product.Stock = dto.Stock;
                product.CategoryId = dto.CategoryId;
                product.SubCategoryId = dto.SubCategoryId;
                product.TechManagerId = dto.TechManagerId;
                product.StockControlManagerId = dto.StockControlManagerId;
                product.status = dto.Status;
                product.ImageUrl = dto.ImageUrl;

                // Clear existing specs & warranties - Cascade will handle delete from DB
                product.Specifications?.Clear();
                product.Warranties?.Clear();

                // Add new specifications
                if (dto.Specifications != null)
                {
                    foreach (var specDto in dto.Specifications)
                    {
                        product.Specifications.Add(new Specification
                        {
                            Key = specDto.Key,
                            Value = specDto.Value,
                            ProductId = product.Id
                        });
                    }
                }

                // Add new warranties
                if (dto.Warranties != null)
                {
                    foreach (var warrantyDto in dto.Warranties)
                    {
                        product.Warranties.Add(new Warranty
                        {
                            Description = warrantyDto.Description,
                            StartDate = warrantyDto.StartDate,
                            EndDate = warrantyDto.EndDate,
                            ProductId = product.Id
                        });
                    }
                }

                _productRepo.Update(product);
                await _productRepo.SaveChangesAsync();

                return new GeneralResponse<bool> { Success = true, Message = "Product updated successfully.", Data = true };
            }
            catch (Exception)
            {
                return new GeneralResponse<bool> { Success = false, Message = "An unexpected error occurred while updating the product.", Data = false };
            }
        }

        public async Task<GeneralResponse<bool>> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Product ID cannot be null or empty.",
                    Data = false
                };
            }

            if (!Guid.TryParse(id, out _))
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Invalid Product ID format. Expected GUID format.",
                    Data = false
                };
            }

            try
            {
                var product = await _productRepo.GetByIdAsync(id);

                if (product == null)
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = $"Product with ID '{id}' not found.",
                        Data = false
                    };
                }

                _productRepo.Remove(product);
                await _productRepo.SaveChangesAsync();

                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = "Product deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = $"An unexpected error occurred while deleting the product: {ex.Message}",
                    Data = false
                };
            }
        }

    }
}
