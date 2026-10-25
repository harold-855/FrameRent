using FrameRent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrameRent.Infrastructure.Persistence.Configurations;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("Rentals");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.CustomerId)
            .IsRequired();

        builder.Property(r => r.StartDate)
            .IsRequired();

        builder.Property(r => r.EndDate)
            .IsRequired();

        builder.Property(r => r.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Details)
            .WithOne()
            .HasForeignKey(d => d.RentalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(r => r.Details)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
