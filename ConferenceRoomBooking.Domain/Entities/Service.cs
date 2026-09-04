using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Domain.Entities;

public class Service
{


    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }

    private Service()
    {
    }

    public Service(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        IsActive = true;
    }

    public void Update(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}