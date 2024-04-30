using System;
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
public class NextXHoursTests
{
    private IOrganizationService _organizationService = null!;
    
    private readonly Contact _positiveContact = Arthur.Contact();
    private readonly Contact _oldNegativeContact = Bruce.Contact();
    private readonly Contact _futureNegativeContact = Daniel.Contact();

    [SetUp]
    public void SetUp()
    {
        _positiveContact.overriddencreatedon = new DateTime(2023, 04, 12, 14, 00, 00);
        _oldNegativeContact.overriddencreatedon = new DateTime(2023, 04, 12, 12, 45, 00);
        _futureNegativeContact.overriddencreatedon = new DateTime(2023, 04, 12, 16, 00, 00);
        
        var options = new SimulatorOptions
        {
            ClockSimulator = new MockSystemTime(new DateTime(2023, 04, 12, 13, 00, 00))
        };
        
        _organizationService = _organizationService.Simulate(options);
    }

    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_positiveContact);
        _organizationService.Simulated().Data().Add(_oldNegativeContact);
        _organizationService.Simulated().Data().Add(_futureNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(1);
    }
    
    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_oldNegativeContact);
        _organizationService.Simulated().Data().Add(_futureNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_positiveContact);
        _organizationService.Simulated().Data().Add(_oldNegativeContact);
        _organizationService.Simulated().Data().Add(_futureNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(1);
    }
    
    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_oldNegativeContact);
        _organizationService.Simulated().Data().Add(_futureNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new NextXHoursConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.NextXHours);
    }
    
    private readonly QueryExpression _queryExpression = new()
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.overriddencreatedon, 
                    ConditionOperator.NextXHours, 2)
            }
        },
        ColumnSet = new ColumnSet(
            Contact.Fields.firstname, 
            Contact.Fields.lastname)
    };
    
    private readonly FetchExpression _fetchQuery = new()
    { 
        Query = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                  <entity name=""contact"">
                    <attribute name=""firstname"" />
                    <attribute name=""lastname"" />
                    <order attribute=""fullname"" descending=""false"" />
                    <filter type=""and"">
                      <condition attribute=""overriddencreatedon"" operator=""next-x-hours"" value=""2"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}