using EnterpriseTaskManager.API.Models.DTOs.Responses;

namespace EnterpriseTaskManager.API.Interfaces;

/// <summary>
/// AI provider abstraction for supporting multiple LLM backends
/// </summary>
public interface IAIProvider
{
    /// <summary>
    /// Suggests task fields based on description (Smart Autofill feature)
    /// </summary>
    Task<AISuggestionResponse> SuggestTaskFieldsAsync(string description);

    /// <summary>
    /// Generates a summary of a task and its comments
    /// </summary>
    Task<string> GenerateSummaryAsync(string content);

    /// <summary>
    /// Provider name (Ollama, OpenAI, etc.)
    /// </summary>
    string ProviderName { get; }
}
