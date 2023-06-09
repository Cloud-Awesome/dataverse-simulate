using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class NextYearConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.NextYear;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var dataStore = new MockedEntityDataService();
        
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var nextYearStart = new DateTime(dataStore.SystemTime.Year + 1, 1, 1);
        var nextYearEnd = new DateTime(dataStore.SystemTime.Year + 1, 12, 31);
        
        return attributeValue.Date >= nextYearStart && attributeValue.Date < nextYearEnd;
    }
}