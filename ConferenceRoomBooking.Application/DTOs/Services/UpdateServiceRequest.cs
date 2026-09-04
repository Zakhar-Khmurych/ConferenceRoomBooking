using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Services;

public sealed record UpdateServiceRequest(
    string Name,
    decimal Price);