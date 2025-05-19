using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("Feedbacks");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Satisfaction)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(f => f.Message)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(f => f.AllowContact)
            .IsRequired();

        builder.Property(f => f.Email)
            .HasMaxLength(255);

        builder.Property(f => f.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}