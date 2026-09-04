using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Services;

public sealed record ServiceResponse(
    Guid Id,
    string Name,
    decimal Price,
    bool IsActive);