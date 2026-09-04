using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IPriceCalculator
{
    decimal CalculateRoomPrice(
        decimal hourlyPrice,
        DateTime startAt,
        DateTime endAt);

    decimal CalculateTotalPrice(
        decimal hourlyPrice,
        DateTime startAt,
        DateTime endAt,
        IEnumerable<decimal> servicePrices);
}