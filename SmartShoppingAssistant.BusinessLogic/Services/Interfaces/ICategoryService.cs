using SmartShoppingAssistant.BusinessLogic.DTOs.CategoryDTOs;
using SmartShoppingAssistant.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryGetDTO> GetByIdAsync(int id);
        Task<IEnumerable<CategoryGetDTO>> GetAllAsync();
        Task<CategoryGetDTO> AddAsync(CategoryPostDTO categoryPostDTO);
        Task<CategoryGetDTO> UpdateAsync(int id, CategoryPutDTO categoryPutDTO);
        Task DeleteAsync(int id);
    }
}
