namespace CloudAwesome.Xrm.Simulate.DataStores;

public class SimulatorAudit
{
    public SimulatorAudit(string message, string? entityLogicalName, Guid? id)
    {
        Message = message;
        EntityLogicalName = entityLogicalName;
        Id = id;
    }

    public string Message { get; set; }
    public Guid? Id { get; set; }
    public string? EntityLogicalName { get; set; }

    public override string ToString()
    {
        return $"'{Message}' triggered on '{EntityLogicalName}' with ID '{Id}'";
    }
}