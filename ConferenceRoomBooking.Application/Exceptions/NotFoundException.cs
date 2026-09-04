using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}