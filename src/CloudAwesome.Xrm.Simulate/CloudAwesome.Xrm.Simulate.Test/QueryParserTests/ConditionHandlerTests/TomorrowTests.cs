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
public class TomorrowTests
{
    private IOrganizationService _organizationService = null!;
    private IOrganizationService? orgService;
    
    private readonly Contact _earlyPositiveContact = Arthur.Contact();
    private readonly Contact _earlyNegativeContact = Bruce.Contact();
    private readonly Contact _latePositiveContact = Siobhan.Contact();
    private readonly Contact _lateNegativeContact = Daniel.Contact();

    [SetUp]
    public void SetUp()
    {
        _earlyNegativeContact.overriddencreatedon = new DateTime(2023, 04, 19, 09, 45, 00);
        _earlyPositiveContact.overriddencreatedon = new DateTime(2023, 04, 20, 07, 45, 00);
        _latePositiveContact.overriddencreatedon = new DateTime(2023, 04, 20, 22, 10, 32);
        _lateNegativeContact.overriddencreatedon = new DateTime(2023, 04, 21);
        
        var options = new SimulatorOptions
        {
            ClockSimulator = new MockSystemTime(new DateTime(2023, 04, 19, 14, 00, 00))
        };
        
        orgService = _organizationService.Simulate(options);
    }

    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        orgService!.Data().Add(_earlyPositiveContact);
        orgService!.Data().Add(_latePositiveContact);
        orgService!.Data().Add(_earlyNegativeContact);
        orgService!.Data().Add(_lateNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(2);
    }
    
    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService!.Data().Add(_earlyNegativeContact);
        orgService!.Data().Add(_lateNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        orgService!.Data().Add(_earlyPositiveContact);
        orgService!.Data().Add(_latePositiveContact);
        orgService!.Data().Add(_earlyNegativeContact);
        orgService!.Data().Add(_lateNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(2);
    }
    
    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService!.Data().Add(_earlyNegativeContact);
        orgService!.Data().Add(_lateNegativeContact);

        var contacts = orgService!.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new TomorrowConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.Tomorrow);
    }
    
    private readonly QueryExpression _queryExpression = new()
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.overriddencreatedon, 
                    ConditionOperator.Tomorrow)
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
                      <condition attribute=""overriddencreatedon"" operator=""tomorrow"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}