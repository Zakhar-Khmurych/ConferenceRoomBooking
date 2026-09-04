using System;
using System.Collections.Generic;
using System.Text;

using ConferenceRoomBooking.Application.DTOs.Reports;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IReportService
{
    Task<RevenueReportResponse> GetRevenueAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<OccupancyReportResponse>> GetOccupancyAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<PopularServiceResponse>> GetPopularServicesAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken);
}