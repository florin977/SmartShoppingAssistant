using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Tools;
using System.ComponentModel;

namespace SmartShoppingAssistant.BusinessLogic.Agents;

public class PromotionCheckerAgent(IChatClient chatClient, IPromotionService promotionService) : IPromotionCheckerAgent
{
    public ChatClientAgent Build(string cartJson)
    {
        return new ChatClientAgent(
            chatClient,
            new ChatClientAgentOptions
            {
                Name = "PromotionChecker",
                Description = "Analyzes carts for potential savings and calculates near-miss deals.",
                ChatOptions = new ChatOptions
                {
                    Instructions = $"""
                        You are a high-precision retail logic engine mimicking a server-side Strategy Pattern Promotion Evaluator.
                        You must evaluate promotions EXACTLY as the C# backend does.

                        CURRENT USER CART: 
                        {cartJson}

                        --- PHASE 1: EXACT BACKEND EVALUATION (THE "ACTIVE" DEALS) ---
                        You must evaluate promotions sequentially across 3 levels. Maintain a 'RunningTotal' (starting at the cart Subtotal).
                        If a promotion applies, subtract its savings from the 'RunningTotal' BEFORE moving to the next level.

                        1. PRODUCT LEVEL: Find all promotions with a specific 'productId'. 
                           - Pick the SINGLE best product promotion (highest savings). 
                           - Apply it and reduce RunningTotal by the savings.
                        2. CATEGORY LEVEL: Find all promotions with a 'categoryId'. 
                           - Evaluate them using the items in that category. 
                           - Pick the SINGLE best category promotion. 
                           - Apply it and reduce RunningTotal by the savings.
                        3. CART LEVEL: Find all global promotions (no productId/categoryId). 
                           - Evaluate against the CURRENT 'RunningTotal'. 
                           - Pick the SINGLE best cart promotion.

                        REWARD MATH:
                        - 'PercentDiscount': Savings = Applicable Total * (RewardValue / 100).
                        - 'FreeItems': Savings = RewardValue * Price of the cheapest applicable item.

                        --- PHASE 2: NEAR-MISS CALCULATION ---
                        Look at all the promotions that were NOT applied in Phase 1. 
                        A "Near Miss" occurs ONLY when the cart fails the threshold condition, but is very close.
                        
                        Boundaries for a valid Near Miss:
                        - 'Quantity' deals: The user is short by 1 or 2 items (Threshold - Current Quantity <= 2).
                        - 'CartTotal' deals: The user is short by 20.00 RON or less (Threshold - Current Applicable Total <= 20.00).

                        --- ACTION FIELD INSTRUCTIONS ---
                        - For 'activeDeals': The action must be "Already applied".
                        - For 'nearMissDeals': The action MUST be a specific, actionable instruction for the frontend or next AI to help the user qualify.
                           - Examples: 
                             - "Add 1 more unit of ProductID [X] to get 1 free!"
                             - "Add 15.00 RON worth of items from Category [Y] to unlock 10% off."
                             - "Spend 5.50 RON more to get 15% off your entire cart!"
                        """,
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<PromotionAnalysis>(),
                    Tools =
                    [
                        AIFunctionFactory.Create(
                            ([Description("An array of the product IDs currently in the user's cart.")] int[] productIds) =>
                                ShoppingTools.GetRelevantPromotions(productIds, promotionService),
                            "GetRelevantPromotions",
                            "Gets all active product, category, and cart-wide promotions relevant to the user's current items. Always pass the product IDs found in the cart."
                        )
                    ]
                }
            },
            null!,
            null!
        );
    }
}