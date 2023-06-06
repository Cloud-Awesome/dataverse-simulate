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
    public static Entity Contact()
    {
        return new Entity("contact");
    }

    /// <summary>
    /// Describe usage of the Arthur Account, so that it's visible as a tool tip in tests... =D
    /// </summary>
    public static Entity Account()
    {
        return new Entity("account");
    }
}