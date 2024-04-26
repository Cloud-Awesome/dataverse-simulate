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
public class NotNullTests
{
    private IOrganizationService _organizationService = null!;
    private IOrganizationService? orgService;

    private readonly Contact _positiveContact = Daniel.Contact();
    private readonly Contact _negativeContact = Arthur.Contact();

    [SetUp]
    public void SetUp()
    {
        _negativeContact.lastname = null;
        orgService = _organizationService.Simulate();
    }
    
    [Test]
    public void QueryExpression_Returns_Positive_Results()
    {
        orgService.Data().Add(_positiveContact);
        orgService.Data().Add(_negativeContact);

        var contacts = orgService.RetrieveMultiple(queryExpression);

        contacts.Entities.Count().Should().Be(1);
    }

    [Test]
    public void QueryExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService.Data().Add(_negativeContact);

        var contacts = orgService.RetrieveMultiple(queryExpression);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void FetchExpression_Returns_Positive_Results()
    {
        orgService.Data().Add(_positiveContact);
        orgService.Data().Add(_negativeContact);

        var contacts = orgService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(1);
    }

    [Test]
    public void FetchExpression_Returns_Empty_Set_If_None_Found()
    {
        orgService.Data().Add(_negativeContact);
        var contacts = orgService.RetrieveMultiple(_fetchQuery);

        contacts.Entities.Count().Should().Be(0);
    }
    
    [Test]
    public void Correct_ConditionOperator_Is_Set()
    {
        var handler = new NotNullConditionHandler();
        handler.Operator.Should().Be(ConditionOperator.NotNull);
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
                        ConditionOperator.NotNull)
                }
            },
            ColumnSet = new ColumnSet(
                Contact.Fields.firstname, 
                Contact.Fields.lastname)
        };

        _positiveContact.customertypecode = Contact_customertypecode.DefaultValue;
    
        orgService.Data().Add(_negativeContact);
        orgService.Data().Add(Bruce.Contact());
        orgService.Data().Add(_positiveContact);

        var contacts = orgService.RetrieveMultiple(query);

        contacts.Entities.Count().Should().Be(1);
    }

    private QueryExpression queryExpression = new QueryExpression
    {
        EntityName = Contact.EntityLogicalName,
        Criteria = new FilterExpression
        {
            Conditions =
            {
                new ConditionExpression(Contact.Fields.lastname, 
                    ConditionOperator.NotNull)
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
                      <condition attribute=""lastname"" operator=""not-null"" />
                    </filter>
                  </entity>
                </fetch>" 
    };
}