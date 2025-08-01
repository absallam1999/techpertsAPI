﻿using Core.DTOs.Category;
using Core.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Core.DTOs;

namespace Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<GeneralResponse<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync();
        Task<GeneralResponse<CategoryDTO>> GetCategoryByIdAsync(string Id);
        Task<GeneralResponse<CategoryDTO>> CreateCategoryAsync(CategoryCreateDTO categoryDTO);
        Task<GeneralResponse<bool>> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDTO);
        Task<GeneralResponse<bool>> DeleteCategoryAsync(string Id);
    }
}
