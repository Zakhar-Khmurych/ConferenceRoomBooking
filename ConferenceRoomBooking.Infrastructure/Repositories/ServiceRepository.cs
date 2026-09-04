using System;
using System.Collections.Generic;
using System.Text;
using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Repositories;

public sealed class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _db;

    public ServiceRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Service?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _db.Services
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Service>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken)
    {
        var serviceIds = ids.Distinct().ToList();

        return await _db.Services
            .Where(x => serviceIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Service service,
        CancellationToken cancellationToken)
    {
        await _db.Services.AddAsync(
            service,
            cancellationToken);
    }

    public Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        return _db.SaveChangesAsync(
            cancellationToken);
    }
}