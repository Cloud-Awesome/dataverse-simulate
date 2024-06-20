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
    
    private Guid _contactId = Guid.NewGuid();
    private Entity _contactRecord = Arthur.Contact();

    [SetUp]
    public void SetUp()
    {
        _contactRecord.Id = _contactId;
        
        _organizationService = _organizationService.Simulate();
        _organizationService.Simulated().Data().Add(_contactRecord);
    }
    
    [Test]
    public void Retrieve_Existing_Record_Returns_Valid_Entity()
    {
        var retrievedContact = 
            _organizationService.Retrieve(Contact.EntityLogicalName, _contactId, new ColumnSet(true));

        retrievedContact.Id.Should().Be(_contactId);
    }

    [Test]
    public void Retrieve_Existing_Record_Returns_All_Columns()
    {
        var retrievedContact = (Contact)
            _organizationService.Retrieve("contact", _contactId, new ColumnSet(true));

        retrievedContact.FirstName.Should().Be(Arthur.Contact().FirstName);
        retrievedContact.LastName.Should().Be(Arthur.Contact().LastName);
        retrievedContact.BirthDate.Should().Be(Arthur.Contact().BirthDate);
    }

    [Test]
    public void Retrieve_Existing_Record_Returns_Defined_ColumnSet()
    {
        var retrievedContact =
            _organizationService.Retrieve("contact", _contactId, 
                new ColumnSet(Contact.Fields.FirstName));

        retrievedContact.Attributes["firstname"].Should().Be(Arthur.Contact().FirstName);

        var missingAttribute = () => retrievedContact.Attributes["lastname"];
        missingAttribute.Should().Throw<KeyNotFoundException>();
    }

    [Test]
    public void Retrieve_Existing_Record_Always_Returns_Primary_Guid()
    {
        var retrievedContact =
            _organizationService.Retrieve("contact", _contactId, 
                new ColumnSet(Contact.Fields.FirstName));

        retrievedContact.Attributes["firstname"].Should().Be(Arthur.Contact().FirstName);

        var missingAttribute = () => retrievedContact.Attributes["contactid"];
        missingAttribute.Should().NotThrow<KeyNotFoundException>();
        
        retrievedContact.Attributes["contactid"].Should().Be(_contactId);
    }
}