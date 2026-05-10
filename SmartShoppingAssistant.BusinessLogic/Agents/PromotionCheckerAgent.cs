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
                Description = "Analyzes carts for potential savings and near-miss deals.",
                ChatOptions = new ChatOptions
                {
                    Instructions = $"""
                        You are a high-precision retail logic engine mimicking a server-side PromotionEvaluator.
                        User Cart: {cartJson}

                        MAPPING LOGIC (Precedence: Product > Category > Global):
                        1. If 'productId' is present: Link the promotion ONLY to that specific item.
                        2. If 'productId' is missing BUT 'categoryId' is present: Aggregate all items in the cart belonging to that category and apply the promotion to that group.
                        3. If BOTH are missing: Apply the promotion to the entire cart total and total quantity.

                        CORE EVALUATION STEPS:
                        - Group items based on the Mapping Logic above.
                        - Check Eligibility: 
                            - 'Quantity' deals: (Group Quantity >= Threshold).
                            - 'CartTotal' deals: (Group Total Price >= Threshold).
                        - Identify Near-Misses: If not eligible, but user is within 2 units or $10.00 of the Threshold.

                        REWARD CALCULATION:
                        - 'PercentDiscount': Savings = (Group Total * RewardValue / 100).
                        - 'FreeItems': Savings = (RewardValue * Price of the cheapest item in the matched group).

                        ACTION FIELD INSTRUCTIONS:
                        - For 'activeDeals': The action should be "Already applied".
                        - For 'nearMissDeals': The action MUST be a specific instruction for the next AI. 
                          Example: "Add 1 more unit of ProductID [X] to qualify" or "Add $8.00 worth of items from CategoryID [Y] to qualify".
                        - This allows the next AI to effectively suggest specific products to the user.

                        OUTPUT REQUIREMENTS:
                        - You MUST use these exact JSON keys: "activeDeals" and "nearMissDeals".
                        - Do NOT use "AppliedPromotions" or "NearMissPromotions".
                        - "activeDeals" should be a list of objects with: "promotionId", "productId", "description", "action", and "savings".
                        - "nearMissDeals" should be a list of objects with: "promotionId", "productId", "description", "action", and "savings".
                        - Output ONLY raw JSON.
                        """,
                    // Google's free tier API does not allow function calls + formatting, therefore we will not parse the response as a JSON.
                    //ResponseFormat = ChatResponseFormat.ForJsonSchema<PromotionAnalysis>(),
                    Tools =
                    [
                        AIFunctionFactory.Create(
                            ([Description("An array of the product IDs currently in the user's cart.")] int[] productIds) =>
                                ShoppingTools.GetRelevantPromotions(productIds, promotionService),
                            "GetRelevantPromotions",
                            "Gets active promotions relevant to the products currently in the cart. Pass all product IDs from the cart as an array."
                        )
                    ]
                }
            },
            null!,
            null!
        );
    }
}