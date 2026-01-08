namespace EnterpriseTaskManager.API.Models.Entities;

/// <summary>
/// Status of a task in the workflow
/// </summary>
public enum TaskStatus
{
    Todo,
    InProgress,
    Review,
    Done
}

/// <summary>
/// Priority level for task urgency
/// </summary>
public enum TaskPriority
{
    Low,
    Medium,
    High,
    Urgent
}

/// <summary>
/// Sentiment analysis result for customer feedback
/// </summary>
public enum SentimentType
{
    Positive,
    Neutral,
    Negative,
    Urgent
}

/// <summary>
/// User role for authorization
/// </summary>
public enum UserRole
{
    User,
    Manager,
    Admin
}
