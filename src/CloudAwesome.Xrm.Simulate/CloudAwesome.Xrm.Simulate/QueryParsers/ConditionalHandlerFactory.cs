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
        // TODO - Mostly untested/in progress (excluding Equal / NotEqual)
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
            { ConditionOperator.LessEqual, new LessEqualConditionHandler() },
            { ConditionOperator.LessThan, new LessThanConditionHandler() },
            { ConditionOperator.NotEqual, new NotEqualConditionHandler() },
            { ConditionOperator.NotNull, new NotNullConditionHandler() },
            { ConditionOperator.Null, new NullConditionHandler() },
                
            // ... Add other condition handler instances as needed
        };
    }
}