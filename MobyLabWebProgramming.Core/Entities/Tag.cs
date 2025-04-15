namespace MobyLabWebProgramming.Core.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Relația many-to-many cu Place prin tabela PlaceTag.
    /// </summary>
    public ICollection<PlaceTag> PlaceTags { get; set; } = null!;
}