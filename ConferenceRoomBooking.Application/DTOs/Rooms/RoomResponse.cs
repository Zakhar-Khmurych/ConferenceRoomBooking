namespace ConferenceRoomBooking.Application.DTOs.Rooms;

public sealed record RoomResponse(
    Guid Id,
    string Name,
    int Capacity,
    decimal BaseHourlyPrice,
    bool IsActive);