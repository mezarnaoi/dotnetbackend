namespace MobyLabWebProgramming.Core.Entities;

public class Review : BaseEntity
{
    public string Content { get; set; } = null!;
    public int Rating { get; set; }
    
    /// <summary>
    /// Relația cu User - o recenzie aparține unui utilizator.
    /// </summary>
    public Guid UserId { get; set; } 
    public User User { get; set; } = null!; // Relație Many-To-One

    /// <summary>
    /// Relația cu Place - o recenzie este pentru un loc.
    /// </summary>
    public Guid PlaceId { get; set; }
    public Place Place { get; set; } = null!; // Relație Many-To-One
}