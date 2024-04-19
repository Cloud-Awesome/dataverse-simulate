using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class LastXHoursConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.LastXHours;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var hours = Convert.ToInt32(condition.Values[0]);
        
        return attributeValue >= dataService.SystemTime.AddHours(-hours) 
               && attributeValue <= dataService.SystemTime;
    }
}