using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Reports;

public sealed record OccupancyReportResponse(
    Guid RoomId,
    string RoomName,
    double OccupancyPercent);
