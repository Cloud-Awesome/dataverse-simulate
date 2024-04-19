using CloudAwesome.Xrm.Simulate.DataStores;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Simulate.DataServices;

public class SimulatorAuditService
{
    private readonly SimulatorAuditStore _simulatorAuditStore = new();
    
    public void Add(string message, string? entityLogicalName, Guid? id)
    {
        _simulatorAuditStore.Logs.Add(
            new SimulatorAudit(message, entityLogicalName, id));
    }
    
    public void Clear()
    {
        _simulatorAuditStore.Logs.Clear();
    }

    public List<SimulatorAudit> Get()
    {
        return _simulatorAuditStore.Logs;
    }

    public List<SimulatorAudit> Get(string message)
    {
        return _simulatorAuditStore.Logs
            .Where(x => x.Message == message)
            .ToList();
    }
    
    public List<SimulatorAudit> Get(Guid id)
    {
        return _simulatorAuditStore.Logs
            .Where(x => x.Id == id)
            .ToList();
    }
}