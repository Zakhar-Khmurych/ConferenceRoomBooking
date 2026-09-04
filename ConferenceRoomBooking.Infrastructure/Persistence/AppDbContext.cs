using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<Booking> Bookings => Set<Booking>();

    public DbSet<BookingService> BookingServices => Set<BookingService>();

    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}