using ConferenceRoomBooking.Application.DTOs.Reports;
using ConferenceRoomBooking.Application.Interfaces;

namespace ConferenceRoomBooking.Application.Services;

public sealed class ReportService : IReportService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;

    public ReportService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
    }

    public async Task<RevenueReportResponse> GetRevenueAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken)
    {
        ValidatePeriod(from, to);

        var bookings =
            await _bookingRepository.GetForPeriodAsync(
                from,
                to,
                cancellationToken);

        var confirmed = bookings
            .Where(x => x.Status ==
                Domain.Enums.BookingStatus.Confirmed)
            .ToList();

        var revenue = confirmed.Sum(
            x => x.TotalPrice);

        var average = confirmed.Count == 0
            ? 0
            : revenue / confirmed.Count;

        return new RevenueReportResponse(
            revenue,
            confirmed.Count,
            decimal.Round(average, 2));
    }

    public Task<IReadOnlyList<OccupancyReportResponse>>
        GetOccupancyAsync(
            DateTime from,
            DateTime to,
            CancellationToken cancellationToken)
    {
        // Реалізацію додамо після того,
        // як визначимо точне правило occupancy.
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<PopularServiceResponse>>
        GetPopularServicesAsync(
            DateTime from,
            DateTime to,
            CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private static void ValidatePeriod(
        DateTime from,
        DateTime to)
    {
        if (from >= to)
            throw new ArgumentException(
                "From must be earlier than To.");
    }
}