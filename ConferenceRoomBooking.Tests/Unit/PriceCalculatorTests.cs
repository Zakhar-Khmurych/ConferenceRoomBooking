using ConferenceRoomBooking.Application.Services;

namespace ConferenceRoomBooking.Tests.Unit;

public sealed class PriceCalculatorTests
{
    private readonly PriceCalculator _calculator = new();

    [Theory]
    [InlineData(9, 10, 2000)]
    [InlineData(6, 7, 1800)]
    [InlineData(18, 19, 1600)]
    [InlineData(12, 13, 2300)]
    public void CalculateRoomPrice_ReturnsCorrectPrice(
        int startHour,
        int endHour,
        decimal expected)
    {

        var start = new DateTime(2026, 11, 10, startHour, 0, 0);
        var end = new DateTime(2026, 11, 10, endHour, 0, 0);


        var result = _calculator.CalculateRoomPrice(2000, start,end);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateTotalPrice_IncludesServicePrices()
    {
        var start = new DateTime(2026, 11, 10, 9, 0, 0);
        var end = new DateTime(2026, 11, 10, 10, 0, 0);

        var servicePrices = new decimal[] { 500, 300 };

        var result = _calculator.CalculateTotalPrice(2000, start, end, servicePrices);

        Assert.Equal(2800m, result);
    }

    [Fact]
    public void CalculateRoomPrice_WhenStartIsAfterEnd_ThrowsException()
    {
        
        var start = new DateTime(2026, 11, 10, 11, 0, 0);
        var end = new DateTime(2026, 11, 10, 10, 0, 0);

        
        Action action = () =>
        {
            _calculator.CalculateRoomPrice(2000, start, end);
        };
        
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void CalculateRoomPrice_WhenBookingCrossesPeakHour_UsesBothRates()
    {
        var start = new DateTime(2026, 11, 10, 11, 30, 0);
        var end = new DateTime(2026, 11, 10, 12, 30, 0);

        var result = _calculator.CalculateRoomPrice(2000m,start,end);

        Assert.Equal(2150m, result);
    }

    [Fact]
    public void CalculateRoomPrice_WhenBookingEndsAt23_ReturnsEveningPrice()
    {
        var start = new DateTime(2026, 11, 10, 22, 0, 0);
        var end = new DateTime(2026, 11, 10, 23, 0, 0);

        var result = _calculator.CalculateRoomPrice(2000m,start,end);

        Assert.Equal(1600m, result);
    }
}
