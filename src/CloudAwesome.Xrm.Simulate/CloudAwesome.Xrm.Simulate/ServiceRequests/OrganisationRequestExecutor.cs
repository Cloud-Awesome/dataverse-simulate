using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.QueryParsers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class OrganisationRequestExecutor(MockedEntityDataService dataService): IOrganisationRequestExecutor
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        dataService = organizationService.Simulated().Data();
        
        // CreateRequest
        organizationService.Execute(Arg.Any<CreateRequest>())
            .Returns(x =>
            {
                var request = x.Arg<CreateRequest>();

                var createdId = new EntityCreator(dataService).Create(request.Target, options);
                return new CreateResponse
                {
                    Results = new ParameterCollection { new("id", createdId) },
                    ResponseName = "Create"
                };
            });
        
        // Retrieve Multiple -> TODO - QueryByAttribute is not yet supported
        organizationService.Execute(Arg.Any<RetrieveMultipleRequest>())
            .Returns(x =>
            {
                var request = x.Arg<RetrieveMultipleRequest>();
                
                var results = QueryExpressionParser.Parse(
                    (QueryExpression) request.Query,
                    dataService.Get(), dataService);
                
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