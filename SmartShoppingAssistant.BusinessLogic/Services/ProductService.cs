using AutoMapper;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper) : IProductService
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
        public async Task<IEnumerable<ProductGetDTO>> GetFilteredAsync(ProductQueryDTO productQuery)
        {
            var queryParameters = mapper.Map<DataAccess.Repository.Parameters.ProductQueryParameters>(productQuery);
            var filteredProducts = await productRepository.GetFilteredAsync(queryParameters);
            return mapper.Map<IEnumerable<ProductGetDTO>>(filteredProducts);
        }

        public async Task<ProductGetDTO> AddAsync(ProductPostDTO productPostDTO)
        {
            var product = mapper.Map<Product>(productPostDTO);
            // Get all the categories that match the provided category IDs and assign them to the product
            var categories = await categoryRepository.GetCategoriesWithIdsAsync(productPostDTO.CategoryIds);
            product.Categories = categories.ToList();

            var createdProduct = await productRepository.AddAsync(product);
            return mapper.Map<ProductGetDTO>(createdProduct);
        }

        public async Task<ProductGetDTO> UpdateAsync(int id, ProductPutDTO productPutDTO)
        {
            var queriedProduct = await productRepository.GetByIdAsync(id);
            mapper.Map(productPutDTO, queriedProduct);
            // Update the product's categories based on the provided category IDs
            queriedProduct.Categories.Clear(); // Clear existing categories
            var newCategories = await categoryRepository.GetCategoriesWithIdsAsync(productPutDTO.CategoryIds);
            foreach (var Category in newCategories) 
            {
                queriedProduct.Categories.Add(Category);
            }

            var updatedProduct = await productRepository.UpdateAsync(queriedProduct);
            return mapper.Map<ProductGetDTO>(updatedProduct);
        }

        public async Task DeleteAsync(int id)
        {
            await productRepository.DeleteAsync(id);
        }
    }
}