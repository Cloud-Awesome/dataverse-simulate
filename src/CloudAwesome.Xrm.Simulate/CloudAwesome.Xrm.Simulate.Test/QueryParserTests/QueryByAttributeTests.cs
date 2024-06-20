using System.Linq;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.QueryParserTests;

[TestFixture]
public class QueryByAttributeTests
{
    private IOrganizationService _organizationService = null!;

    [SetUp]
    public void SetUp()
    {
        _organizationService = _organizationService.Simulate();
    }
    
    [Test]
    public void Query_By_Attribute_Returns_Valid_Single_Result()
    {
        _organizationService.Simulated().Data().Add(Arthur.Contact());

        var query = new QueryByAttribute(Contact.EntityLogicalName)
        {
            Attributes = { Contact.Fields.LastName },
            Values = { Arthur.Contact().LastName }
        };
        
        var contacts = _organizationService.RetrieveMultiple(query);

        contacts.Entities.Count.Should().Be(1);
        contacts.Entities.FirstOrDefault()?.Attributes["firstname"].Should().Be("Arthur");
    }

    [Test]
    public void Query_By_Attribute_Returns_Valid_Results()
    {
        _organizationService.Simulated().Data().Add(Siobhan.Contact());
        _organizationService.Simulated().Data().Add(Daniel.Contact());

        var query = new QueryByAttribute(Contact.EntityLogicalName)
        {
            Attributes = { Contact.Fields.LastName },
            Values = { Siobhan.Contact().LastName }
        };

        var contacts = _organizationService.RetrieveMultiple(query);
        
        contacts.Entities.Count.Should().Be(2);
    }
    
    [Test]
    public void Query_By_Attribute_With_Multiple_Conditions_Returns_Valid_Results()
    {
        _organizationService.Simulated().Data().Add(Siobhan.Contact());
        _organizationService.Simulated().Data().Add(Daniel.Contact());

        var query = new QueryByAttribute(Contact.EntityLogicalName)
        {
            Attributes = { Contact.Fields.LastName, Contact.Fields.FirstName },
            Values = { Siobhan.Contact().LastName, Siobhan.Contact().FirstName }
        };

        var contacts = _organizationService.RetrieveMultiple(query);
        
        contacts.Entities.Count.Should().Be(1);
    }

    [Test]
    public void Query_By_Attribute_With_TopCount_Returns_Correct_Results()
    {
        _organizationService.Simulated().Data().Add(Arthur.Contact());
        _organizationService.Simulated().Data().Add(Bruce.Contact());
        _organizationService.Simulated().Data().Add(Daniel.Contact());
        _organizationService.Simulated().Data().Add(Siobhan.Contact());

        var query = new QueryByAttribute(Contact.EntityLogicalName)
        {
            TopCount = 2,
            Orders =
            {
                new OrderExpression(Contact.Fields.FirstName, OrderType.Descending)
            }
        };

        var contacts = _organizationService.RetrieveMultiple(query).Entities.Cast<Contact>().ToList();

        contacts.Count.Should().Be(2);
        contacts[0].FirstName.Should().Be(Siobhan.Contact().FirstName);
        contacts[1].FirstName.Should().Be(Daniel.Contact().FirstName);
    }
}