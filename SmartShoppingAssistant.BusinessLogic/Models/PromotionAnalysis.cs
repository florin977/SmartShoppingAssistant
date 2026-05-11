using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SmartShoppingAssistant.BusinessLogic.Models;

[Description("Promotion analysis for the current cart")]
public sealed class PromotionAnalysis
{
    [JsonPropertyName("activeDeals")]
    public List<Deal> ActiveDeals { get; set; } = [];

    [JsonPropertyName("nearMissDeals")]
    public List<Deal> NearMissDeals { get; set; } = [];
}

public sealed class Deal
{
    [JsonPropertyName("promotionId")]
    public int PromotionId { get; set; }

    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty; // Changed from string? Google free tier does not allow null values in the response, so we will use an empty string instead.

    [JsonPropertyName("savings")]
    public decimal Savings { get; set; } = 0; // Changed from decimal? Google free tier does not allow null values in the response, so we will use 0 as the default value instead.
}