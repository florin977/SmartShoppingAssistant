using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using SmartShoppingAssistant.DataAccess.Repository.Parameters;

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
               .Include(p => p.Promotions.Where(pr => pr.IsActive))
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
                    .Include(p => p.Promotions.Where(pr => pr.IsActive))
                    .ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching products: {ex.Message}", ex);
            }
        }
        public async Task<IEnumerable<Product>> GetFilteredAsync(ProductQueryParameters productQueryParameters)
        {
            try
            {
                var query = context.Set<Product>()
                    .Include(p => p.Categories)
                    .Include(p => p.Promotions.Where(pr => pr.IsActive))
                    .AsQueryable();
                if (!string.IsNullOrEmpty(productQueryParameters.Search))
                {
                    query = query.Where(p => p.Name.Contains(productQueryParameters.Search) || p.Description.Contains(productQueryParameters.Search));
                }
                if (productQueryParameters.MinPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= productQueryParameters.MinPrice.Value);
                }
                if (productQueryParameters.MaxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= productQueryParameters.MaxPrice.Value);
                }
                if (productQueryParameters.CategoryId.HasValue)
                {
                    query = query.Where(p => p.Categories.Any(c => c.Id == productQueryParameters.CategoryId.Value));
                }
                if (!string.IsNullOrEmpty(productQueryParameters.SortBy))
                {
                    if (productQueryParameters.SortBy.Equals("price", StringComparison.OrdinalIgnoreCase))
                    {
                        query = "desc".Equals(productQueryParameters.SortDirection, StringComparison.OrdinalIgnoreCase)
                            ? query.OrderByDescending(p => p.Price)
                            : query.OrderBy(p => p.Price);
                    }
                    else if (productQueryParameters.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                    {
                        query = "desc".Equals(productQueryParameters.SortDirection, StringComparison.OrdinalIgnoreCase)
                            ? query.OrderByDescending(p => p.Name)
                            : query.OrderBy(p => p.Name);
                    }
                }

                // Pagination
                query = query
                    .Skip((productQueryParameters.Page - 1) * productQueryParameters.PageSize)
                    .Take(productQueryParameters.PageSize);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching filtered products: {ex.Message}", ex);
            }
        }
    }
}