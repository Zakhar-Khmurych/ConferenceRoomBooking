using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Reports;

public sealed record PopularServiceResponse(
    Guid ServiceId,
    string ServiceName,
    int BookingCount,
    decimal Revenue);