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
        e.creditonhold = true;

        // Generate some random employee id on create
        Random random = new Random();
        e.employeeid = random.Next(11111, 99999).ToString();

        // Default Surname to uppercase
        if (e.lastname is not null) e.lastname = e.lastname.ToUpper();

        return e;
    }
}