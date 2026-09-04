using System;
using System.Collections.Generic;
using System.Text;

using ConferenceRoomBooking.Application.Interfaces;

namespace ConferenceRoomBooking.Application.Services;

public sealed class PriceCalculator : IPriceCalculator
{
    public decimal CalculateRoomPrice(
        decimal hourlyPrice,
        DateTime startAt,
        DateTime endAt)
    {
        if (startAt >= endAt)
            throw new ArgumentException(
                "Start time must be earlier than end time.");

        decimal total = 0;

        var current = startAt;

        while (current < endAt)
        {
            var nextHour = current.AddHours(1);

            if (nextHour > endAt)
                nextHour = endAt;

            var duration = (decimal)(
                nextHour - current).TotalHours;

            var multiplier = GetMultiplier(
                current.TimeOfDay);

            total += hourlyPrice * multiplier * duration;

            current = nextHour;
        }

        return decimal.Round(
            total,
            2,
            MidpointRounding.AwayFromZero);
    }

    public decimal CalculateTotalPrice(
        decimal hourlyPrice,
        DateTime startAt,
        DateTime endAt,
        IEnumerable<decimal> servicePrices)
    {
        var roomPrice = CalculateRoomPrice(
            hourlyPrice,
            startAt,
            endAt);

        var servicesPrice = servicePrices.Sum();

        return decimal.Round(
            roomPrice + servicesPrice,
            2,
            MidpointRounding.AwayFromZero);
    }

    private static decimal GetMultiplier(
        TimeSpan time)
    {
        // Пікові години йдуть першими
        if (time >= TimeSpan.FromHours(12) &&
            time < TimeSpan.FromHours(14))
        {
            return 1.15m;
        }

        if (time >= TimeSpan.FromHours(6) &&
            time < TimeSpan.FromHours(9))
        {
            return 0.90m;
        }

        if (time >= TimeSpan.FromHours(18) &&
            time < TimeSpan.FromHours(23))
        {
            return 0.80m;
        }

        if (time >= TimeSpan.FromHours(9) &&
            time < TimeSpan.FromHours(18))
        {
            return 1.00m;
        }

        throw new ArgumentException(
            "Booking time must be between 06:00 and 23:00.");
    }
}