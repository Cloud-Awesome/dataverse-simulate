using System;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Test.TestEntities;

/// <summary>
/// Describe usage of the Arthur persona, so that it's visible as a tool tip in tests... =D
/// </summary>
public static class Arthur
{
    /// <summary>
    /// Describe usage of the Arthur contact, so that it's visible as a tool tip in tests... =D
    /// </summary>
    public static Contact Contact()
    {
        return new Contact
        {
            ContactId = Guid.Parse("22c4ba07-5df0-4bed-aacf-276270b75f2f"),
            FirstName = "Arthur",
            LastName = "Nicholson-Gumula",
            Birthdate = new DateTime(1984, 12, 14),
            ParentCustomerId = Account().ToEntityReference(),
            GenderCode = Contact_GenderCode.Male,
            StatusCode = Contact_StatusCode.Active,
            OverriddenCreatedOn = new DateTime(2023, 04, 18),
            NumberOfChildren = 0
        };
    }

    /// <summary>
    /// Describe usage of the Arthur Account, so that it's visible as a tool tip in tests... =D
    /// </summary>
    public static Entity Account()
    {
        return new Entity("account", Guid.Parse("9b19f826-6ca7-456a-ae73-3ffbd687cd2b"))
        {
            Attributes =
            {
                ["Id"] = Guid.Parse("9b19f826-6ca7-456a-ae73-3ffbd687cd2b"),
                ["name"] = "Cloud Awesome Limited"
            }
        };
    }
}