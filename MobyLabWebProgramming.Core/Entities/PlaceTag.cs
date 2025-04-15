namespace MobyLabWebProgramming.Core.Entities;

public class PlaceTag : BaseEntity
{
    public Guid PlaceId { get; set; }
    public Place Place { get; set; } = null!;
    
    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}