using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.CategoryDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
    {
        public async Task<CategoryGetDTO> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            return mapper.Map<CategoryGetDTO>(category);
        }
        public async Task<IEnumerable<CategoryGetDTO>> GetAllAsync()
        {
            var categories = await categoryRepository.GetAllAsync();
            return mapper.Map<IEnumerable<CategoryGetDTO>>(categories);
        }
        public async Task<CategoryGetDTO> AddAsync(CategoryPostDTO categoryPostDTO)
        {
            var category = mapper.Map<Category>(categoryPostDTO);
            var addedCategory = await categoryRepository.AddAsync(category);
            return mapper.Map<CategoryGetDTO>(addedCategory);
        }
        public async Task<CategoryGetDTO> UpdateAsync(int id, CategoryPutDTO categoryPutDTO)
        {
            var queriedCategory = await categoryRepository.GetByIdAsync(id);
            mapper.Map(categoryPutDTO, queriedCategory);
            var updatedCategory = await categoryRepository.UpdateAsync(queriedCategory);
            return mapper.Map<CategoryGetDTO>(updatedCategory);
        }
        public async Task DeleteAsync(int id)
        {
            await categoryRepository.DeleteAsync(id);
        }
    }
}
