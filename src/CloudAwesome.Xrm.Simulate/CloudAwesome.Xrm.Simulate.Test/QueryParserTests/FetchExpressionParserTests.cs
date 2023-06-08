using System.Collections.Generic;
using System.Linq;
using CloudAwesome.Xrm.Simulate.QueryParsers;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.QueryParserTests;

[TestFixture(Description = "N.B. Filters and LinkEntity currently only work if you've included the attributes in the ColumnSet")]
public class FetchExpressionParserTests
{
    private readonly IOrganizationService _organizationService = null!;

    [Test]
    public void Basic_FetchXml_String_Is_Correctly_Converted_To_Query_Expression()
    {
        var fetch = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                       <entity name=""contact"">
                        <attribute name=""lastname"" />
                        <attribute name=""firstname"" />
                        <order attribute=""firstname"" descending=""false"" />
                        <filter type=""and"">
                          <condition attribute=""lastname"" operator=""eq"" value=""Nicholson"" />
                        </filter>
                      </entity>
                    </fetch>";

        var qe = FetchExpressionParser.ConvertFetchXmlToQueryExpression(fetch);

        qe.Should().NotBeNull();
        qe?.EntityName.Should().Be(Contact.EntityLogicalName);
        qe?.ColumnSet.Columns.Count.Should().Be(2);
        qe?.Criteria.Should().NotBeNull();
        qe?.Criteria.Conditions.Count.Should().Be(1);
        qe?.Orders.Count.Should().Be(1);
    }

    [Test]
    public void FetchXml_String_With_LinkEntity_Is_Converted_To_Query_Expressions()
    {
      var fetch = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""true"">
                      <entity name=""contact"">
                        <attribute name=""lastname"" />
                        <attribute name=""firstname"" />
                        <filter type=""and"">
                          <condition attribute=""lastname"" operator=""eq"" value=""Nicholson-Gumula"" />
                        </filter>
                        <link-entity name=""account"" from=""Id"" to=""parentcustomerid"" visible=""false"" link-type=""outer"" alias=""account"">
                          <attribute name=""name"" />
                        </link-entity>                
                      </entity>
                    </fetch>";

      var qe = FetchExpressionParser.ConvertFetchXmlToQueryExpression(fetch);

      qe.Should().NotBeNull();
      qe?.LinkEntities.Count.Should().Be(1);
      qe?.LinkEntities.FirstOrDefault()?.EntityAlias.Should().Be("account");
      qe?.LinkEntities.FirstOrDefault()?.LinkFromEntityName.Should().Be("contact");
      qe?.LinkEntities.FirstOrDefault()?.LinkToEntityName.Should().Be("account");
      qe?.LinkEntities.FirstOrDefault()?.Columns.Columns.Count.Should().Be(1);
    }
    
    [Test]
    public void Basic_Fetch_Returns_Correct_Records()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        orgService.Data().Add(Siobhan.Contact());
        orgService.Data().Add(Daniel.Contact());
        // Should not be returned in query
        orgService.Data().Add(Arthur.Contact()); 
        
        var fetch = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                       <entity name=""contact"">
                        <attribute name=""lastname"" />
                        <attribute name=""firstname"" />
                        <filter type=""and"">
                          <condition attribute=""lastname"" operator=""eq"" value=""Nicholson"" />
                        </filter>
                      </entity>
                    </fetch>";

        var query = new FetchExpression { Query = fetch };
        var contacts = orgService.RetrieveMultiple(query);

