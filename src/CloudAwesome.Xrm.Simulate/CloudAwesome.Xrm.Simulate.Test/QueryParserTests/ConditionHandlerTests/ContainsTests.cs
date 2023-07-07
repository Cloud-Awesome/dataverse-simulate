using System.Linq;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.QueryParserTests.ConditionHandlerTests;

[TestFixture()]
public class ContainsTests
{
    private IOrganizationService _organizationService = null!;
    private IOrganizationService? orgService;

    [SetUp]
    public void ContainsTestsSetUp()
    {
        orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
    }
    
    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        orgService.Data().Add(Arthur.Contact());
        orgService.Data().Add(Siobhan.Contact());
        orgService.Data().Add(Bruce.Contact());

        var contacts = orgService.RetrieveMultiple(queryExpression);

        contacts.Entities.Count().Should().Be(2);
    }

    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService.Data().Add(Bruce.Contact());

        var contacts = orgService.RetrieveMultiple(queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        orgService.Data().Add(Arthur.Contact());
        orgService.Data().Add(Siobhan.Contact());
        orgService.Data().Add(Bruce.Contact());

        var contacts = orgService.RetrieveMultiple(fetchQuery);

        contacts.Entities.Count().Should().Be(2);
    }

    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService.Data().Add(Bruce.Contact());

        var contacts = orgService.RetrieveMultiple(fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }

    private QueryExpression queryExpression = new QueryExpression
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.lastname, 
                    ConditionOperator.Contains, "ichol")
            }
        },
        ColumnSet = new ColumnSet(
            Contact.Fields.firstname, 
            Contact.Fields.lastname)
    };

    private FetchExpression fetchQuery = new FetchExpression
    { 
        Query = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                  <entity name=""contact"">
                    <attribute name=""firstname"" />
                    <attribute name=""lastname"" />
                    <order attribute=""fullname"" descending=""false"" />
                    <filter type=""and"">
                      <condition attribute=""lastname"" operator=""like"" value=""%ichol%"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}