using Microsoft.Agents.AI;

namespace SmartShoppingAssistant.BusinessLogic.Agents.Interfaces
{
    public interface ISuggestionComposerAgent
    {
        ChatClientAgent Build(string cartJson, string promotionAnalysisJson, string availableCategoriesJson);
    }
}
