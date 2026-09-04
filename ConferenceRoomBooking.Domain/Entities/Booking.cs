using System;
using System.Collections.Generic;
using System.Text;
using ConferenceRoomBooking.Domain.Enums;

namespace ConferenceRoomBooking.Domain.Entities;

public class Booking
{
    public Guid Id { get; private set; }

    public Guid RoomId { get; private set; }

    public DateTime StartAt { get; private set; }

    public DateTime EndAt { get; private set; }

    public decimal TotalPrice { get; private set; }

    public BookingStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Booking()
    {
    }

    public Booking(
        Guid roomId,
        DateTime startAt,
        DateTime endAt,
        decimal totalPrice)
    {
        if (startAt >= endAt)
            throw new ArgumentException("Start time must be before end time.");

        Id = Guid.NewGuid();
        RoomId = roomId;
        StartAt = startAt;
        EndAt = endAt;
        TotalPrice = totalPrice;
        Status = BookingStatus.Confirmed;
        CreatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = BookingStatus.Cancelled;
    }
}