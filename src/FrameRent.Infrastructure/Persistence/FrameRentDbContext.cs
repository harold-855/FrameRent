
using FrameRent.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FrameRent.Infrastructure.Persistence;

public class FrameRentDbContext: DbContext
{
    public FrameRentDbContext(
        DbContextOptions<FrameRentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<RentalDetail> RentalDetails => Set<RentalDetail>();
    public DbSet<Penalty> Penalties => Set<Penalty>();
}