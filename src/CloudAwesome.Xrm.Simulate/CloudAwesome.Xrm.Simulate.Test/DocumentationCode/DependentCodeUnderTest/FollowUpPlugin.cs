using System;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Test.DocumentationCode.DependentCodeUnderTest;

public sealed class FollowUpPlugin : IPlugin 
{
    public void Execute(IServiceProvider serviceProvider)
    {
        ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        
        IPluginExecutionContext context = (IPluginExecutionContext)
            serviceProvider.GetService(typeof(IPluginExecutionContext));
        
        Entity followup = new Entity("task");
        followup["subject"] = "Follow up on your call";

        if (context.InputParameters.Contains("Target"))
        {
            var target = (Entity) context.InputParameters["Target"];
            followup["regardingid"] = target.ToEntityReference();    
        }
        
        IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
        
        tracingService.Trace("FollowUpPlugin: Successfully created the task activity.");
        service.Create(followup);
    }
}