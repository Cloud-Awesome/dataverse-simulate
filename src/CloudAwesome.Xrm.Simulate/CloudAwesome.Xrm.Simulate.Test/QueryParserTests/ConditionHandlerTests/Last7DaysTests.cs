﻿using System;
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
public class Last7DaysTests
{
    private IOrganizationService _organizationService = null!;
    private IOrganizationService? orgService;

    [SetUp]
    public void Last7DaysTestsSetUp()
    {
        var options = new SimulatorOptions
        {
            ClockSimulator = new MockSystemTime(new DateTime(2023, 04, 19))
        };
        
        orgService = _organizationService.Simulate(options);
        orgService.Data().Reinitialise();
    }

    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        orgService!.Data().Add(Arthur.Contact());
        orgService!.Data().Add(Bruce.Contact());

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(1);
    }
    
    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService!.Data().Add(Bruce.Contact());

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        orgService!.Data().Add(Arthur.Contact());
        orgService!.Data().Add(Bruce.Contact());

        var contacts = orgService!.RetrieveMultiple(fetchQuery);

        contacts.Entities.Count().Should().Be(1);
    }
    
    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService.Data().Add(Bruce.Contact());

        var contacts = orgService.RetrieveMultiple(fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new Last7DaysConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.Last7Days);
    }
    
    private readonly QueryExpression _queryExpression = new QueryExpression
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.overriddencreatedon, 
                    ConditionOperator.Last7Days)
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
                      <condition attribute=""overriddencreatedon"" operator=""last-seven-days"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}