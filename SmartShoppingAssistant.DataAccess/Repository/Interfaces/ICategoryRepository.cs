using SmartShoppingAssistant.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.DataAccess.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<ICollection<Category>> GetCategoriesWithIdsAsync(List<int> ids);
    }
}
