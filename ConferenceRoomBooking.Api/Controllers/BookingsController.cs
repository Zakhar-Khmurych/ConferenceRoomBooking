using ConferenceRoomBooking.Application.DTOs.Bookings;
using ConferenceRoomBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public sealed class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(
        IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(BookingResponse),
        StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var booking =
            await _bookingService.CreateAsync(
                request,
                cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = booking.Id },
            booking);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var booking =
            await _bookingService.GetByIdAsync(
                id,
                cancellationToken);

        return Ok(booking);
    }
}