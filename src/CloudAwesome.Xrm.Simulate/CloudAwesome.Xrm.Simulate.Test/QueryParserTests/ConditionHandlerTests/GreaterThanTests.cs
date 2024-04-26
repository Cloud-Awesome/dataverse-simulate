using System.Linq;
using CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.QueryParserTests.ConditionHandlerTests;

[TestFixture]
public class GreaterThanTests
{
    private IOrganizationService _organizationService = null!;
    private IOrganizationService? orgService;

    [SetUp]
    public void BeginsWithSetUp()
    {
        orgService = _organizationService.Simulate();
    }
    
    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        orgService.Data().Add(Arthur.Contact()); // 0
        orgService.Data().Add(Siobhan.Contact()); // 1
        orgService.Data().Add(Bruce.Contact()); // 2

        var contacts = orgService.RetrieveMultiple(queryExpression);

        contacts.Entities.Count().Should().Be(1);
    }

    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService.Data().Add(Arthur.Contact()); // 0
        orgService.Data().Add(Siobhan.Contact()); // 1

        var contacts = orgService.RetrieveMultiple(queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        orgService.Data().Add(Arthur.Contact()); // 0
        orgService.Data().Add(Siobhan.Contact()); // 1
        orgService.Data().Add(Bruce.Contact()); // 2

        var contacts = orgService.RetrieveMultiple(fetchQuery);

        contacts.Entities.Count().Should().Be(1);
    }

    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService.Data().Add(Arthur.Contact()); // 0
        orgService.Data().Add(Siobhan.Contact()); // 1

        var contacts = orgService.RetrieveMultiple(fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new GreaterThanConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.GreaterThan);
    }

    private QueryExpression queryExpression = new QueryExpression
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.numberofchildren, 
                    ConditionOperator.GreaterThan, 1)
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
                      <condition attribute=""numberofchildren"" operator=""gt"" value=""1"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}