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
public class OnOrAfterTests
{
    private IOrganizationService _organizationService = null!;
    
    private readonly Contact _earlyNegativeContact = Bruce.Contact();
    private readonly Contact _positiveContact = Arthur.Contact();
    private readonly Contact _positiveContactWithTime = Siobhan.Contact();
    private readonly Contact _latePositiveContact = Daniel.Contact();

    [SetUp]
    public void SetUp()
    {
        _earlyNegativeContact.OverriddenCreatedOn = new DateTime(2023, 04, 18);
        _positiveContact.OverriddenCreatedOn = new DateTime(2023, 04, 19);
        _positiveContactWithTime.OverriddenCreatedOn = new DateTime(2023, 04, 19, 14, 00, 00);
        _latePositiveContact.OverriddenCreatedOn = new DateTime(2023, 04, 30, 18, 00, 00);
        
        var options = new SimulatorOptions
        {
            ClockSimulator = new MockSystemTime(new DateTime(2023, 04, 19))
        };
        
        _organizationService = _organizationService.Simulate(options);
    }

    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_earlyNegativeContact);
        _organizationService.Simulated().Data().Add(_positiveContact);
        _organizationService.Simulated().Data().Add(_positiveContactWithTime);
        _organizationService.Simulated().Data().Add(_latePositiveContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(3);
    }
    
    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_earlyNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_positiveContact);
        _organizationService.Simulated().Data().Add(_earlyNegativeContact);
        _organizationService.Simulated().Data().Add(_positiveContactWithTime);
        _organizationService.Simulated().Data().Add(_latePositiveContact);

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(3);
    }
    
    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_earlyNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new OnOrAfterConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.OnOrAfter);
    }
    
    private readonly QueryExpression _queryExpression = new()
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.OverriddenCreatedOn, 
                    ConditionOperator.OnOrAfter, new DateTime(2023, 04, 19))
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
                      <condition attribute=""overriddencreatedon"" operator=""on-or-after"" value=""2023-04-19"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}