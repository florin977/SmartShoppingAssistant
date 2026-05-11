using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SmartShoppingAssistant.BusinessLogic.Models;

[Description("Product suggestions generated based on cart contents and near-miss promotions")]
public sealed class SuggestionResult
{
    [JsonPropertyName("suggestions")]
    public List<Suggestion> Suggestions { get; set; } = [];
}

public sealed class Suggestion
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; set; } = 0; // Google free tier does not allow null values in the response, so we will use 0 as the default value instead of making it nullable.

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty; // Suggestion reason, referencing the near-miss deal or cross-sell logic.
}