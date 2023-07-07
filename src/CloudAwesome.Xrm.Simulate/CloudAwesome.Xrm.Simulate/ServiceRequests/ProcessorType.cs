namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class ProcessorType
{
    public ProcessorType(string entityName, ProcessorMessage message)
    {
        EntityName = entityName;
        Message = message;
    }

    public string EntityName { get; set; }
    
    public ProcessorMessage Message { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is ProcessorType key)
        {
            return EntityName == key.EntityName && Message == key.Message;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(EntityName, Message);
    }
}