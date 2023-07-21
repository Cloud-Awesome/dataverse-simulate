using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.BasicServiceRequestTests;

[TestFixture]
public class RetrieveTests
{
    private IOrganizationService _organizationService = null!;
    private IOrganizationService orgService;
    
    private Guid _contactId = Guid.NewGuid();
    private Entity _contactRecord = Arthur.Contact();

    [SetUp]
    public void SetUp()
    {
        _contactRecord.Id = _contactId;
        
        orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        orgService.Data().Add(_contactRecord);
    }
    
    [Test]
    public void Retrieve_Existing_Record_Returns_Valid_Entity()
    {
        var retrievedContact = 
            orgService.Retrieve(Contact.EntityLogicalName, _contactId, new ColumnSet(true));

        retrievedContact.Id.Should().Be(_contactId);
    }

    [Test]
    public void Retrieve_Existing_Record_Returns_All_Columns()
    {
        var retrievedContact = (Contact)
            orgService.Retrieve("contact", _contactId, new ColumnSet(true));

        retrievedContact.firstname.Should().Be(Arthur.Contact().firstname);
        retrievedContact.lastname.Should().Be(Arthur.Contact().lastname);
        retrievedContact.birthdate.Should().Be(Arthur.Contact().birthdate);
    }

    [Test]
    public void Retrieve_Existing_Record_Returns_Defined_ColumnSet()
    {
        var retrievedContact =
            orgService.Retrieve("contact", _contactId, 
                new ColumnSet(Contact.Fields.firstname));

        retrievedContact.Attributes["firstname"].Should().Be(Arthur.Contact().firstname);

        var missingAttribute = () => retrievedContact.Attributes["lastname"];
        missingAttribute.Should().Throw<KeyNotFoundException>();
    }

    [Test]
    public void Retrieve_Existing_Record_Always_Returns_Primary_Guid()
    {
        var retrievedContact =
            orgService.Retrieve("contact", _contactId, 
                new ColumnSet(Contact.Fields.firstname));

        retrievedContact.Attributes["firstname"].Should().Be(Arthur.Contact().firstname);

        var missingAttribute = () => retrievedContact.Attributes["contactid"];
        missingAttribute.Should().NotThrow<KeyNotFoundException>();
        
        retrievedContact.Attributes["contactid"].Should().Be(_contactId);
    }
}