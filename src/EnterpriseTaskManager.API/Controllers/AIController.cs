using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnterpriseTaskManager.API.Interfaces;

namespace EnterpriseTaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AIController : ControllerBase
{
    private readonly IAIProvider _aiProvider;
    private readonly ILogger<AIController> _logger;
    private readonly IConfiguration _configuration;

    public AIController(IAIProvider aiProvider, ILogger<AIController> logger, IConfiguration configuration)
    {
        _aiProvider = aiProvider;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Smart Autofill: Suggest task fields based on description
    /// </summary>
    [HttpPost("suggest-fields")]
    public async Task<IActionResult> SuggestFields([FromBody] SuggestFieldsRequest request)
    {
        try
        {
            var minLength = _configuration.GetValue<int>("AI:MinDescriptionLengthForSuggestions");
            if (request.Description.Length < minLength)
            {
                return BadRequest(new
                {
                    message = $"Description must be at least {minLength} characters for AI suggestions"
                });
            }

            var suggestions = await _aiProvider.SuggestTaskFieldsAsync(request.Description);
            return Ok(suggestions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating AI suggestions");
            return StatusCode(500, new
            {
                message = "AI service is temporarily unavailable. Please try again later."
            });
        }
    }

    /// <summary>
    /// Health check for AI provider
    /// </summary>
    [HttpGet("health")]
    [AllowAnonymous]
    public IActionResult HealthCheck()
    {
        return Ok(new
        {
            provider = _aiProvider.ProviderName,
            status = "online",
            timestamp = DateTime.UtcNow
        });
    }
}

public class SuggestFieldsRequest
{
    public string Description { get; set; } = string.Empty;
}
