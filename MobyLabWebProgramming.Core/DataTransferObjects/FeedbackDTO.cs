namespace MobyLabWebProgramming.Core.DataTransferObjects;
using System.Text.Json.Serialization;

public class FeedbackDTO
{
    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("satisfaction")]
    public string Satisfaction { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("allowContact")]
    public bool AllowContact { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
}