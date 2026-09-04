using System;
using System.Collections.Generic;
using System.Text;

using ConferenceRoomBooking.Application.DTOs.Bookings;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(
        CreateBookingRequest request,
        CancellationToken cancellationToken);

    Task<BookingResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
}