using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class NextXWeeksConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.NextXWeeks;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var weeks = Convert.ToInt32(condition.Values[0]);
        return attributeValue.Date <= dataService.SystemTime.AddDays(7 * weeks) && 
               attributeValue.Date >= dataService.SystemTime;
    }
}