using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.QueryDTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using System.ComponentModel;

namespace SmartShoppingAssistant.BusinessLogic.Tools;

public static class ShoppingTools
{
    [Description("Gets active promotions relevant to the products currently in the cart, plus all store-wide deals.")]
    public static async Task<List<PromotionGetDTO>> GetRelevantPromotions(
        [Description("An array of the product IDs currently in the user's cart.")] int[] productIds,
        IPromotionService promotionService)
    {
        Console.WriteLine($"[TOOL CALLED] Checking promotions for IDs: {string.Join(",", productIds)}");

        var results = await promotionService.GetAllActivePromotionsForCart(productIds);

        Console.WriteLine($"[TOOL DATA] Successfully retrieved {results.Count} promotions from EF Core:");
        Console.WriteLine("----------------------------------------------------------------------");

        foreach (var p in results)
        {
            Console.WriteLine($"- ID: {p.Id} | Name: {p.Name} | Type: {p.Type} | Threshold: {p.Threshold} | ProductId: {p.ProductId} | CategoryId: {p.CategoryId}");
        }

        Console.WriteLine("----------------------------------------------------------------------\n");

        return results;
    }

    [Description("Searches the store for products. Use this to find items that fulfill near-miss promotions by specifying a categoryId and a maxPrice.")]
    public static async Task<List<object>> SearchProducts(
        [Description("The exact ID of the category to search in. Use null if searching the entire store.")] int? categoryId,
        [Description("The maximum price the user should pay to complete a near-miss promotion.")] decimal? maxPrice,
        [Description("The page number for pagination.")] int page,
        [Description("The page size for pagination.")] int pageSize,
        IProductService productService)
    {
        var queryParams = new ProductQueryDTO
        {
            CategoryId = categoryId,
            MaxPrice = maxPrice,
            Page = page,
            PageSize = pageSize,
            SortBy = "price",
            SortDirection = "asc"
        };

        var products = await productService.GetFilteredAsync(queryParams);

        return products.Select(p => new
        {
            p.Id,
            p.Name,
            p.Price
        }).ToList<object>();
    }

    [Description("Gets the details of a specific product by its exact ID. Use this when you already know the product ID needed for a promotion.")]
    public static async Task<object?> GetProductById(
    [Description("The exact ID of the product.")] int productId,
    IProductService productService)
    {
        var product = await productService.GetByIdAsync(productId);

        if (product == null) return null;

        return new
        {
            product.Id,
            product.Name,
            product.Price
        };
    }
}