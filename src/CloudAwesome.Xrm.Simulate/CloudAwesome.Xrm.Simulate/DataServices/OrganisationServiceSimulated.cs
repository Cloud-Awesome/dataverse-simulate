namespace CloudAwesome.Xrm.Simulate.DataServices;

public class OrganisationServiceSimulated
{
    private readonly MockedEntityDataService _dataService;
    private readonly SimulatorAuditService _auditService;

    public OrganisationServiceSimulated(MockedEntityDataService dataService, SimulatorAuditService auditService)
    {
        _dataService = dataService;
        _auditService = auditService;
    }
    
    public MockedEntityDataService Data()
    {
        return _dataService;
    }

    public SimulatorAuditService Audit()
    {
        return _auditService;
    }
}