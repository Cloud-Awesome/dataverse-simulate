using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class LastXWeeksConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.LastXWeeks;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var dataStore = new MockedEntityDataService();
        
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var weeks = Convert.ToInt32(condition.Values[0]);
        return attributeValue.Date >= dataStore.SystemTime.AddDays(-7 * weeks) && 
               attributeValue.Date <= dataStore.SystemTime;
    }
}