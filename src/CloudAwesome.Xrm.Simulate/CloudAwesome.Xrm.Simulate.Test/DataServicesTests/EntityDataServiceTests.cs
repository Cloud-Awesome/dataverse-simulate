using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.DataServicesTests;

[TestFixture]
public class EntityDataServiceTests
{
    private readonly IOrganizationService _organizationService = null!;
    
    [Test]
    public void Initialise_Data_Store_Should_Correctly_Save_Entities()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        orgService.Data().Add(Arthur.Contact());

        var contacts = orgService.Data().Get(Arthur.Contact().LogicalName);
        contacts.Count.Should().Be(1);
    }
    
    [Test]
    public void Initialise_Data_Store_With_No_Accounts_Should_Return_Empty_List()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        orgService.Data().Add(Arthur.Contact());

        var contacts = orgService.Data().Get(Arthur.Account().LogicalName);
        contacts.Count.Should().Be(0);
    }

    [Test]
    public void Clearing_Data_Should_Reinitialise_The_Data_Store()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Add(Arthur.Contact());
        orgService.Data().Reinitialise();

        var contacts = orgService.Data().Get(Arthur.Contact().LogicalName);
        contacts.Count.Should().Be(0);
    }

    [Test]
    public void Initialise_Multiple_Entities_Should_Correctly_Save()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        orgService.Data().Add(Arthur.Contact());
        orgService.Data().Add(Arthur.Account());
        orgService.Data().Add(Siobhan.Contact());

        var contacts = orgService.Data().Get(Arthur.Contact().LogicalName);
        var accounts = orgService.Data().Get(Arthur.Account().LogicalName);
        var leads = orgService.Data().Get("lead");

        contacts.Count.Should().Be(2);
        accounts.Count.Should().Be(1);
        leads.Count.Should().Be(0);
    }
}