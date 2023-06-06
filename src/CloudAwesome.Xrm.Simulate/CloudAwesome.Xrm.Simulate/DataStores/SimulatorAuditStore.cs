namespace CloudAwesome.Xrm.Simulate.DataStores;

public class SimulatorAuditStore
{
    private static readonly Lazy<SimulatorAuditStore> _instance =
        new Lazy<SimulatorAuditStore>(() => new SimulatorAuditStore());

    public static SimulatorAuditStore Instance => _instance.Value;

    public List<SimulatorAudit> Logs { get; private set; }

    private SimulatorAuditStore()
    {
        Logs = new List<SimulatorAudit>();
    }
}