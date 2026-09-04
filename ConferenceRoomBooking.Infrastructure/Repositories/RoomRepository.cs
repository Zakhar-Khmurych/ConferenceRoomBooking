using System;
using System.Collections.Generic;
using System.Text;

using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Repositories;

public sealed class RoomRepository : IRoomRepository
{

    private readonly AppDbContext _db;

    public RoomRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Room?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _db.Rooms
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Room>> GetAvailableAsync(
        DateTime startAt,
        DateTime endAt,
        int capacity,
        CancellationToken cancellationToken)
    {
        var bookedRoomIds = await _db.Bookings
            .Where(x =>
                x.StartAt < endAt &&
                x.EndAt > startAt &&
                x.Status == Domain.Enums.BookingStatus.Confirmed)
            .Select(x => x.RoomId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return await _db.Rooms
            .Where(x =>
                x.IsActive &&
                x.Capacity >= capacity &&
                !bookedRoomIds.Contains(x.Id))
            .OrderBy(x => x.BaseHourlyPrice)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Room room,
        CancellationToken cancellationToken)
    {
        await _db.Rooms.AddAsync(
            room,
            cancellationToken);
    }

    public Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        return _db.SaveChangesAsync(
            cancellationToken);
    }
}