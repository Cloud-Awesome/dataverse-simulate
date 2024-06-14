using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceProviders;

public static class PluginExecutionContextSimulator
{
    public static IPluginExecutionContext? Create(MockedEntityDataService dataService, ISimulatorOptions? options)
    {
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
        
        pluginExecutionContext.PrimaryEntityId.Returns(context.PrimaryEntityId);
        pluginExecutionContext.PrimaryEntityName.Returns(context.PrimaryEntityName);
        
        pluginExecutionContext.InitiatingUserId.Returns(x => 
            options?.AuthenticatedUser?.Id ?? 
            dataService.AuthenticatedUser.Id);
        
        pluginExecutionContext.Depth.Returns(context.Depth);
        pluginExecutionContext.Stage.Returns(context.Stage);
        
        pluginExecutionContext.InputParameters.Returns(context.InputParameters);
        pluginExecutionContext.OutputParameters.Returns(context.OutputParameters);
        pluginExecutionContext.SharedVariables.Returns(context.SharedVariables);
        
        pluginExecutionContext.PreEntityImages.Returns(context.PreEntityImages);
        pluginExecutionContext.PostEntityImages.Returns(context.PostEntityImages);
        
        return pluginExecutionContext;
    }
}