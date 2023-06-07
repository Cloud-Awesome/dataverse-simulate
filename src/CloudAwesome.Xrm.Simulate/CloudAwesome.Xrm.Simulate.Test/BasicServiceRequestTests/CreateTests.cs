using System;
using System.Linq;
using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.BasicServiceRequestTests;

[TestFixture]
public class CreateTests
{
    private readonly IOrganizationService _organizationService = null!;
    
    [Test]
    public void Create_Contact_Saves_Record_To_Data_Store()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        var contactId = orgService.Create(Arthur.Contact());
        contactId.Should().NotBeEmpty();

        var contacts = orgService.Data().Get(Arthur.Contact().LogicalName);
        contacts.Count.Should().Be(1);

        contacts.FirstOrDefault()?.Id.Should().Be(contactId);
    }

    [Test]
    public void Calling_Create_Method_Via_Execute_Method_Should_Save_To_Data_Store()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();

        var createRequest = new CreateRequest
        {
            Target = Arthur.Contact()
        };

        var createResponse = (CreateResponse) orgService.Execute(createRequest);
        createResponse.id.Should().NotBeEmpty();
        createResponse.ResponseName.Should().Be("Create");
        
        var contacts = orgService.Data().Get(Arthur.Contact().LogicalName);
        contacts.Count.Should().Be(1);

        contacts.FirstOrDefault()?.Id.Should().Be(createResponse.id);
    }

    [Test]
    public void Create_Contact_Sets_System_DateTime_Metadata()
    {
        var mockSystemTime = new MockSystemTime(new DateTime(2020, 8, 16));
        var options = new SimulatorOptions
        {
            ClockSimulator = mockSystemTime
        };
        
        var orgService = _organizationService.Simulate(options);
        orgService.Data().Reinitialise();

        var contactId = orgService.Create(Arthur.Contact());
        var contact = orgService.Data().Get(Arthur.Contact().LogicalName).FirstOrDefault();

        contact.Attributes["createdon"].Should().Be(mockSystemTime.Now);
        contact.Attributes["modifiedon"].Should().Be(mockSystemTime.Now);
    }

    [Test]
    public void Create_Contact_Sets_System_User_Metadata()
    {
        var options = new SimulatorOptions
        {
            AuthenticatedUser = new Entity("systemuser", Guid.NewGuid())
            {
                Attributes =
                {
                    ["fullname"] = "Lynda Archer"
                }
            }
        };
        var authenticatedUser = options.AuthenticatedUser.ToEntityReference();
        
        var orgService = _organizationService.Simulate(options);
        orgService.Data().Reinitialise();
        
        var contactId = orgService.Create(Arthur.Contact());
        var contact = orgService.Data().Get(Arthur.Contact().LogicalName).FirstOrDefault();

        contact.Attributes["createdby"].Should().Be(authenticatedUser);
        contact.Attributes["modifiedby"].Should().Be(authenticatedUser);
        contact.Attributes["ownerid"].Should().Be(authenticatedUser);

        contact.Attributes["createdby"].Should().BeOfType<EntityReference>();
        var retrievedContact = (EntityReference) contact.Attributes["createdby"];
        retrievedContact.Id.Should().Be(authenticatedUser.Id);
    }
}