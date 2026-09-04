using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Bookings;

public sealed record CreateBookingRequest(
    Guid RoomId,
    DateTime StartAt,
    DateTime EndAt,
    IReadOnlyCollection<Guid> ServiceIds);
