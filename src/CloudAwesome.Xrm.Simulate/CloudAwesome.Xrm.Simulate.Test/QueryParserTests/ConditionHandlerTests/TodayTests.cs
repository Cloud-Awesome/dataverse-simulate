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
public class TodayTests
{
    private IOrganizationService _organizationService = null!;
    
    private readonly Contact _earlyPositiveContact = Arthur.Contact();
    private readonly Contact _earlyNegativeContact = Bruce.Contact();
    private readonly Contact _latePositiveContact = Siobhan.Contact();
    private readonly Contact _lateNegativeContact = Daniel.Contact();

    [SetUp]
    public void SetUp()
    {
        _earlyNegativeContact.OverriddenCreatedOn = new DateTime(2023, 04, 18, 09, 45, 00);
        _earlyPositiveContact.OverriddenCreatedOn = new DateTime(2023, 04, 19, 07, 45, 00);
        _latePositiveContact.OverriddenCreatedOn = new DateTime(2023, 04, 19, 22, 10, 32);
        _lateNegativeContact.OverriddenCreatedOn = new DateTime(2023, 04, 20);
        
        var options = new SimulatorOptions
        {
            ClockSimulator = new MockSystemTime(new DateTime(2023, 04, 19, 14, 00, 00))
        };
        
        _organizationService = _organizationService.Simulate(options);
    }

    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_earlyPositiveContact);
        _organizationService.Simulated().Data().Add(_latePositiveContact);
        _organizationService.Simulated().Data().Add(_earlyNegativeContact);
        _organizationService.Simulated().Data().Add(_lateNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(2);
    }
    
    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_earlyNegativeContact);
        _organizationService.Simulated().Data().Add(_lateNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_earlyPositiveContact);
        _organizationService.Simulated().Data().Add(_latePositiveContact);
        _organizationService.Simulated().Data().Add(_earlyNegativeContact);
        _organizationService.Simulated().Data().Add(_lateNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(2);
    }
    
    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_earlyNegativeContact);
        _organizationService.Simulated().Data().Add(_lateNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new TodayConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.Today);
    }
    
    private readonly QueryExpression _queryExpression = new()
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.OverriddenCreatedOn, 
                    ConditionOperator.Today)
            }
        },
        ColumnSet = new ColumnSet(
            Contact.Fields.FirstName, 
            Contact.Fields.LastName)
    };
    
    private readonly FetchExpression _fetchQuery = new()
    { 
        Query = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                  <entity name=""contact"">
                    <attribute name=""firstname"" />
                    <attribute name=""lastname"" />
                    <order attribute=""fullname"" descending=""false"" />
                    <filter type=""and"">
                      <condition attribute=""overriddencreatedon"" operator=""today"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}