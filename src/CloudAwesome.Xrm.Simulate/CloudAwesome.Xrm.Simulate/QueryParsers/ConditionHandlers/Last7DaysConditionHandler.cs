using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class Last7DaysConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.Last7Days;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var dataStore = new MockedEntityDataService();
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        
        return attributeValue.Date >= dataStore.SystemTime.Date.AddDays(-7) && 
               attributeValue.Date <= dataStore.SystemTime.Date;
    }
}