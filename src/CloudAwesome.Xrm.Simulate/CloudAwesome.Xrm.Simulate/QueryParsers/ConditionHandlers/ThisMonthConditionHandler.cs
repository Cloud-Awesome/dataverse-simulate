using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class ThisMonthConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.ThisMonth;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var dataStore = new MockedEntityDataService();
        
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var thisMonthStart = new DateTime(dataStore.SystemTime.Year, 
            dataStore.SystemTime.Month, 1);
        var thisMonthEnd = new DateTime(dataStore.SystemTime.Year, 
            dataStore.SystemTime.Month + 1, 1).AddDays(-1);
        
        return attributeValue.Date >= thisMonthStart && attributeValue.Date < thisMonthEnd;
    }
}