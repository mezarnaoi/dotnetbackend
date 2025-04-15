namespace MobyLabWebProgramming.Core.Entities;

public class Place : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    // Relația cu Category
    public Guid CategoryId { get; set; } // Foreign key
    public Category Category { get; set; } = null!; // Navigation property
    
    /// <summary>
    /// Relația cu entitatea Review - un loc poate avea mai multe recenzii.
    /// </summary>
    public ICollection<Review> Reviews { get; set; } = null!;
    public ICollection<PlaceTag> PlaceTags { get; set; } = null!; // Relație many-to-many cu Tag
}