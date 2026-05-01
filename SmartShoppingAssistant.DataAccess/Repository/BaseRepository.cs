using Microsoft.EntityFrameworkCore;

namespace SmartShoppingAssistant.DataAccess.Repository
{
    public class BaseRepository<TEntity>(SmartShoppingAssistantDbContext context) : IRepository<TEntity> where TEntity : class
    {
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
                }

                await context.Set<TEntity>().AddAsync(entity);

                await context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the entity: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await context.Set<TEntity>().FindAsync(id);

                if (entity == null)
                {
                    throw new Exception($"Entity with id {id} not found");
                }

                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the entity with id {id}: {ex.Message}", ex);
            }
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            try
            {
                var list = await context.Set<TEntity>().ToListAsync();

                if (list == null)
                {
                    throw new Exception("No entities found");
                }

                return list;

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving entities: {ex.Message}", ex);

            }
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                var entity = await context.Set<TEntity>().FindAsync(id);

                if (entity == null)
                {
                    throw new Exception($"Entity with id {id} not found");
                }

                return entity;

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the entity with {id}: {ex.Message}", ex);
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
                }

                context.Set<TEntity>().Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the entity: {ex.Message}", ex);
            }
        }
    }
}