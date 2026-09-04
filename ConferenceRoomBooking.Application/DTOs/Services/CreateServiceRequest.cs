using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Services;

public sealed record CreateServiceRequest(
    string Name,
    decimal Price);
