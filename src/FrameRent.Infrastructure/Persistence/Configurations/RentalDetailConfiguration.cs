using FrameRent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrameRent.Infrastructure.Persistence.Configurations;

public class RentalDetailConfiguration : IEntityTypeConfiguration<RentalDetail>
{
    public void Configure(EntityTypeBuilder<RentalDetail> builder)
    {
        builder.ToTable("RentalDetails");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.RentalId)
            .IsRequired();

        builder.Property(d => d.EquipmentId)
            .IsRequired();

        builder.Property(d => d.Quantity)
            .IsRequired();

        builder.Property(d => d.DailyRate)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(d => d.RentalDays)
            .IsRequired();

        builder.Property(d => d.Subtotal)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne<Equipment>()
            .WithMany()
            .HasForeignKey(d => d.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Rental>()
            .WithMany(r => r.Details)
            .HasForeignKey(d => d.RentalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
