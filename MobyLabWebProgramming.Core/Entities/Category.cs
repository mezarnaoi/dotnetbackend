namespace MobyLabWebProgramming.Core.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Place> Places { get; set; } = null!;
}