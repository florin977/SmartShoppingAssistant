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
        public ChatClientAgent Build(string cartJson, string promotionAnalysisJson, string availableCategoriesJson)
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
                        - Promotion Analysis: {promotionAnalysisJson}

                        CORE EVALUATION STEPS:
                        1. Analyze the 'nearMissDeals' from the Promotion Analysis. Prioritize finding products that fulfill the 'action' required to activate these deals (e.g., if the action says "Add 8.00 RON from Category Y", find products in Category Y around 8.00 RON).
                        2. If there are no near-misses, or you need more suggestions, look at the User Cart and suggest relevant cross-sell items from the Available Categories.
                        3. Limit your output to exactly a MAXIMUM of 5 suggestions.

                        ANTI-HALLUCINATION RULES (CRITICAL):
                        - You MUST ONLY suggest products that explicitly exist in the provided 'Available Categories & Products' data.
                        - Do NOT invent product names, IDs, or prices. 
                        - Use the exact 'productId', 'productName', and 'price' from the provided data.

                        OUTPUT REQUIREMENTS:
                        - You MUST return a structured JSON object with a single key: "suggestions".
                        - "suggestions" should be an array of objects with: "productId", "productName", "price", "reason" (why you suggest it, referencing the near-miss deal or cross-sell logic).
                        - Output ONLY raw, valid JSON. Do not include markdown blocks like ```json.
                        """,
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<SuggestionResult>(),
                    Tools =
                    [
                        AIFunctionFactory.Create(
                            ([Description("The exact ID of the category to search in. Use null if searching the entire store.")] int? categoryId,
                             [Description("The maximum price the user should pay to complete a near-miss promotion.")] decimal? maxPrice) =>
                                ShoppingTools.SearchProducts(categoryId, maxPrice, productService),
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
