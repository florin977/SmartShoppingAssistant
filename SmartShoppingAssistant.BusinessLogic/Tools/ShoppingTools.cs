using SmartShoppingAssistant.BusinessLogic.DTOs.PromotionDTOs;
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
}