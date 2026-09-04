using System;
using System.Collections.Generic;
using System.Text;

using ConferenceRoomBooking.Application.DTOs.Rooms;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IRoomService
{
    Task<RoomResponse> CreateAsync(
        CreateRoomRequest request,
        CancellationToken cancellationToken);

    Task<RoomResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<RoomResponse> UpdateAsync(
        Guid id,
        UpdateRoomRequest request,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<RoomResponse>> GetAvailableAsync(
        DateTime startAt,
        DateTime endAt,
        int capacity,
        CancellationToken cancellationToken);
}