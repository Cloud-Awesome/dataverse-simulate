using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Gather;

/// <summary>
/// Base features for test fixtures integrating with Dataverse.
///
/// Currently doesn't do much, just ensures the connection is disposed at the end of a fixture. 
/// </summary>
public abstract class IntegrationBaseFixture
{
    [OneTimeTearDown]
    public void TestFixtureTearDown()
    {
        DataverseConnectionManager.Instance.DisposeConnection();
    }
}