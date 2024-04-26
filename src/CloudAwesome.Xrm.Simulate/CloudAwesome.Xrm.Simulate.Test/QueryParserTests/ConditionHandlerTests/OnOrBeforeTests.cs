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
public class OnOrBeforeTests
{
    private IOrganizationService _organizationService = null!;
    private IOrganizationService? orgService;
    
    private readonly Contact _earlyPositiveContact = Bruce.Contact();
    private readonly Contact _positiveContactWithTime = Arthur.Contact();
    private readonly Contact _negativeContactWithTime = Siobhan.Contact();
    private readonly Contact _lateNegativeContact = Daniel.Contact();

    [SetUp]
    public void SetUp()
    {
        _earlyPositiveContact.overriddencreatedon = new DateTime(2023, 04, 18);
        _positiveContactWithTime.overriddencreatedon = new DateTime(2023, 04, 19, 09, 00, 00);
        _negativeContactWithTime.overriddencreatedon = new DateTime(2023, 04, 20, 14, 00, 00);
        _lateNegativeContact.overriddencreatedon = new DateTime(2023, 04, 30, 18, 00, 00);
        
        var options = new SimulatorOptions
        {
            ClockSimulator = new MockSystemTime(new DateTime(2023, 04, 19, 12, 00, 00))
        };
        
        orgService = _organizationService.Simulate(options);
    }

    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        orgService!.Data().Add(_earlyPositiveContact);
        orgService!.Data().Add(_positiveContactWithTime);
        orgService!.Data().Add(_negativeContactWithTime);
        orgService!.Data().Add(_lateNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(2);
    }
    
    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService!.Data().Add(_negativeContactWithTime);
        orgService!.Data().Add(_lateNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        orgService!.Data().Add(_positiveContactWithTime);
        orgService!.Data().Add(_earlyPositiveContact);
        orgService!.Data().Add(_negativeContactWithTime);
        orgService!.Data().Add(_lateNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(2);
    }
    
    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService!.Data().Add(_negativeContactWithTime);
        orgService!.Data().Add(_lateNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new OnOrBeforeConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.OnOrBefore);
    }
    
    private readonly QueryExpression _queryExpression = new QueryExpression
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.overriddencreatedon, 
                    ConditionOperator.OnOrBefore, new DateTime(2023, 04, 19))
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
                      <condition attribute=""overriddencreatedon"" operator=""on-or-before"" value=""2023-04-19"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}