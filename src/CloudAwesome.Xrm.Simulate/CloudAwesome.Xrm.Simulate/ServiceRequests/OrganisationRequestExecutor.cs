using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.QueryParsers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Moq;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class OrganisationRequestExecutor: IOrganisationRequestExecutor
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        var systemTime = options?.ClockSimulator?.Now ?? DateTime.Now;

        // CreateRequest
        Mock.Get(organizationService)
            .Setup(x => x.Execute(It.IsAny<CreateRequest>()))
            .Returns((CreateRequest request) => 
            {
                var createdId = new EntityCreator().Create(request.Target, systemTime);
                return new CreateResponse
                {
                    Results = new ParameterCollection { new("id", createdId) },
                    ResponseName = "Create"
                };
            });
        
        // Retrieve Multiple -> TODO - Only QueryExpression is supported atm...
        Mock.Get(organizationService)
            .Setup(x => x.Execute(It.IsAny<RetrieveMultipleRequest>()))
            .Returns((RetrieveMultipleRequest request) =>
            {
                var results = QueryExpressionParser.Parse(
                    (QueryExpression) request.Query,
                    MockedEntityDataStore.Instance.Data);
                
                return new RetrieveMultipleResponse
                {
                    Results = new ParameterCollection
                    {
                        ["EntityCollection"] = new EntityCollection(results.ToList())
                    },
                    ResponseName = "RetrieveMultiple"
                };
            });
}
}