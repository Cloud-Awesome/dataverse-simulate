using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class LastXMonthsConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.LastXMonths;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var months = Convert.ToInt32(condition.Values[0]);
        
        return attributeValue.Date >= dataService.SystemTime.Date.AddMonths(-months) 
               && attributeValue.Date <= dataService.SystemTime.Date;
    }
}