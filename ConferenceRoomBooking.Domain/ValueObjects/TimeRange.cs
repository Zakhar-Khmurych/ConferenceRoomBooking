using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Domain.ValueObjects;

public sealed class TimeRange
{
    public DateTime Start { get; }

    public DateTime End { get; }

    public TimeSpan Duration => End - Start;

    public TimeRange(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException(
                "Start must be earlier than end.");

        Start = start;
        End = end;
    }

    public bool Overlaps(TimeRange other)
    {
        return Start < other.End &&
               End > other.Start;
    }
}