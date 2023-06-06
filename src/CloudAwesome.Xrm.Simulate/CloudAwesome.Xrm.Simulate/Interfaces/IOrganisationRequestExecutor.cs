using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Interfaces;

public interface IOrganisationRequestExecutor
{
    void MockRequest(IOrganizationService organizationService, ISimulatorOptions? options = null);
}