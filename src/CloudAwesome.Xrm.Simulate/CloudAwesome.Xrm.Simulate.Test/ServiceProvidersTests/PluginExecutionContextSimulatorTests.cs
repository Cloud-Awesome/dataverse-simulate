using System;
using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.ServiceProvidersTests;

[TestFixture]
public class PluginExecutionContextSimulatorTests
{
    private readonly IServiceProvider _serviceProvider = null!;

    [Test]
    public void User_From_Context_Is_Set_From_Simulator_Options()
    {
        var options = new SimulatorOptions
        {
            AuthenticatedUser = Arthur.Contact()
        };
        
        var service = _serviceProvider.Simulate(options);
        var executionContext = (IPluginExecutionContext)service.GetService(typeof(IPluginExecutionContext))!;

        executionContext.UserId.Should().Be(Arthur.Contact().Id);
    }
}