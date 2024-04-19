using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class LessEqualConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.LessEqual;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<IComparable>(condition.AttributeName);
        return attributeValue.CompareTo(condition.Values[0]) <= 0;
    }
}