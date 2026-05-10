using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class CategoryRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Category>(context), ICategoryRepository
    {
        public async override Task<List<Category>> GetAllAsync()
        {
            try
            {
                var categories = await context.Set<Category>()
                    .ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching categories: {ex.Message}", ex);
            }
        }
        public async Task<ICollection<Category>> GetCategoriesWithIdsAsync(List<int> ids)
        {
            try
            {
                var categories = await context.Set<Category>()
                    .Where(c => ids.Contains(c.Id))
                    .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching categories: {ex.Message}", ex);
            }
        }

        public async override Task<Category> GetByIdAsync(int id)
        {
            try
            {
                var category = await context.Set<Category>()
               .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found.");
                }

                return category;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching the category with ID {id}: {ex.Message}", ex);
            }
        }
    }
}