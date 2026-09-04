using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.Exceptions;

public sealed class BookingConflictException : Exception
{
    public BookingConflictException(string message)
        : base(message)
    {
    }
}

