﻿using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.DataServicesTests;

[TestFixture]
public class EntityDataServiceTests
{
    private IOrganizationService _organizationService = null!;

    [SetUp]
    public void SetUp()
    {
        _organizationService = _organizationService.Simulate();
    }
    
    [Test]
    public void Initialise_Data_Store_Should_Correctly_Save_Entities()
    {
        _organizationService.Simulated().Data().Add(Arthur.Contact());
        var contacts = _organizationService.Simulated().Data().Get(Arthur.Contact().LogicalName);
        contacts.Count.Should().Be(1);
    }
    
    [Test]
    public void Initialise_Data_Store_With_No_Accounts_Should_Return_Empty_List()
    {
        _organizationService.Simulated().Data().Add(Arthur.Contact());
        var contacts = _organizationService.Simulated().Data().Get(Arthur.Account().LogicalName);
        contacts.Count.Should().Be(0);
    }

    [Test]
    public void Clearing_Data_Should_Reinitialise_The_Data_Store()
    {
        _organizationService.Simulated().Data().Add(Arthur.Contact());
        _organizationService.Simulated().Data().Reinitialise();

        var contacts = _organizationService.Simulated().Data().Get(Arthur.Contact().LogicalName);
        contacts.Count.Should().Be(0);
    }

    [Test]
    public void Initialise_Multiple_Entities_Should_Correctly_Save()
    {
        _organizationService.Simulated().Data().Add(Arthur.Contact());
        _organizationService.Simulated().Data().Add(Arthur.Account());
        _organizationService.Simulated().Data().Add(Siobhan.Contact());

        var contacts = _organizationService.Simulated().Data().Get(Arthur.Contact().LogicalName);
        var accounts = _organizationService.Simulated().Data().Get(Arthur.Account().LogicalName);
        var leads = _organizationService.Simulated().Data().Get("lead");

        contacts.Count.Should().Be(2);
        accounts.Count.Should().Be(1);
        leads.Count.Should().Be(0);
    }
}