using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Interfaces;

public interface IEntityProcessor
{
    Entity Process(Entity entity);
}