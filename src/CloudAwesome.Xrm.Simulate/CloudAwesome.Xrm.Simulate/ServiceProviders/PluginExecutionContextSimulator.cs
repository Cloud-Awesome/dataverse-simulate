using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceProviders;

public static class PluginExecutionContextSimulator
{
    public static IPluginExecutionContext? Create(ISimulatorOptions? options)
    {
        var dataService = new MockedEntityDataService();
        if (dataService.FakeServiceFailureSettings is { PluginExecutionContext: true })
        {
            return null;
        }
        
        var pluginExecutionContext = Substitute.For<IPluginExecutionContext>();

        pluginExecutionContext.UserId.Returns(x => 
            options?.AuthenticatedUser?.Id ?? 
            dataService.AuthenticatedUser.Id);
        
        if (dataService.ExecutionContext is null) return pluginExecutionContext;

        var context = dataService.ExecutionContext;
        
        pluginExecutionContext.InitiatingUserId.Returns(x => 
            options?.AuthenticatedUser?.Id ?? 
            dataService.AuthenticatedUser.Id);
        
        pluginExecutionContext.Depth.Returns(context.Depth);
        pluginExecutionContext.Stage.Returns(context.Stage);
        
        pluginExecutionContext.InputParameters.Returns(context.InputParameters);
        pluginExecutionContext.PreEntityImages.Returns(context.PreEntityImages);
        pluginExecutionContext.PostEntityImages.Returns(context.PostEntityImages);
        
        return pluginExecutionContext;
    }
}