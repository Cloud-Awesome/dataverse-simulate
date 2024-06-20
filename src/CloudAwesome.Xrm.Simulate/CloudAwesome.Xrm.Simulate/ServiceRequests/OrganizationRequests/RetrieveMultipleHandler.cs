using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.QueryParsers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests.OrganizationRequests;

public class RetrieveMultipleHandler: IRequestHandler
{
	public OrganizationResponse Handle(OrganizationRequest request, MockedEntityDataService dataService,
		SimulatorAuditService auditService, ISimulatorOptions? options = null)
	{
		var retrieveMultipleRequest = (RetrieveMultipleRequest) request;
		
		var results = QueryExpressionParser.Parse(
			(QueryExpression) retrieveMultipleRequest.Query,
			dataService.Get(), dataService);
		
		return new RetrieveMultipleResponse
		{
			Results = new ParameterCollection
			{
				["EntityCollection"] = new EntityCollection(results.ToList())
			},
			ResponseName = "RetrieveMultiple"
		};
	}
}