using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class ConditionHandlerFactory
{
    private static readonly Dictionary<ConditionOperator, IConditionHandler> Handlers;

    static ConditionHandlerFactory()
    {
        Handlers = new Dictionary<ConditionOperator, IConditionHandler>
        {
            { ConditionOperator.Equal, new EqualConditionHandler() },
            { ConditionOperator.NotEqual, new NotEqualConditionHandler() },
                
            // ... Add other condition handler instances as needed
        };
    }

    public static IConditionHandler GetHandler(ConditionOperator conditionOperator)
    {
        if (Handlers.TryGetValue(conditionOperator, out var handler))
        {
            return handler;
        }

        throw new NotImplementedException($"Condition operator {conditionOperator} not implemented.");
    }
}