using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Interfaces;

public interface IEntityRequestBase
{
    void MockRequest(IOrganizationService organizationService, ISimulatorOptions? simulatorOptions = null);
}