using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Reports;

public sealed record RevenueReportResponse(
    decimal TotalRevenue,
    int BookingCount,
    decimal AverageBookingValue);
