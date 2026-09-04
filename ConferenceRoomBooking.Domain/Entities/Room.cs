using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Domain.Entities;

// Абстрактна кімната із задачі. Я навіть не знаю, що ще додати...
public class Room
{

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public int Capacity { get; private set; }
    public decimal BaseHourlyPrice { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private Room()
    {
    }
    public Room(
        string name,
        int capacity,
        decimal baseHourlyPrice)
    {
        Id = Guid.NewGuid();
        Name = name;
        Capacity = capacity;
        BaseHourlyPrice = baseHourlyPrice;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string name,
        int capacity,
        decimal baseHourlyPrice)
    {
        Name = name;
        Capacity = capacity;
        BaseHourlyPrice = baseHourlyPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}