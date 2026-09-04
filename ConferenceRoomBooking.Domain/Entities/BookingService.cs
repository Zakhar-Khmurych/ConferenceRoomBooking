namespace ConferenceRoomBooking.Domain.Entities;

public sealed class BookingService
{
    public Guid BookingId { get; private set; }

    public Guid ServiceId { get; private set; }

    public decimal Price { get; private set; }

    private BookingService()
    {
    }

    public BookingService(
        Guid bookingId,
        Guid serviceId,
        decimal price)
    {
        BookingId = bookingId;
        ServiceId = serviceId;
        Price = price;
    }
}