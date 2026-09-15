using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests.OrganizationRequests;

public class RetrieveRequestHandler : IRequestHandler
{
    public OrganizationResponse Handle(
        OrganizationRequest request,
        MockedEntityDataService dataService,
        SimulatorAuditService auditService,
        ISimulatorOptions? options = null)
    {
        var retrieveRequest = (RetrieveRequest)request;
        var entity = new EntityRetriever(dataService, auditService).Retrieve(
            retrieveRequest.Target.LogicalName,
            retrieveRequest.Target.Id,
            retrieveRequest.ColumnSet,
            options);

        return new RetrieveResponse
        {
            Results = new ParameterCollection
            {
                ["Entity"] = entity
            },
            ResponseName = "Retrieve"
        };
    }
}
