namespace CloudAwesome.Xrm.Simulate.DataStores;

public class SimulatorAuditStore
{
    public List<SimulatorAudit> Logs { get; private set; } = new();
}