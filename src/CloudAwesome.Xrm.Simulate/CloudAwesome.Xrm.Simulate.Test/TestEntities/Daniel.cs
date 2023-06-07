using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Test.TestEntities;

/// <summary>
/// Describe usage of the Siobhan persona, so that it's visible as a tool tip in tests... =D
/// </summary>
public static class Daniel
{
    /// <summary>
    /// Describe usage of the Siobhan contact, so that it's visible as a tool tip in tests... =D
    /// </summary>
    public static Contact Contact()
    {
        return new Contact
        {
            firstname = "Daniel",
            lastname = "Nicholson",
            gendercode = Contact_gendercode.Male,
            statuscode = Contact_StatusCode.Active
        };
    }

    /// <summary>
    /// Describe usage of the Siobhan Account, so that it's visible as a tool tip in tests... =D
    /// </summary>
    public static Entity Account()
    {
        return new Entity("account");
    }
}