using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceProviders;

public static class PluginExecutionContextSimulator
{
    public static IPluginExecutionContext Create(ISimulatorOptions? options)
    {
        var pluginExecutionContext = Substitute.For<IPluginExecutionContext>();
        var dataService = new MockedEntityDataService();
        
        // TODO - inputs and context for plugin context...
        // pluginExecutionContext.Depth
        // pluginExecutionContext.InputParameters
        // pluginExecutionContext.PreEntityImages
        
        pluginExecutionContext.UserId.Returns(x => options?.AuthenticatedUser?.Id ?? dataService.AuthenticatedUser.Id);

        return pluginExecutionContext;
    }
}