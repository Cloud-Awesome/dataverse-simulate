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
public class NullTests
{
    private IOrganizationService _organizationService = null!;

    private readonly Contact _negativeContact = Daniel.Contact();
    private readonly Contact _positiveContact = Arthur.Contact();

    [SetUp]
    public void SetUp()
    {
        _positiveContact.lastname = null;
        _organizationService = _organizationService.Simulate();
    }
    
    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_negativeContact);
        _organizationService.Simulated().Data().Add(_positiveContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(1);
    }

    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_negativeContact);

        var contacts = _organizationService.RetrieveMultiple(_queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        _organizationService.Simulated().Data().Add(_negativeContact);
        _organizationService.Simulated().Data().Add(_positiveContact);

        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(1);
    }

    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        _organizationService.Simulated().Data().Add(_negativeContact);
        var contacts = _organizationService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new NullConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.Null);
    }
    
    [Test]
    public void QueryExpression_Returns_Positive_Results_On_OptionSet()
    {
        var query = new QueryExpression()
        {
            EntityName = Contact.EntityLogicalName,
            Criteria = new FilterExpression
            {
                Conditions =
                {
                    new ConditionExpression(Contact.Fields.customertypecode, 
                        ConditionOperator.Null)
                }
            },
            ColumnSet = new ColumnSet(
                Contact.Fields.firstname, 
                Contact.Fields.lastname)
        };

        _negativeContact.customertypecode = Contact_customertypecode.DefaultValue;
    
        _organizationService.Simulated().Data().Add(_negativeContact);
        _organizationService.Simulated().Data().Add(_positiveContact);

        var contacts = _organizationService.RetrieveMultiple(query);

        contacts.Entities.Count().Should().Be(1);
    }

    private readonly QueryExpression _queryExpression = new()
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.lastname, 
                    ConditionOperator.Null)
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
                      <condition attribute=""lastname"" operator=""null"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}