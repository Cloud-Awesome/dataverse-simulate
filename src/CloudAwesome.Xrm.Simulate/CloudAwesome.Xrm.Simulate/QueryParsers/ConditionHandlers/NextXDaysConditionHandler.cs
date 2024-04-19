using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class NextXDaysConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.NextXDays;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var days = Convert.ToInt32(condition.Values[0]);
        
        return attributeValue.Date <= dataService.SystemTime.Date.AddDays(days) 
               && attributeValue.Date >= dataService.SystemTime.Date;
    }
}