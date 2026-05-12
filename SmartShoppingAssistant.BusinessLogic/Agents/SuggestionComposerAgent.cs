using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BusinessLogic.Agents.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Tools;
using System.ComponentModel;

namespace SmartShoppingAssistant.BusinessLogic.Agents
{
    public class SuggestionComposerAgent(IChatClient chatClient, IProductService productService) : ISuggestionComposerAgent
    {
        public ChatClientAgent Build(string cartJson, string availableCategoriesJson)
        {
            return new ChatClientAgent(
            chatClient,
            new ChatClientAgentOptions
            {
                Name = "SuggestionComposer",
                Description = "Generates product suggestions based on cart contents, categories, and near-miss promotions.",
                ChatOptions = new ChatOptions
                {
                    Instructions = $"""
                        You are a highly efficient e-commerce recommendation engine.

                        INPUT DATA:
                        - User Cart: {cartJson}
                        - Available Categories & Products: {availableCategoriesJson}
                        - The promotion analysis JSON from the previous AI.

                        CORE EVALUATION STEPS:
                        1. Analyze the 'nearMissDeals' from the previous AI's JSON. Prioritize finding products that fulfill the 'action' required to activate these deals (e.g., if the action says "Add 8.00 RON from Category Y", find products in Category Y around 8.00 RON). Also include the quantity needed to reach the promotion threshold.
                        2. If there are no near-misses, or you need more suggestions, look at the User Cart and suggest relevant cross-sell items from the Available Categories.
                        3. Limit your output to exactly a MAXIMUM of 5 suggestions.

                        ANTI-HALLUCINATION RULES (CRITICAL):
                        - You MUST ONLY suggest products that explicitly exist in the provided 'Available Categories & Products' data.
                        - Do NOT invent product names, IDs, or prices. 
                        - Use the exact 'productId', 'productName', and 'price' from the provided data.
                        - You can use an higher maxPrice when querying the products, it is ok to suggest products that are more expensive than the maxPrice in the near-miss action, but you cannot suggest products that are more expensive than the most expensive product in the available categories.
                        - You can make 2-3 requests to the SearchProducts tool with different pageSize and page parameters to explore more products, but remember to only suggest a maximum of 5 products in total.
                        - Use an high pageSize so you can get as many products as possible to choose from, but remember to only suggest a maximum of 5 products. I highly recommend a pageSize of 20.
                        - Calculate the total savings you can get for each promotion you suggest. Remember the savings is the difference between the original price and the price after applying the promotion.
                        The savings apply either to the entire category, the product itself or the cart itself, based on what combination of productId and categoryId the promotion has (if both are null, the promotion is considered cart-wide).
                        
                        """,
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<AnalysisResponse>(),
                    Tools =
                    [
                        AIFunctionFactory.Create(
                            ([Description("The exact ID of the category to search in. Use null if searching the entire store.")] int? categoryId,
                             [Description("The maximum price the user should pay to complete a near-miss promotion.")] decimal? maxPrice,
                             [Description("The page number for pagination.")] int page,
                             [Description("The page size for pagination.")] int pageSize
                             ) =>
                                ShoppingTools.SearchProducts(categoryId, maxPrice, page, pageSize, productService),
                            "SearchProducts",
                            "Searches the store for products. Use this to find items that fulfill near-miss promotions by specifying a categoryId and a maxPrice."
                        ),

                        AIFunctionFactory.Create(
                            ([Description("The exact ID of the product.")] int productId) =>
                                ShoppingTools.GetProductById(productId, productService),
                            "GetProductById",
                            "Gets the details of a specific product by its exact ID. Use this when you already know the product ID needed for a promotion."
                        )
                ]
                }
            },
            null!,
            null!
        );
        }
    }
}
