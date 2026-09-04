using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        AppDbContext db,
        CancellationToken cancellationToken = default)
    {
        await db.Database.MigrateAsync(cancellationToken);

        if (await db.Rooms.AnyAsync(cancellationToken))
        {
            return;
        }

        var rooms = new[]
        {
            new Room("Зал А", 50, 2000m),
            new Room("Зал B", 100, 3500m),
            new Room("Зал C", 30, 1500m)
        };

        var services = new[]
        {
            new Service("Проєктор", 500m),
            new Service("Wi-Fi", 300m),
            new Service("Звук", 700m)
        };

        await db.Rooms.AddRangeAsync(
            rooms,
            cancellationToken);

        await db.Services.AddRangeAsync(
            services,
            cancellationToken);

        await db.SaveChangesAsync(cancellationToken);
    }
}