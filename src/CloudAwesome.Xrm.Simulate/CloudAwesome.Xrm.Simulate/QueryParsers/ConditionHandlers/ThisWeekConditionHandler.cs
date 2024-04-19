using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class ThisWeekConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.ThisWeek;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var thisWeekStart = 
            dataService.SystemTime.Date
                .AddDays(-(int)dataService.SystemTime.Date.DayOfWeek);
        var thisWeekEnd = thisWeekStart.AddDays(7);
        
        return attributeValue.Date >= thisWeekStart && attributeValue.Date < thisWeekEnd;
    }
}