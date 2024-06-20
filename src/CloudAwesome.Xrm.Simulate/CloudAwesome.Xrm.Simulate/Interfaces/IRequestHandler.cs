using CloudAwesome.Xrm.Simulate.DataServices;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Interfaces;

public interface IRequestHandler
{
	OrganizationResponse Handle(
		OrganizationRequest request, 
		MockedEntityDataService dataService, SimulatorAuditService auditService, 
		ISimulatorOptions? options = null);
}
