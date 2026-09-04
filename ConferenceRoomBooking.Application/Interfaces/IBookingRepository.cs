using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<bool> HasConflictAsync(
        Guid roomId,
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Booking>> GetForPeriodAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken);

    Task AddAsync(
        Booking booking,
        CancellationToken cancellationToken);

    Task AddServicesAsync(
        IEnumerable<BookingService> bookingServices,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}