using ConferenceRoomBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.Api.Controllers;

[ApiController]
[Route("api/reports")]
public sealed class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(
        IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> Revenue(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken)
    {
        var result =
            await _reportService.GetRevenueAsync(
                from,
                to,
                cancellationToken);

        return Ok(result);
    }

    [HttpGet("occupancy")]
    public async Task<IActionResult> Occupancy(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken)
    {
        var result =
            await _reportService.GetOccupancyAsync(
                from,
                to,
                cancellationToken);

        return Ok(result);
    }

    [HttpGet("services")]
    public async Task<IActionResult> Services(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken)
    {
        var result =
            await _reportService.GetPopularServicesAsync(
                from,
                to,
                cancellationToken);

        return Ok(result);
    }
}