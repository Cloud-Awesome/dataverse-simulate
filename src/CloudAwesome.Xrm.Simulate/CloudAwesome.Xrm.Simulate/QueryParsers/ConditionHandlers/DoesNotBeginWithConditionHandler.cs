using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class DoesNotBeginWithConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.DoesNotBeginWith;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<string>(condition.AttributeName);
        return attributeValue == null || !attributeValue.StartsWith(condition.Values[0].ToString());
    }
}