using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Parameters;
using SmartShoppingAssistant.DataAccess.Repository.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedResult<Product>> GetFilteredAsync(ProductQueryParameters productQueryParameters);
        Task<List<Product>> GetProductsWithIdsAsync(IEnumerable<int> ids);
    }
}
