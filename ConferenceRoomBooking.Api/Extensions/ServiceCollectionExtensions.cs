using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Application.Services;
using ConferenceRoomBooking.Infrastructure.Persistence;
using ConferenceRoomBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceRoomBooking.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IRoomService, RoomService>();

        services.AddScoped<
            IBookingService,
            ConferenceRoomBooking.Application.Services.BookingService>();

        services.AddScoped<
            IServiceService,
            ServiceService>();

        services.AddScoped<
            IPriceCalculator,
            PriceCalculator>();

        services.AddScoped<
            IReportService,
            ReportService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Default");

        services.AddDbContext<AppDbContext>(
            options =>
                options.UseSqlite(connectionString));

        services.AddScoped<
            IRoomRepository,
            RoomRepository>();

        services.AddScoped<
            IBookingRepository,
            BookingRepository>();

        services.AddScoped<
            IServiceRepository,
            ServiceRepository>();

        return services;
    }
}