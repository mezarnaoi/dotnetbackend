using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class PlaceTagConfiguration : IEntityTypeConfiguration<PlaceTag>
{
    public void Configure(EntityTypeBuilder<PlaceTag> builder)
    {
        // Cheie primară compusă
        builder.HasKey(pt => new { pt.PlaceId, pt.TagId });
        
        // Relația cu Place
        builder.HasOne(pt => pt.Place)
            .WithMany(p => p.PlaceTags)
            .HasForeignKey(pt => pt.PlaceId)
            .OnDelete(DeleteBehavior.Cascade); // Opțional
        
        // Relația cu Tag
        builder.HasOne(pt => pt.Tag)
            .WithMany(t => t.PlaceTags)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade); // Opțional
    }
}