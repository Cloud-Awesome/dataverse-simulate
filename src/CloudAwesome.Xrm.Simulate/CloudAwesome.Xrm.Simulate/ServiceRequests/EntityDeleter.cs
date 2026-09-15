using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;
using System.ServiceModel;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public sealed class EntityDeleter(MockedEntityDataService dataService) : IEntityDeleter
{
    private const string RequestMessage = "Delete";
    private const int ObjectDoesNotExistErrorCode = -2147220969;
    
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        organizationService.When(x => 
            x.Delete(Arg.Any<string>(), Arg.Any<Guid>()))
            .Do(x =>
            {
                var entityName = x.Arg<string>();
                var id = x.Arg<Guid>();
             
                RequestFailureHandler.Handle(options, RequestMessage, id);

                this.ValidateExists(entityName, id);
                
                dataService.Delete(entityName, id);
            });
    }

    private void ValidateExists(string logicalName, Guid id)
    {
        if (dataService.Get(logicalName).Any(entity => entity.Id == id))
        {
            return;
        }

        var message = $"Entity '{GetEntityDisplayName(logicalName)}' With Id = {id} Does Not Exist";
        var fault = new OrganizationServiceFault
        {
            ErrorCode = ObjectDoesNotExistErrorCode,
            Message = message
        };

        throw new FaultException<OrganizationServiceFault>(fault, new FaultReason(message));
    }

    private static string GetEntityDisplayName(string logicalName)
    {
        return string.IsNullOrEmpty(logicalName)
            ? logicalName
            : char.ToUpperInvariant(logicalName[0]) + logicalName[1..];
    }
}
