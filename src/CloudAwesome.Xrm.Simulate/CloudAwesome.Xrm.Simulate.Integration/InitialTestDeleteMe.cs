using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Gather;

/// <summary>
/// This intended use of this project is to create both real integration and mocked tests.
/// 
/// 1. Compare the results of a live API call with the results of a mocked API call;
///     and fix any issues found in both .Simulate and .Simulate.Test
///
/// 2. Investigate what a live API call returns (e.g. which exception/details is thrown in x case);
///     Implement it in the mock; test that the two are equivalent (i.e. Green-Gather-Red-Green)
///
/// 3. When MS releases a new version, re-run all tests to see if any implementation has changed etc; 
///     Mirror in the mock etc...
/// 
/// </summary>
[TestFixture]
public class InitialTestDeleteMe
{
    private readonly IOrganizationService _mockService = null!; 
    
    [Test]
    public void Temp()
    {
        // Live
        var service = DataverseConnectionManager.Instance.GetConnection();

        var arthurId = service.Create(_arthur);
        var retrievedArthur = 
            service.Retrieve(_arthur.LogicalName, arthurId, new ColumnSet("firstname", "lastname"));
        service.Delete(_arthur.LogicalName, arthurId);
        DataverseConnectionManager.Instance.DisposeConnection();

        // Mocked
        var mockService = _mockService.Simulate();
        mockService.Data().Reinitialise();
        
        var mockedArthurId = mockService.Create(_arthur);
        var mockRetrievedArthur = 
            mockService.Retrieve(_arthur.LogicalName, mockedArthurId, new ColumnSet("firstname", "lastname"));

        // Assert
        arthurId.Should().NotBeEmpty();
        mockedArthurId.Should().NotBeEmpty();

        mockRetrievedArthur.Attributes.Should().BeEquivalentTo(retrievedArthur.Attributes);
        // TODO - fails - need to always return the primary key, even if it isn't asked for in the column set!
        //mockRetrievedArthur.FormattedValues.Should().BeEquivalentTo(retrievedArthur.FormattedValues);
        
    }
    
    private readonly Entity _arthur = new Entity("contact")
    {
        Attributes = new AttributeCollection
        {
            new("firstname", "Arthur"),
            new("lastname", "Nicholson-Gumula"),
        }
    };
}