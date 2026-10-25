using FrameRent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrameRent.Infrastructure.Persistence.Configurations;

public class PenaltyConfiguration : IEntityTypeConfiguration<Penalty>
{
    public void Configure(EntityTypeBuilder<Penalty> builder)
    {
        builder.ToTable("Penalties");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.RentalId)
            .IsRequired();

        builder.Property(p => p.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.IsPaid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne<Rental>()
            .WithMany()
            .HasForeignKey(p => p.RentalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
