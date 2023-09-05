using System;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Test.TestEntities;

/// <summary>
/// Describe usage of the Siobhan persona, so that it's visible as a tool tip in tests... =D
/// </summary>
public static class Siobhan
{
    /// <summary>
    /// Describe usage of the Siobhan contact, so that it's visible as a tool tip in tests... =D
    /// </summary>
    public static Contact Contact()
    {
        return new Contact
        {
            contactid = Guid.Parse("c9db861c-d362-4c54-8687-14d05d1d8f92"),
            firstname = "Siobhan",
            lastname = "Nicholson",
            gendercode = Contact_gendercode.Female,
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