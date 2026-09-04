using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Room>> GetAvailableAsync(
        DateTime startAt,
        DateTime endAt,
        int capacity,
        CancellationToken cancellationToken);

    Task AddAsync(
        Room room,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}