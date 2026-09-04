using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Bookings;

public sealed record BookingResponse(
    Guid Id,
    Guid RoomId,
    DateTime StartAt,
    DateTime EndAt,
    decimal TotalPrice,
    string Status);