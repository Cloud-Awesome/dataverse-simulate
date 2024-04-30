namespace CloudAwesome.Xrm.Simulate.QueryParsers;

internal static class ConditionExpressionParser
{
    internal static int ParseIntCondition(object value)
    {
        int parsedCondition;
        if (value is string)
        {
            parsedCondition = int.Parse(value.ToString() ?? string.Empty);
        }
        else
        {
            parsedCondition = (int) value;
        }

        return parsedCondition;
    }
}