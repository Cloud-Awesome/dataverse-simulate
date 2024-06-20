using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class OrganisationRequestExecutor
    (MockedEntityDataService dataService, SimulatorAuditService auditService, RequestHandlerRegistry handlerRegistry): IOrganisationRequestExecutor
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        organizationService.Execute(Arg.Any<OrganizationRequest>())
            .Returns(x =>
            {
                var request = x.Arg<OrganizationRequest>();
                var handler = handlerRegistry.GetHandler(request);
                return handler.Handle(request, dataService, auditService, options);
            });
    }
}