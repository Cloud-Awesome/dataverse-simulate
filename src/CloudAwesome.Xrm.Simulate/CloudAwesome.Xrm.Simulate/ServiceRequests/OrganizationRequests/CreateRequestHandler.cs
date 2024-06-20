using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests.OrganizationRequests;

public class CreateRequestHandler: IRequestHandler
{
	public OrganizationResponse Handle(OrganizationRequest request, MockedEntityDataService dataService,
		SimulatorAuditService auditService, ISimulatorOptions? options = null)
	{
		var createRequest = (CreateRequest) request;
		
		var createdId = new EntityCreator(dataService, auditService).Create(createRequest.Target, options);
		return new CreateResponse
		{
			Results = new ParameterCollection { new("id", createdId) },
			ResponseName = "Create"
		};
	}
}