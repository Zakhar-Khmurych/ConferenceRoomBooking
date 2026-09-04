using System;
using System.Collections.Generic;
using System.Text;

using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Domain.Enums;
using ConferenceRoomBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Repositories;

public sealed class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _db;

    public BookingRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Booking?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _db.Bookings
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<bool> HasConflictAsync(
        Guid roomId,
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken)
    {
        return await _db.Bookings.AnyAsync(
            x =>
                x.RoomId == roomId &&
                x.Status == BookingStatus.Confirmed &&
                x.StartAt < endAt &&
                x.EndAt > startAt,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetForPeriodAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken)
    {
        return await _db.Bookings
            .Where(x =>
                x.StartAt < to &&
                x.EndAt > from)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Booking booking,
        CancellationToken cancellationToken)
    {
        await _db.Bookings.AddAsync(
            booking,
            cancellationToken);
    }

    public async Task AddServicesAsync(
        IEnumerable<BookingService> bookingServices,
        CancellationToken cancellationToken)
    {
        await _db.BookingServices.AddRangeAsync(
            bookingServices,
            cancellationToken);
    }

    public Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        return _db.SaveChangesAsync(
            cancellationToken);
    }
}