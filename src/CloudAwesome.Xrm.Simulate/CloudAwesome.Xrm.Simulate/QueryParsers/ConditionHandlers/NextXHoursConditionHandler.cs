using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class NextXHoursConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.NextXHours;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var dataStore = new MockedEntityDataService();
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var hours = Convert.ToInt32(condition.Values[0]);
        
        return attributeValue.Date <= dataStore.SystemTime.AddHours(hours) 
               && attributeValue.Date >= dataStore.SystemTime;
    }
}