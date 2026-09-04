using System;
using System.Collections.Generic;
using System.Text;

using ConferenceRoomBooking.Application.DTOs.Services;
using ConferenceRoomBooking.Application.Exceptions;
using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Services;

public sealed class ServiceService : IServiceService
{
    private readonly IServiceRepository _repository;

    public ServiceService(IServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse> CreateAsync(
        CreateServiceRequest request,
        CancellationToken cancellationToken)
    {
        Validate(request);

        var service = new Service(
            request.Name,
            request.Price);

        await _repository.AddAsync(
            service,
            cancellationToken);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return Map(service);
    }

    public async Task<ServiceResponse> UpdateAsync(
        Guid id,
        UpdateServiceRequest request,
        CancellationToken cancellationToken)
    {
        Validate(request);

        var service = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (service is null)
            throw new NotFoundException(
                $"Service with id '{id}' was not found.");

        service.Update(
            request.Name,
            request.Price);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return Map(service);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (service is null)
            throw new NotFoundException(
                $"Service with id '{id}' was not found.");

        service.Deactivate();

        await _repository.SaveChangesAsync(
            cancellationToken);
    }

    private static void Validate(
        CreateServiceRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException(
                "Service name is required.");

        if (request.Price < 0)
            throw new ValidationException(
                "Service price cannot be negative.");
    }

    private static void Validate(
        UpdateServiceRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException(
                "Service name is required.");

        if (request.Price < 0)
            throw new ValidationException(
                "Service price cannot be negative.");
    }

    private static ServiceResponse Map(Service service)
    {
        return new ServiceResponse(
            service.Id,
            service.Name,
            service.Price,
            service.IsActive);
    }
}