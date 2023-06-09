using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class LastYearConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.LastYear;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var dataStore = new MockedEntityDataService();
        
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var lastYearStart = new DateTime(dataStore.SystemTime.Year - 1, 1, 1);
        var lastYearEnd = new DateTime(dataStore.SystemTime.Year - 1, 12, 31);
        
        return attributeValue.Date >= lastYearStart && attributeValue.Date < lastYearEnd;
    }
}