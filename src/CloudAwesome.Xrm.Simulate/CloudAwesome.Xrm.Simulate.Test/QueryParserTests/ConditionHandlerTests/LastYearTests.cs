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
public class LastYearTests
{
    private IOrganizationService _organizationService = null!;
    
    private readonly Contact _positiveContact = Arthur.Contact();
    private readonly Contact _tooOldNegativeContact = Bruce.Contact();
    private readonly Contact _tooNewNegativeContact = Daniel.Contact();

    [SetUp]
    public void SetUp()
    {
        _positiveContact.OverriddenCreatedOn = new DateTime(2022, 04, 06);
        _tooOldNegativeContact.OverriddenCreatedOn = new DateTime(2021, 03, 27);
        _tooNewNegativeContact.OverriddenCreatedOn = new DateTime(2023, 01, 01);
        
        var options = new SimulatorOptions
        {
            ClockSimulator = new MockSystemTime(new DateTime(2023, 04, 24))
        };
        
        _organizationService = _organizationService.Simulate(options);
    }

    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_positiveContact);
        _organizationService.Simulated().Data().Add(_tooOldNegativeContact);
        _organizationService.Simulated().Data().Add(_tooNewNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(1);
    }
    
    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_tooOldNegativeContact);
        _organizationService.Simulated().Data().Add(_tooNewNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_positiveContact);
        _organizationService.Simulated().Data().Add(_tooOldNegativeContact);
        _organizationService.Simulated().Data().Add(_tooNewNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(1);
    }
    
    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_tooOldNegativeContact);
        _organizationService.Simulated().Data().Add(_tooNewNegativeContact);

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new LastYearConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.LastYear);
    }
    
    private readonly QueryExpression _queryExpression = new()
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.OverriddenCreatedOn, 
                    ConditionOperator.LastYear)
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
                      <condition attribute=""overriddencreatedon"" operator=""last-year"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}