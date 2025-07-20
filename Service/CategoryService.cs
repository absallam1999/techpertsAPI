using Core.DTOs.Category;
using Core.DTOs.Product;
using Core.Interfaces;
using Core.Interfaces.Services;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Core.DTOs;
using TechpertsSolutions.Core.Entities;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<CartItem> _cartItemRepository;

        public CategoryService(IRepository<Category> categoryRepository,
            IRepository<Product> productRepository,
            IRepository<CartItem> cartItemRepository) 
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<GeneralResponse<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllWithIncludesAsync(
                    c => c.SubCategories,
                    c => c.Products
                );

                return new GeneralResponse<IEnumerable<CategoryDTO>>
                {
                    Success = true,
                    Message = "Categories retrieved successfully.",
                    Data = CategoryMapper.MapToCategoryDTOList(categories)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<CategoryDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving categories.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<CategoryDTO>> GetCategoryByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GeneralResponse<CategoryDTO>
                {
                    Success = false,
                    Message = "Category ID cannot be null or empty.",
                    Data = null
                };
            }

            if (!Guid.TryParse(id, out _))
            {
                return new GeneralResponse<CategoryDTO>
                {
                    Success = false,
                    Message = "Invalid Category ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                var category = await _categoryRepository.GetByIdWithIncludesAsync(
                    id,
                    c => c.SubCategories,
                    c => c.Products
                );

                if (category == null)
                {
                    return new GeneralResponse<CategoryDTO>
                    {
                        Success = false,
                        Message = $"Category with ID '{id}' not found.",
                        Data = null
                    };
                }

                return new GeneralResponse<CategoryDTO>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = CategoryMapper.MapToCategoryDTO(category)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<CategoryDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving the category.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<CategoryDTO>> CreateCategoryAsync(CategoryCreateDTO categoryCreateDto)
        {
            if (categoryCreateDto == null)
            {
                return new GeneralResponse<CategoryDTO>
                {
                    Success = false,
                    Message = "Category data cannot be null.",
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(categoryCreateDto.Name))
            {
                return new GeneralResponse<CategoryDTO>
                {
                    Success = false,
                    Message = "Category name is required.",
                    Data = null
                };
            }

            try
            {
                var category = CategoryMapper.MapToCategory(categoryCreateDto);

                await _categoryRepository.AddAsync(category);
                await _categoryRepository.SaveChangesAsync();

                return new GeneralResponse<CategoryDTO>
                {
                    Success = true,
                    Message = "Category created successfully.",
                    Data = CategoryMapper.MapToCategoryDTO(category)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<CategoryDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while creating the category.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<bool>> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDto)
        {
            if (categoryUpdateDto == null)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Update data cannot be null.",
                    Data = false
                };
            }

            if (string.IsNullOrWhiteSpace(categoryUpdateDto.Id))
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Category ID is required.",
                    Data = false
                };
            }

            if (!Guid.TryParse(categoryUpdateDto.Id, out _))
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Invalid Category ID format. Expected GUID format.",
                    Data = false
                };
            }

            if (string.IsNullOrWhiteSpace(categoryUpdateDto.Name))
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Category name is required.",
                    Data = false
                };
            }

            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryUpdateDto.Id);
                if (category == null)
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = $"Category with ID '{categoryUpdateDto.Id}' not found.",
                        Data = false
                    };
                }

                CategoryMapper.MapToCategory(categoryUpdateDto, category);

                _categoryRepository.Update(category);
                await _categoryRepository.SaveChangesAsync();
                
                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = "Category updated successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "An unexpected error occurred while updating the category.",
                    Data = false
                };
            }
        }

        public async Task<GeneralResponse<bool>> DeleteCategoryAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new GeneralResponse<bool> { Success = false, Message = "Category ID is required.", Data = false };

            if (!Guid.TryParse(id, out _))
                return new GeneralResponse<bool> { Success = false, Message = "Invalid Category ID format.", Data = false };

            try
            {
                var category = await _categoryRepository.GetByIdWithIncludesAsync(id, c => c.Products);
                if (category == null)
                    return new GeneralResponse<bool> { Success = false, Message = "Category not found.", Data = false };

                foreach (var product in category.Products ?? new List<Product>())
                {
                    var cartItems = await _cartItemRepository.FindAsync(ci => ci.ProductId == product.Id);
                    foreach (var cartItem in cartItems)
                        _cartItemRepository.Remove(cartItem);

                    _productRepository.Remove(product);
                }

                _categoryRepository.Remove(category);

                await _cartItemRepository.SaveChangesAsync();
                await _productRepository.SaveChangesAsync();
                await _categoryRepository.SaveChangesAsync();

                return new GeneralResponse<bool> { Success = true, Message = "Category deleted successfully.", Data = true };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool> { Success = false, Message = $"Error occurred: {ex.Message}", Data = false };
            }
        }
    }
}
