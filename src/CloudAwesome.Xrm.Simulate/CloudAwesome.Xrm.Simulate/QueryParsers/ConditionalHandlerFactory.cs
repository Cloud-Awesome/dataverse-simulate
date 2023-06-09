using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class ConditionHandlerFactory
{
    private static readonly Dictionary<ConditionOperator, IConditionHandler> Handlers;

    public static IConditionHandler GetHandler(ConditionOperator conditionOperator)
    {
        if (Handlers.TryGetValue(conditionOperator, out var handler))
        {
            return handler;
        }

        throw new NotImplementedException($"Condition operator {conditionOperator} not implemented.");
    }
    
    static ConditionHandlerFactory()
    {
        Handlers = new Dictionary<ConditionOperator, IConditionHandler>
        {
            { ConditionOperator.BeginsWith, new BeginsWithConditionHandler() },
            { ConditionOperator.Contains, new ContainsConditionHandler() },
            { ConditionOperator.DoesNotBeginWith, new DoesNotBeginWithConditionHandler() },
            { ConditionOperator.DoesNotContain, new DoesNotContainConditionHandler() },
            { ConditionOperator.DoesNotEndWith, new DoesNotEndWithConditionHandler() },
            { ConditionOperator.EndsWith, new EndsWithConditionHandler() },
            { ConditionOperator.Equal, new EqualConditionHandler() },
            { ConditionOperator.GreaterEqual, new GreaterEqualConditionHandler() },
            { ConditionOperator.GreaterThan, new GreaterThanConditionHandler() },
            { ConditionOperator.Last7Days, new Last7DaysConditionHandler() },
            { ConditionOperator.LastWeek, new LastWeekConditionHandler() },
            { ConditionOperator.LastXDays, new LastXDaysConditionHandler() },
            { ConditionOperator.LastXHours, new LastXHoursConditionHandler() },
            { ConditionOperator.LessEqual, new LessEqualConditionHandler() },
            { ConditionOperator.LessThan, new LessThanConditionHandler() },
            { ConditionOperator.Like, new LikeConditionHandler() },
            { ConditionOperator.NotEqual, new NotEqualConditionHandler() },
            { ConditionOperator.NotLike, new NotLikeConditionHandler() },
            { ConditionOperator.NotNull, new NotNullConditionHandler() },
            { ConditionOperator.NotOn, new NotOnConditionHandler() },
            { ConditionOperator.Null, new NullConditionHandler() },
            { ConditionOperator.On, new OnConditionHandler() },
            { ConditionOperator.OnOrAfter, new OnOrAfterConditionHandler() },
            { ConditionOperator.OnOrBefore, new OnOrBeforeConditionHandler() },
            { ConditionOperator.Today, new TodayConditionHandler() },
            { ConditionOperator.Tomorrow, new TomorrowConditionHandler() },
            { ConditionOperator.Yesterday, new YesterdayConditionHandler() },
            { ConditionOperator.LastYear, new LastYearConditionHandler() },
            
            
            // ... Add other condition handler instances as needed
        };
    }
}