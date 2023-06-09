﻿using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class LastWeekConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.LastWeek;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var dataStore = new MockedEntityDataService();
        
        var attributeValue = entity.GetAttributeValue<DateTime>(condition.AttributeName);
        var lastWeekStart = 
            dataStore.SystemTime.Date
                .AddDays(-(int)dataStore.SystemTime.Date.DayOfWeek)
                .AddDays(-7);
        var lastWeekEnd = lastWeekStart.AddDays(7);
        
        return attributeValue.Date >= lastWeekStart && attributeValue.Date < lastWeekEnd;
    }
}