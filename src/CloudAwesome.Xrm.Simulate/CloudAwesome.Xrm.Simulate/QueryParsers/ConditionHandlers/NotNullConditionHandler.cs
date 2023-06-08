using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class NotNullConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.NotNull;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var attributeValue = entity.GetAttributeValue<object>(condition.AttributeName);
        if (attributeValue is OptionSetValue value) attributeValue = value.Value;
        
        return attributeValue != null;
    }
}