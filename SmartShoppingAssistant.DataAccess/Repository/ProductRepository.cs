using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class ProductRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Product>(context), IProductRepository
    {
        public async override Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var product = await context.Set<Product>()
               .Include(p => p.Categories)
               .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {id} not found.");
                }

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching the product with ID {id}: {ex.Message}", ex);
            }
        }
        public async override Task<List<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Set<Product>()
                    .Include(p => p.Categories)
                    .ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching products: {ex.Message}", ex);
            }
        }
    }
}
