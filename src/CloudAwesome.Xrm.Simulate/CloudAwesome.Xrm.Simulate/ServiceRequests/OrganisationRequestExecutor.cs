using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
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
}
}