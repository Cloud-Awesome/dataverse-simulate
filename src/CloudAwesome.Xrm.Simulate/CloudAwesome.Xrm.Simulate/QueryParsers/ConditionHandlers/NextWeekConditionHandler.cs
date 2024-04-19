using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class NextWeekConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.NextWeek;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var nextWeekStart = 
            dataService.SystemTime.Date
                .AddDays(-(int)dataService.SystemTime.Date.DayOfWeek)
                .AddDays(7);
        var nextWeekEnd = nextWeekStart.AddDays(7);
        
        return attributeValue.Date >= nextWeekStart && attributeValue.Date < nextWeekEnd;
    }
}