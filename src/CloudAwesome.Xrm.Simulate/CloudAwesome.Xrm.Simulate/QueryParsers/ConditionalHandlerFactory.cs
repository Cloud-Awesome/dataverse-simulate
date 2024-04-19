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
            { ConditionOperator.LastXMonths, new LastXMonthsConditionHandler() },
            { ConditionOperator.LastXWeeks, new LastXWeeksConditionHandler() },
            { ConditionOperator.LastXYears, new LastXYearsConditionHandler() },
            { ConditionOperator.LastYear, new LastYearConditionHandler() },
            { ConditionOperator.LessEqual, new LessEqualConditionHandler() },
            { ConditionOperator.LessThan, new LessThanConditionHandler() },
            { ConditionOperator.Like, new LikeConditionHandler() },
            { ConditionOperator.NextWeek, new NextWeekConditionHandler() },
            { ConditionOperator.NextXDays, new NextXDaysConditionHandler() },
            { ConditionOperator.NextXHours, new NextXHoursConditionHandler() },
            { ConditionOperator.NextXMonths, new NextXMonthsConditionHandler() },
            { ConditionOperator.NextXWeeks, new NextXWeeksConditionHandler() },
            { ConditionOperator.NextXYears, new NextXYearsConditionHandler() },
            { ConditionOperator.NextYear, new NextYearConditionHandler() },
            { ConditionOperator.NotEqual, new NotEqualConditionHandler() },
            { ConditionOperator.NotLike, new NotLikeConditionHandler() },
            { ConditionOperator.NotNull, new NotNullConditionHandler() },
            { ConditionOperator.NotOn, new NotOnConditionHandler() },
            { ConditionOperator.Null, new NullConditionHandler() },
            { ConditionOperator.On, new OnConditionHandler() },
            { ConditionOperator.OnOrAfter, new OnOrAfterConditionHandler() },
            { ConditionOperator.OnOrBefore, new OnOrBeforeConditionHandler() },
            { ConditionOperator.ThisMonth, new ThisMonthConditionHandler() },
            { ConditionOperator.ThisWeek, new ThisWeekConditionHandler() },
            { ConditionOperator.ThisYear, new ThisYearConditionHandler() },
            { ConditionOperator.Today, new TodayConditionHandler() },
            { ConditionOperator.Tomorrow, new TomorrowConditionHandler() },
            { ConditionOperator.Yesterday, new YesterdayConditionHandler() },
        };
    }
}

/*
 * Still to do...
 * --------------
 *
 * older than ... *6
 *
 * next 7 days
 * between
 * not between
 *
 * equal User Id
 * not equal user id
 *
 * --------------
 * 
 * containsValues
 * doesNotContainValues
 * in
 * mask
 *
 * --------------
 * 
 * Team, User, BusinessUnit conditions
 *
 * --------------
 * 
 * FiscalPeriod conditions
 * 
 */