using System;
using System.Collections.Generic;

namespace ConferenceRoomBooking.Application.DTOs.Rooms;

public sealed record UpdateRoomRequest(
    string Name,
    int Capacity,
    decimal BaseHourlyPrice,
    IReadOnlyCollection<Guid> ServiceIds);