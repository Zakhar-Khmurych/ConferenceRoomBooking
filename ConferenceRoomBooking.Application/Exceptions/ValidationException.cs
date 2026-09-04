using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
    }
}
