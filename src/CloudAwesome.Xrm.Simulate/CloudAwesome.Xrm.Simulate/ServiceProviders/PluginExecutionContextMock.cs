using CloudAwesome.Xrm.Simulate.DataServices;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.ServiceProviders;

public class PluginExecutionContextMock: IPluginExecutionContext
{
    private readonly MockedEntityDataService _dataService = new MockedEntityDataService();
    
    public int Mode { get; set; }
    public int IsolationMode { get; set; }
    public int Depth { get; set; }
    public string MessageName { get; set; }
    public string PrimaryEntityName { get; set; }
    public Guid? RequestId { get; set; }
    public string SecondaryEntityName { get; set; }
    public ParameterCollection InputParameters { get; set; }
    public ParameterCollection OutputParameters { get; set; }
    public ParameterCollection SharedVariables { get; set; }
    public Guid UserId { get; }
    public Guid InitiatingUserId { get; }
    public Guid BusinessUnitId => _dataService.BusinessUnit.Id;
    public Guid OrganizationId => _dataService.Organization.Id;
    public string OrganizationName => _dataService.Organization.Name;
    public Guid PrimaryEntityId { get; }
    public EntityImageCollection PreEntityImages { get; }
    public EntityImageCollection PostEntityImages { get; }
    public EntityReference OwningExtension { get; }
    public Guid CorrelationId { get; }
    public bool IsExecutingOffline { get; }
    public bool IsOfflinePlayback { get; }
    public bool IsInTransaction { get; }
    public Guid OperationId { get; }
    public DateTime OperationCreatedOn { get; }
    public int Stage { get; }
    public IPluginExecutionContext ParentContext { get; }
}