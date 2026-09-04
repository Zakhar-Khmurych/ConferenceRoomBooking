using System;
using System.Collections.Generic;
using System.Text;

using ConferenceRoomBooking.Application.DTOs.Services;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IServiceService
{
    Task<ServiceResponse> CreateAsync(
        CreateServiceRequest request,
        CancellationToken cancellationToken);

    Task<ServiceResponse> UpdateAsync(
        Guid id,
        UpdateServiceRequest request,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken);
}