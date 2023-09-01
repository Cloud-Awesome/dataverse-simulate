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
    private readonly IOrganizationService _organizationService = null!;
    
    [Test]
    public void Query_By_Attribute_Returns_Valid_Single_Result()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        orgService.Data().Add(Arthur.Contact());

        var query = new QueryByAttribute(Contact.EntityLogicalName)
        {
            Attributes = { Contact.Fields.lastname },
            Values = { Arthur.Contact().lastname }
        };
        
        var contacts = orgService.RetrieveMultiple(query);

        contacts.Entities.Count.Should().Be(1);
        contacts.Entities.FirstOrDefault()?.Attributes["firstname"].Should().Be("Arthur");
    }

    [Test]
    public void Query_By_Attribute_Returns_Valid_Results()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        orgService.Data().Add(Siobhan.Contact());
        orgService.Data().Add(Daniel.Contact());

        var query = new QueryByAttribute(Contact.EntityLogicalName)
        {
            Attributes = { Contact.Fields.lastname },
            Values = { Siobhan.Contact().lastname }
        };

        var contacts = orgService.RetrieveMultiple(query);
        
        contacts.Entities.Count.Should().Be(2);
    }
    
    [Test]
    public void Query_By_Attribute_With_Multiple_Conditions_Returns_Valid_Results()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        orgService.Data().Add(Siobhan.Contact());
        orgService.Data().Add(Daniel.Contact());

        var query = new QueryByAttribute(Contact.EntityLogicalName)
        {
            Attributes = { Contact.Fields.lastname, Contact.Fields.firstname },
            Values = { Siobhan.Contact().lastname, Siobhan.Contact().firstname }
        };

        var contacts = orgService.RetrieveMultiple(query);
        
        contacts.Entities.Count.Should().Be(1);
    }
    
}