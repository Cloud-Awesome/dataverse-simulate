using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class GreaterEqualConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.GreaterEqual;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<IComparable>(condition.AttributeName);
        var conditionValue = ConditionExpressionParser.ParseIntCondition(condition.Values[0]);
        
        return attributeValue.CompareTo(conditionValue) >= 0;
    }
}