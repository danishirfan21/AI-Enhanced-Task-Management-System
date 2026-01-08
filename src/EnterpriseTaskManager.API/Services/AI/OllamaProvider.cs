using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using EnterpriseTaskManager.API.Interfaces;
using EnterpriseTaskManager.API.Models.DTOs.Responses;
using EnterpriseTaskManager.API.Models.Entities;

namespace EnterpriseTaskManager.API.Services.AI;

public class OllamaProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OllamaProvider> _logger;
    private readonly string _endpoint;
    private readonly string _model;

    public string ProviderName => "Ollama";

    public OllamaProvider(HttpClient httpClient, IConfiguration configuration, ILogger<OllamaProvider> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _endpoint = configuration["AI:Ollama:Endpoint"] ?? "http://localhost:11434";
        _model = configuration["AI:Ollama:Model"] ?? "llama3.2:3b";

        _httpClient.Timeout = TimeSpan.FromSeconds(configuration.GetValue<int>("AI:Ollama:Timeout"));
    }

    public async Task<AISuggestionResponse> SuggestTaskFieldsAsync(string description)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var prompt = $@"Analyze this task description and suggest:
1. A concise title (max 80 characters)
2. Priority level (Low, Medium, High, or Urgent)
3. Best matching category (Bug, Feature, Enhancement, Documentation, or Support)

Task description: ""{description}""

Respond in this EXACT format:
TITLE: [your suggested title]
PRIORITY: [Low/Medium/High/Urgent]
CATEGORY: [Bug/Feature/Enhancement/Documentation/Support]
CONFIDENCE: [High/Medium/Low]";

            var requestBody = new
            {
                model = _model,
                prompt = prompt,
                stream = false,
                options = new
                {
                    temperature = 0.7,
                    num_predict = 150
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_endpoint}/api/generate", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonDocument.Parse(responseBody);
            var aiResponse = jsonResponse.RootElement.GetProperty("response").GetString() ?? "";

            stopwatch.Stop();

            // Parse AI response
            return ParseSuggestionResponse(aiResponse, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Ollama API");
            stopwatch.Stop();

            // Return fallback response
            return new AISuggestionResponse
            {
                SuggestedTitle = TruncateDescription(description, 80),
                SuggestedPriority = TaskPriority.Medium,
                SuggestedCategory = "Support",
                Confidence = "Low",
                ProcessingTimeMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }

    public async Task<string> GenerateSummaryAsync(string content)
    {
        try
        {
            var prompt = $@"Summarize this task thread in a brief, structured format. Include:
- Key points
- Important decisions
- Current blockers
- Next steps

Content:
{content}

Provide a concise summary in bullet points.";

            var requestBody = new
            {
                model = _model,
                prompt = prompt,
                stream = false,
                options = new
                {
                    temperature = 0.5,
                    num_predict = 200
                }
            };

            var httpContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_endpoint}/api/generate", httpContent);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonDocument.Parse(responseBody);
            return jsonResponse.RootElement.GetProperty("response").GetString() ?? "Unable to generate summary.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating summary with Ollama");
            return "Error generating AI summary. Please try again later.";
        }
    }

    private AISuggestionResponse ParseSuggestionResponse(string aiResponse, long elapsedMs)
    {
        var response = new AISuggestionResponse
        {
            ProcessingTimeMs = (int)elapsedMs
        };

        try
        {
            // Extract title
            var titleMatch = Regex.Match(aiResponse, @"TITLE:\s*(.+?)(?:\n|$)", RegexOptions.IgnoreCase);
            if (titleMatch.Success)
            {
                response.SuggestedTitle = titleMatch.Groups[1].Value.Trim();
            }

            // Extract priority
            var priorityMatch = Regex.Match(aiResponse, @"PRIORITY:\s*(\w+)", RegexOptions.IgnoreCase);
            if (priorityMatch.Success && Enum.TryParse<TaskPriority>(priorityMatch.Groups[1].Value, true, out var priority))
            {
                response.SuggestedPriority = priority;
            }

            // Extract category
            var categoryMatch = Regex.Match(aiResponse, @"CATEGORY:\s*(\w+)", RegexOptions.IgnoreCase);
            if (categoryMatch.Success)
            {
                response.SuggestedCategory = categoryMatch.Groups[1].Value.Trim();
            }

            // Extract confidence
            var confidenceMatch = Regex.Match(aiResponse, @"CONFIDENCE:\s*(\w+)", RegexOptions.IgnoreCase);
            if (confidenceMatch.Success)
            {
                response.Confidence = confidenceMatch.Groups[1].Value.Trim();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error parsing AI response: {Response}", aiResponse);
        }

        return response;
    }

    private string TruncateDescription(string description, int maxLength)
    {
        if (description.Length <= maxLength)
            return description;

        return description.Substring(0, maxLength - 3) + "...";
    }
}
