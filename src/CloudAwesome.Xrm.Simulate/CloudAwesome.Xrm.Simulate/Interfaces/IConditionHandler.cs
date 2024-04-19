using CloudAwesome.Xrm.Simulate.DataServices;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.Interfaces;

public interface IConditionHandler
{
    ConditionOperator Operator { get; }
    bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService);
}