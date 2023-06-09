using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class ThisWeekConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.ThisWeek;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var dataStore = new MockedEntityDataService();
        
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var thisWeekStart = 
            dataStore.SystemTime.Date
                .AddDays(-(int)dataStore.SystemTime.Date.DayOfWeek);
        var thisWeekEnd = thisWeekStart.AddDays(7);
        
        return attributeValue.Date >= thisWeekStart && attributeValue.Date < thisWeekEnd;
    }
}