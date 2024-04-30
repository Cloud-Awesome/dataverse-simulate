﻿using System.Linq;
using CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.QueryParserTests.ConditionHandlerTests;

[TestFixture]
public class NotEqualTests
{
    private IOrganizationService _organizationService = null!;

    [SetUp]
    public void SetUp()
    {
        _organizationService = _organizationService.Simulate();
    }
    
    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(Arthur.Contact());
        _organizationService.Simulated().Data().Add(Siobhan.Contact());
        _organizationService.Simulated().Data().Add(Bruce.Contact());

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(2);
    }

    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(Siobhan.Contact());

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(Arthur.Contact());
        _organizationService.Simulated().Data().Add(Siobhan.Contact());
        _organizationService.Simulated().Data().Add(Bruce.Contact());

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(2);
    }

    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(Siobhan.Contact());

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new NotEqualConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.NotEqual);
    }

    private readonly QueryExpression _queryExpression = new()
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.lastname, 
                    ConditionOperator.NotEqual, "Nicholson")
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
                      <condition attribute=""lastname"" operator=""ne"" value=""Nicholson"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}