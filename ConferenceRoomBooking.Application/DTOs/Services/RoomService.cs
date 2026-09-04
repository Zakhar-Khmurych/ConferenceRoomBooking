using System;
using System.Collections.Generic;
using System.Text;

using ConferenceRoomBooking.Application.DTOs.Rooms;
using ConferenceRoomBooking.Application.Exceptions;
using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Services;

public sealed class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<RoomResponse> CreateAsync(
        CreateRoomRequest request,
        CancellationToken cancellationToken)
    {
        Validate(request);

        var room = new Room(
            request.Name,
            request.Capacity,
            request.BaseHourlyPrice);

        await _roomRepository.AddAsync(
            room,
            cancellationToken);

        await _roomRepository.SaveChangesAsync(
            cancellationToken);

        return Map(room);
    }

    public async Task<RoomResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (room is null)
            throw new NotFoundException(
                $"Room with id '{id}' was not found.");

        return Map(room);
    }

    public async Task<RoomResponse> UpdateAsync(
        Guid id,
        UpdateRoomRequest request,
        CancellationToken cancellationToken)
    {
        Validate(request);

        var room = await _roomRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (room is null)
            throw new NotFoundException(
                $"Room with id '{id}' was not found.");

        room.Update(
            request.Name,
            request.Capacity,
            request.BaseHourlyPrice);

        await _roomRepository.SaveChangesAsync(
            cancellationToken);

        return Map(room);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (room is null)
            throw new NotFoundException(
                $"Room with id '{id}' was not found.");

        room.Deactivate();

        await _roomRepository.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<IReadOnlyList<RoomResponse>> GetAvailableAsync(
        DateTime startAt,
        DateTime endAt,
        int capacity,
        CancellationToken cancellationToken)
    {
        ValidatePeriod(startAt, endAt);

        if (capacity <= 0)
            throw new ValidationException(
                "Capacity must be greater than zero.");

        var rooms = await _roomRepository.GetAvailableAsync(
            startAt,
            endAt,
            capacity,
            cancellationToken);

        return rooms
            .Select(Map)
            .ToList();
    }

    private static void Validate(
        CreateRoomRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException(
                "Room name is required.");

        if (request.Capacity <= 0)
            throw new ValidationException(
                "Capacity must be greater than zero.");

        if (request.BaseHourlyPrice < 0)
            throw new ValidationException(
                "Base hourly price cannot be negative.");
    }

    private static void Validate(
        UpdateRoomRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException(
                "Room name is required.");

        if (request.Capacity <= 0)
            throw new ValidationException(
                "Capacity must be greater than zero.");

        if (request.BaseHourlyPrice < 0)
            throw new ValidationException(
                "Base hourly price cannot be negative.");
    }

    private static void ValidatePeriod(
        DateTime startAt,
        DateTime endAt)
    {
        if (startAt >= endAt)
            throw new ValidationException(
                "Start time must be earlier than end time.");
    }

    private static RoomResponse Map(Room room)
    {
        return new RoomResponse(
            room.Id,
            room.Name,
            room.Capacity,
            room.BaseHourlyPrice,
            room.IsActive);
    }
}