        contacts.Entities.Count.Should().Be(2);
    }

    [Test]
    public void Basic_Fetch_With_No_Results_Returns_Empty_Dataset()
    {
      var orgService = _organizationService.Simulate();
      orgService.Data().Reinitialise();

      var fetch = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                       <entity name=""contact"">
                        <attribute name=""lastname"" />
                        <attribute name=""firstname"" />
                        <filter type=""and"">
                          <condition attribute=""lastname"" operator=""eq"" value=""Nicholson"" />
                        </filter>
                      </entity>
                    </fetch>";

      var query = new FetchExpression { Query = fetch };
      var contacts = orgService.RetrieveMultiple(query);

      contacts.Entities.Count.Should().Be(0);
    }
    
    [Test]
    public void Basic_NotEquals_Fetch_Returns_Correct_Records()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Reinitialise();
        
        orgService.Data().Add(Bruce.Contact());
        // Should not be returned in query
        orgService.Data().Add(Siobhan.Contact());
        
        var fetch = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                       <entity name=""contact"">
                        <attribute name=""lastname"" />
                        <attribute name=""firstname"" />
                        <filter type=""and"">
                          <condition attribute=""lastname"" operator=""ne"" value=""Nicholson"" />
                        </filter>
                      </entity>
                    </fetch>";

        var query = new FetchExpression { Query = fetch };
        var contacts = orgService.RetrieveMultiple(query);

        contacts.Entities.Count.Should().Be(1);
    }
    
    [Test]
    public void Basic_Fetch_Returns_Correct_Columns()
    {
      var orgService = _organizationService.Simulate();
      orgService.Data().Reinitialise();
        
      orgService.Data().Add(Siobhan.Contact());
      orgService.Data().Add(Arthur.Contact()); 
        
      var fetch = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                       <entity name=""contact"">
                        <attribute name=""lastname"" />
                        <attribute name=""firstname"" />
                        <filter type=""and"">
                          <condition attribute=""lastname"" operator=""eq"" value=""Nicholson"" />
                        </filter>
                      </entity>
                    </fetch>";

      var query = new FetchExpression { Query = fetch };
      var contacts = orgService.RetrieveMultiple(query);

      contacts.Entities.Count.Should().Be(1);

      contacts.Entities.FirstOrDefault()?.Attributes[Contact.Fields.firstname].Should().Be(Siobhan.Contact().firstname);
      contacts.Entities.FirstOrDefault()?.Attributes[Contact.Fields.lastname].Should().Be(Siobhan.Contact().lastname);

      var sut = () => contacts.Entities.FirstOrDefault()?.Attributes[Contact.Fields.birthdate];
      sut.Should().Throw<KeyNotFoundException>();
    }

    [Test]
    public void Basic_Fetch_Query_Returns_Correct_Order()
    {
      var orgService = _organizationService.Simulate();
      orgService.Data().Reinitialise();
        
      orgService.Data().Add(Siobhan.Contact());
      orgService.Data().Add(Daniel.Contact()); 
      
      var fetch = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                      <entity name=""contact"">
                        <attribute name=""lastname"" />
                        <attribute name=""firstname"" />
                        <order attribute=""firstname"" descending=""false"" />
                        <filter type=""and"">
                          <condition attribute=""lastname"" operator=""eq"" value=""Nicholson"" />
                        </filter>
                      </entity>
                    </fetch>";
      
      var query = new FetchExpression { Query = fetch };
      var contacts = orgService.RetrieveMultiple(query);

      contacts.Entities.Count.Should().Be(2);
      contacts.Entities[0].Attributes[Contact.Fields.firstname].Should().Be(Daniel.Contact().firstname);
      contacts.Entities[1].Attributes[Contact.Fields.firstname].Should().Be(Siobhan.Contact().firstname);
    }

    [Test]
    public void Fetch_Query_With_Linked_Entity_Returns_Correct_Related_Columns()
    {
      var orgService = _organizationService.Simulate();
      orgService.Data().Reinitialise();
      
      orgService.Data().Add(Arthur.Contact());
      orgService.Data().Add(Arthur.Account());
      
      var fetch = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""true"">
                      <entity name=""contact"">
                        <attribute name=""lastname"" />
                        <attribute name=""firstname"" />
                        <attribute name=""parentcustomerid"" />
                        <filter type=""and"">
                          <condition attribute=""lastname"" operator=""eq"" value=""Nicholson-Gumula"" />
                        </filter>
                        <link-entity name=""account"" to=""Id"" from=""parentcustomerid"" visible=""false"" link-type=""outer"" alias=""account"">
                          <attribute name=""name"" />
                          <attribute name=""Id"" />
                        </link-entity>                
                      </entity>
                    </fetch>";
      
      var query = new FetchExpression { Query = fetch };
      var contacts = orgService.RetrieveMultiple(query);

      contacts.Entities.Count.Should().Be(1);
      contacts.Entities.FirstOrDefault()?.Attributes["account.name"]
        .Should().Be(Arthur.Account().Attributes["name"]);
    }

    [Test]
    public void Fetch_Query_With_Linked_Filters_Returned_Dataset()
    {
      var orgService = _organizationService.Simulate();
      orgService.Data().Reinitialise();
      
      orgService.Data().Add(Arthur.Contact());
      orgService.Data().Add(Arthur.Account());
      orgService.Data().Add(Daniel.Contact()); // Should not be returned in results
      
      var fetch = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""true"">
                      <entity name=""contact"">
                        <attribute name=""lastname"" />
                        <attribute name=""firstname"" />
                        <attribute name=""parentcustomerid"" />
                        <order attribute=""firstname"" descending=""false"" />
                        <link-entity name=""account"" to=""Id"" from=""parentcustomerid"" visible=""false"" link-type=""outer"" alias=""account"">
                          <attribute name=""name"" />
                          <attribute name=""Id"" />
                          <filter type=""and"">
                            <condition attribute=""name"" operator=""eq"" value=""Cloud Awesome Limited"" />
                          </filter>
                        </link-entity>
                      </entity>
                    </fetch>";
      
      var query = new FetchExpression { Query = fetch };
      var contacts = orgService.RetrieveMultiple(query);

      contacts.Entities.Count.Should().Be(1);
    }

    [Test]
    [Ignore("TODO - To be implemented")]
    public void Fetch_Query_Returns_Valid_Results_If_Filter_Attributes_Are_Not_In_ColumnSet()
    {
      
    }
    
    [Test]
    [Ignore("TODO - To be implemented")]
    public void Fetch_Query_Returns_Valid_Results_If_LinkEntity_Attributes_Are_Not_In_ColumnSet()
    {
      
    }
    
    #region Functionality To Be Implemented
    
    [Test]
    [Ignore("To be implemented")]
    public void Fetch_Query_Returns_Top_x_Results()
    {
      
    }

    [Test]
    [Ignore("To be implemented")]
    public void Fetch_Query_Returns_Sum_Aggregate()
    {
      
    }
    
    [Test]
    [Ignore("To be implemented")]
    public void Fetch_Query_Returns_Average_Aggregate()
    {
      
    }
    #endregion
}