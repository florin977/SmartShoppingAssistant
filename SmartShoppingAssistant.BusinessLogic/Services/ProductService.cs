using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class ProductService(IRepository<Product> productRepository, IMapper mapper) : IProductService
    {
        public async Task<ProductGetDTO> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            return mapper.Map<ProductGetDTO>(product);
        }

        public async Task<IEnumerable<ProductGetDTO>> GetAllAsync()
        {
            var products = await productRepository.GetAllAsync();

            return mapper.Map<IEnumerable<ProductGetDTO>>(products);
        }

        public async Task<ProductGetDTO> AddAsync(ProductPostDTO productPostDTO)
        {
            var product = mapper.Map<Product>(productPostDTO);
            var createdProduct = await productRepository.AddAsync(product);

            return mapper.Map<ProductGetDTO>(createdProduct);
        }

        public async Task<ProductGetDTO> UpdateAsync(int id, ProductPutDTO productUpdateDTO)
        {
            var queriedProduct = await productRepository.GetByIdAsync(id);

            queriedProduct.Name = productUpdateDTO.Name;
            queriedProduct.Description = productUpdateDTO.Description;
            queriedProduct.ImageUrl = productUpdateDTO.ImageUrl;
            queriedProduct.Price = productUpdateDTO.Price;
            queriedProduct.Stock = productUpdateDTO.Stock;

            var updatedProduct = await productRepository.UpdateAsync(queriedProduct);

            return mapper.Map<ProductGetDTO>(updatedProduct);
        }

        public async Task DeleteAsync(int id)
        {
            await productRepository.DeleteAsync(id);
        }
    }
}