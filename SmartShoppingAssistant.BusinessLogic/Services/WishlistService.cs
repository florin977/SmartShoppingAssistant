using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.WishlistDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using SmartShoppingAssistant.DataAccess.Parameters;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class WishlistService(IWishlistRepository wishlistRepository, IMapper mapper, IProductRepository productRepository, IUserRepository userRepository) : IWishlistService
    {
        public async Task<WishlistItemGetDTO?> AddToWishlistAsync(WishlistItemPostDTO postDto, int userId)
        {
            var existing = await wishlistRepository.GetByUserAndProductAsync(userId, postDto.ProductId);
            if (existing != null) return mapper.Map<WishlistItemGetDTO>(existing);

            var product = await productRepository.GetByIdAsync(postDto.ProductId);
            if (product == null) throw new KeyNotFoundException("Product not found");

            var entity = mapper.Map<DataAccess.Entities.WishlistItem>(postDto);
            entity.UserId = userId;
            entity.AddedAt = DateTime.UtcNow;

            await wishlistRepository.AddAsync(entity);

            // load navigation
            entity.Product = product;
            return mapper.Map<WishlistItemGetDTO>(entity);
        }

        public async Task DeleteFromWishlistAsync(int wishlistItemId)
        {
            await wishlistRepository.DeleteAsync(wishlistItemId);
        }

        public async Task<PagedResult<WishlistItemGetDTO>> GetByUserIdAsync(int userId, PaginationQueryDTO paginationQuery)
        {
            var paginationParameters = mapper.Map<DataAccess.Repository.Parameters.PaginationParameters>(paginationQuery);
            var paged = await wishlistRepository.GetByUserIdAsync(userId, paginationParameters);
            var items = mapper.Map<IEnumerable<WishlistItemGetDTO>>(paged.Items);
            return new PagedResult<WishlistItemGetDTO>
            {
                Items = items,
                TotalCount = paged.TotalCount,
                TotalPages = paged.TotalPages
            };
        }

        public async Task<WishlistItemGetDTO?> GetByIdAsync(int id)
        {
            var entity = await wishlistRepository.GetByIdAsync(id);
            if (entity == null) return null;
            return mapper.Map<WishlistItemGetDTO>(entity);
        }

        public async Task<WishlistItemGetDTO?> GetByUserAndProductAsync(int userId, int productId)
        {
            var entity = await wishlistRepository.GetByUserAndProductAsync(userId, productId);
            if (entity == null) return null;
            return mapper.Map<WishlistItemGetDTO>(entity);
        }
    }
}
