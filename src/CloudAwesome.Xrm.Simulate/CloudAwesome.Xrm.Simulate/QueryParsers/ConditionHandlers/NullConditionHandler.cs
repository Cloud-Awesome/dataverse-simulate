using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class NullConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.Null;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<object>(condition.AttributeName);
        if (attributeValue is OptionSetValue value) attributeValue = value.Value;
        
        return attributeValue == null;
    }
}