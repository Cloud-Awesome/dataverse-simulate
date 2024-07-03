﻿using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests.OrganizationRequests;

public class WhoAmIRequestHandler: IRequestHandler
{
	public OrganizationResponse Handle(OrganizationRequest request, MockedEntityDataService dataService,
		SimulatorAuditService auditService, ISimulatorOptions? options = null)
	{
		var userId = dataService.AuthenticatedUser.Id;
		var businessUnitId = dataService.BusinessUnit.Id;
		var organizationId = dataService.Organization.Id;

		var response = new WhoAmIResponse
		{
			Results = new ParameterCollection
			{
				{ "UserId", userId },
				{ "BusinessUnitId", businessUnitId },
				{ "OrganizationId", organizationId }
			}
		};

		return response;
	}
}