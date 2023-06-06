using CloudAwesome.Xrm.Simulate.DataStores;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Simulate.DataServices;

public class SimulatorAuditService
{
    public void Add(string message, string? entityLogicalName, Guid? id)
    {
        SimulatorAuditStore.Instance.Logs.Add(
            new SimulatorAudit(message, entityLogicalName, id));
    }
    
    public void Clear()
    {
        SimulatorAuditStore.Instance.Logs.Clear();
    }

    public List<SimulatorAudit> Get()
    {
        return SimulatorAuditStore.Instance.Logs;
    }

    public List<SimulatorAudit> Get(string message)
    {
        return SimulatorAuditStore.Instance.Logs
            .Where(x => x.Message == message)
            .ToList();
    }
    
    public List<SimulatorAudit> Get(Guid id)
    {
        return SimulatorAuditStore.Instance.Logs
            .Where(x => x.Id == id)
            .ToList();
    }
}