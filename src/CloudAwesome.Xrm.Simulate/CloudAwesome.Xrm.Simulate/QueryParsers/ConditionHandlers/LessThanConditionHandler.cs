using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class LessThanConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.LessThan;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var attributeValue = entity.GetAttributeValue<IComparable>(condition.AttributeName);
        return attributeValue.CompareTo(condition.Values[0]) < 0;
    }
}