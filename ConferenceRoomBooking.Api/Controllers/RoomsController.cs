using ConferenceRoomBooking.Application.DTOs.Rooms;
using ConferenceRoomBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.Api.Controllers;

[ApiController]
[Route("api/rooms")]
public sealed class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(RoomResponse),
        StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateRoomRequest request,
        CancellationToken cancellationToken)
    {
        var room = await _roomService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = room.Id },
            room);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(RoomResponse),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var room = await _roomService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(room);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(RoomResponse),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateRoomRequest request,
        CancellationToken cancellationToken)
    {
        var room = await _roomService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(room);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _roomService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable(
        [FromQuery] DateTime startAt,
        [FromQuery] DateTime endAt,
        [FromQuery] int capacity,
        CancellationToken cancellationToken)
    {
        var rooms = await _roomService.GetAvailableAsync(
            startAt,
            endAt,
            capacity,
            cancellationToken);

        return Ok(rooms);
    }
}