using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class Last7DaysConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.Last7Days;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        
        return attributeValue.Date >= dataService.SystemTime.Date.AddDays(-7) && 
               attributeValue.Date <= dataService.SystemTime.Date;
    }
}