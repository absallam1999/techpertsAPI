using Core.DTOs.Category;
using TechpertsSolutions.Core.Entities;

namespace Service.Utilities
{
    public static class CategoryMapper
    {
        public static CategoryDTO MapToCategoryDTO(Category category)
        {
            if (category == null) return null;

            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                Description = category.Description
            };
        }

        public static Category MapToCategory(CategoryCreateDTO dto)
        {
            if (dto == null) return null;

            return new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image
            };
        }

        public static Category MapToCategory(CategoryUpdateDTO dto, Category existingCategory)
        {
            if (dto == null || existingCategory == null) return null;

            existingCategory.Name = dto.Name;
            existingCategory.Description = dto.Description;
            existingCategory.Image = dto.Image;
            return existingCategory;
        }

        public static IEnumerable<CategoryDTO> MapToCategoryDTOList(IEnumerable<Category> categories)
        {
            if (categories == null) return Enumerable.Empty<CategoryDTO>();

            return categories.Select(MapToCategoryDTO).Where(dto => dto != null);
        }
    }
} 