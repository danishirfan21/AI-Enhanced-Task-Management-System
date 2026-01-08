using EnterpriseTaskManager.API.Models.Entities;

namespace EnterpriseTaskManager.API.Models.DTOs.Responses;

public class AISuggestionResponse
{
    public string? SuggestedTitle { get; set; }
    public TaskPriority? SuggestedPriority { get; set; }
    public string? SuggestedCategory { get; set; }
    public string Confidence { get; set; } = "Medium"; // High, Medium, Low
    public int ProcessingTimeMs { get; set; }
}
