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

        int parsedCondition;
        if (condition.Values[0] is string)
        {
            parsedCondition = int.Parse(condition.Values[0].ToString() ?? string.Empty);
        }
        else
        {
            parsedCondition = (int)condition.Values[0];
        }
        
        return attributeValue.CompareTo(parsedCondition) >= 0;
    }
}