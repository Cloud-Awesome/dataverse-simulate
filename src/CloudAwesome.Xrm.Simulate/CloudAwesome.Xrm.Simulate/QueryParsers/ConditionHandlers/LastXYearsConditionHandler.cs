using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class LastXYearsConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.LastXYears;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var dataStore = new MockedEntityDataService();
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var years = Convert.ToInt32(condition.Values[0]);
        
        return attributeValue.Date >= dataStore.SystemTime.Date.AddYears(-years) 
               && attributeValue.Date <= dataStore.SystemTime.Date;
    }
}