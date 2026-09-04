using ConferenceRoomBooking.Application.DTOs.Bookings;
using ConferenceRoomBooking.Application.Exceptions;
using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Services;

public sealed class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IPriceCalculator _priceCalculator;

    public BookingService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IServiceRepository serviceRepository,
        IPriceCalculator priceCalculator)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _serviceRepository = serviceRepository;
        _priceCalculator = priceCalculator;
    }

    public async Task<BookingResponse> CreateAsync(
        CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        if (request.StartAt >= request.EndAt)
        {
            throw new ValidationException(
                "Start time must be before end time.");
        }

        var room = await _roomRepository.GetByIdAsync(
            request.RoomId,
            cancellationToken);

        if (room is null || !room.IsActive)
        {
            throw new NotFoundException(
                $"Room with id '{request.RoomId}' was not found.");
        }

        var hasConflict = await _bookingRepository.HasConflictAsync(
            request.RoomId,
            request.StartAt,
            request.EndAt,
            cancellationToken);

        if (hasConflict)
        {
            throw new BookingConflictException(
                "The room is already booked for the selected time period.");
        }

        var serviceIds = request.ServiceIds
            .Distinct()
            .ToArray();

        var services = serviceIds.Length == 0
            ? Array.Empty<Service>()
            : (await _serviceRepository.GetByIdsAsync(
                serviceIds,
                cancellationToken)).ToArray();

        if (services.Length != serviceIds.Length)
        {
            throw new NotFoundException(
                "One or more selected services were not found.");
        }

        if (services.Any(service => !service.IsActive))
        {
            throw new ValidationException(
                "One or more selected services are inactive.");
        }

        var servicePrices = services
            .Select(service => service.Price);

        var totalPrice = _priceCalculator.CalculateTotalPrice(
            room.BaseHourlyPrice,
            request.StartAt,
            request.EndAt,
            servicePrices);

        var booking = new Booking(
            request.RoomId,
            request.StartAt,
            request.EndAt,
            totalPrice);

        await _bookingRepository.AddAsync(
     booking,
     cancellationToken);

        var bookingServices = services
            .Select(service =>
                new Domain.Entities.BookingService(
                    booking.Id,
                    service.Id,
                    service.Price))
            .ToArray();

        if (bookingServices.Length > 0)
        {
            await _bookingRepository.AddServicesAsync(
                bookingServices,
                cancellationToken);
        }

        await _bookingRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(booking);
    }

    public async Task<BookingResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (booking is null)
        {
            throw new NotFoundException(
                $"Booking with id '{id}' was not found.");
        }

        return MapToResponse(booking);
    }

    private static BookingResponse MapToResponse(
        Booking booking)
    {
        return new BookingResponse(
            booking.Id,
            booking.RoomId,
            booking.StartAt,
            booking.EndAt,
            booking.TotalPrice,
            booking.Status.ToString());
    }
}