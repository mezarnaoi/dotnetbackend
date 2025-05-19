namespace MobyLabWebProgramming.Core.Entities;

public class Feedback
{
    public int Id { get; set; }
    public string Category { get; set; }
    public string Satisfaction { get; set; }
    public string Message { get; set; }
    public bool AllowContact { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
}