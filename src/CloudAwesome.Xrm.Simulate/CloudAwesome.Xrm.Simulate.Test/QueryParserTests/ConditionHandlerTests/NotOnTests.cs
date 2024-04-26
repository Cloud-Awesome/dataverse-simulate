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
public class NotOnTests
{
    private IOrganizationService _organizationService = null!;
    private IOrganizationService? orgService;
    
    private readonly Contact _negativeContact = Arthur.Contact();
    private readonly Contact _earlyPositiveContact = Bruce.Contact();
    private readonly Contact _latePositiveContact = Daniel.Contact();

    [SetUp]
    public void SetUp()
    {
        _earlyPositiveContact.overriddencreatedon = new DateTime(2023, 04, 18);
        _negativeContact.overriddencreatedon = new DateTime(2023, 04, 19);
        _latePositiveContact.overriddencreatedon = new DateTime(2023, 04, 20);
        
        var options = new SimulatorOptions
        {
            ClockSimulator = new MockSystemTime(new DateTime(2023, 04, 19))
        };
        
        orgService = _organizationService.Simulate(options);
    }

    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        orgService!.Data().Add(_negativeContact);
        orgService!.Data().Add(_earlyPositiveContact);
        orgService!.Data().Add(_latePositiveContact);

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(2);
    }
    
    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService!.Data().Add(_negativeContact);

        var contacts = orgService!.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new NotOnConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.NotOn);
    }
    
    private readonly QueryExpression _queryExpression = new QueryExpression
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.overriddencreatedon, 
                    ConditionOperator.NotOn, new DateTime(2023, 04, 19))
            }
        },
        ColumnSet = new ColumnSet(
            Contact.Fields.firstname, 
            Contact.Fields.lastname)
    };
}