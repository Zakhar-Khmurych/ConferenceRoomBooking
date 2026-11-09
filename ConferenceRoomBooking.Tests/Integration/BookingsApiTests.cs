
using System.Net;
using System.Net.Http.Json;
using ConferenceRoomBooking.Application.DTOs.Bookings;
using ConferenceRoomBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceRoomBooking.Tests.Integration;

public sealed class BookingsApiTests
{
    [Fact]
    public async Task CreateBooking_WhenRequestIsValid_ReturnsCreated()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var roomId = await GetFirstRoomIdAsync(factory);

        var request = new CreateBookingRequest(
            roomId,
            new DateTime(2026, 12, 10, 10, 0, 0),
            new DateTime(2026, 12, 10, 11, 0, 0),
            Array.Empty<Guid>());

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/bookings",
            request);

        var booking = await response.Content
            .ReadFromJsonAsync<BookingResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(booking);
        Assert.NotEqual(Guid.Empty, booking.Id);
        Assert.Equal(roomId, booking.RoomId);
        Assert.Equal(request.StartAt, booking.StartAt);
        Assert.Equal(request.EndAt, booking.EndAt);
        Assert.True(booking.TotalPrice > 0);
    }

    [Fact]
    public async Task CreateBooking_WhenRoomIsAlreadyBooked_ReturnsConflict()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var roomId = await GetFirstRoomIdAsync(factory);

        var startAt = new DateTime(2026, 12, 11, 10, 0, 0);
        var endAt = new DateTime(2026, 12, 11, 11, 0, 0);

        var request = new CreateBookingRequest(
            roomId,
            startAt,
            endAt,
            Array.Empty<Guid>());

        // Act: перше бронювання
        var firstResponse = await client.PostAsJsonAsync(
            "/api/bookings",
            request);

        // Act: повторне бронювання того самого залу на той самий час
        var secondResponse = await client.PostAsJsonAsync(
            "/api/bookings",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    private static async Task<Guid> GetFirstRoomIdAsync(
        CustomWebApplicationFactory factory)
    {
        using var scope = factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var room = await db.Rooms
            .AsNoTracking()
            .FirstAsync();

        return room.Id;
    }
}
