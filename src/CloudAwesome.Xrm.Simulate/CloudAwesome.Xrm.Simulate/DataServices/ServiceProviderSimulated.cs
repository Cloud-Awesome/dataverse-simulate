namespace CloudAwesome.Xrm.Simulate.DataServices;

public class ServiceProviderSimulated
{
    private readonly MockedEntityDataService _dataService;
    private readonly MockedLoggingService _loggingService;
    private readonly MockedTelemetryService _telemetryService;
    private readonly SimulatorAuditService _simulatorAuditService;

    public ServiceProviderSimulated(MockedEntityDataService dataService, MockedLoggingService loggingService, 
        MockedTelemetryService telemetryService, SimulatorAuditService simulatorAuditService)
    {
        _dataService = dataService;
        _loggingService = loggingService;
        _telemetryService = telemetryService;
        _simulatorAuditService = simulatorAuditService;
    }

    public MockedEntityDataService Data()
    {
        return _dataService;
    }

    public MockedLoggingService Logs()
    {
        return _loggingService;
    }

    public MockedTelemetryService Telemetry()
    {
        return _telemetryService;
    }

    public SimulatorAuditService Audits()
    {
        return _simulatorAuditService;
    }
}