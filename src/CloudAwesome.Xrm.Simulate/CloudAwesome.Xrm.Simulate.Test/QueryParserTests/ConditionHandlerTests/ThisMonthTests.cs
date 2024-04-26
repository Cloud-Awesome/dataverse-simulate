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
public class ThisMonthTests
{
    private IOrganizationService _organizationService = null!;
    private IOrganizationService? orgService;
    
    private readonly Contact _earlyPositiveContact = Arthur.Contact();
    private readonly Contact _latePositiveContact = Siobhan.Contact();
    private readonly Contact _oldNegativeContact = Bruce.Contact();
    private readonly Contact _futureNegativeContact = Daniel.Contact();

    [SetUp]
    public void SetUp()
    {
        _earlyPositiveContact.overriddencreatedon = new DateTime(2023, 06, 02);
        _latePositiveContact.overriddencreatedon = new DateTime(2023, 06, 28);
        _oldNegativeContact.overriddencreatedon = new DateTime(2023, 05, 09);
        _futureNegativeContact.overriddencreatedon = new DateTime(2024, 07, 02);
        
        var options = new SimulatorOptions
        {
            ClockSimulator = new MockSystemTime(new DateTime(2023, 06, 12))
        };
        
        orgService = _organizationService.Simulate(options);
    }

    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        orgService!.Data().Add(_earlyPositiveContact);
        orgService!.Data().Add(_latePositiveContact);
        orgService!.Data().Add(_oldNegativeContact);
        orgService!.Data().Add(_futureNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(2);
    }
    
    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService!.Data().Add(_oldNegativeContact);
        orgService!.Data().Add(_futureNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        orgService!.Data().Add(_earlyPositiveContact);
        orgService!.Data().Add(_latePositiveContact);
        orgService!.Data().Add(_oldNegativeContact);
        orgService!.Data().Add(_futureNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(2);
    }
    
    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService!.Data().Add(_oldNegativeContact);
        orgService!.Data().Add(_futureNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new ThisMonthConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.ThisMonth);
    }
    
    private readonly QueryExpression _queryExpression = new QueryExpression
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.overriddencreatedon, 
                    ConditionOperator.ThisMonth)
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
                      <condition attribute=""overriddencreatedon"" operator=""this-month"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}