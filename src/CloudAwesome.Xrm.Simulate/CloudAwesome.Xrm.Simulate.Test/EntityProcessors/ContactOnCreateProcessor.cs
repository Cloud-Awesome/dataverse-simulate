using System;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Test.EntityProcessors;

public class ContactOnCreateProcessor: IEntityProcessor
{
    public Entity Process(Entity entity)
    {
        var e = (Contact)entity;

        // Default all new contacts to Credit On Hold
        e.CreditOnHold = true;

        // Generate some random employee id on create
        Random random = new Random();
        e.EmployeeId = random.Next(11111, 99999).ToString();

        // Default Surname to uppercase
        if (e.LastName is not null) e.LastName = e.LastName.ToUpper();

        return e;
    }
}