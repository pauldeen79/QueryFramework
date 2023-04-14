namespace QueryFramework.QueryParsers;

public class SingleEntityQueryParser<TQueryBuilder, TQueryExpressionBuilder> : IQueryParser<TQueryBuilder>
    where TQueryBuilder : ISingleEntityQueryBuilder
    where TQueryExpressionBuilder : FieldExpressionBuilder, new()
{
    private readonly Func<TQueryExpressionBuilder>? _defaultFieldExpressionBuilderFactory;

    public SingleEntityQueryParser(Func<TQueryExpressionBuilder>? defaultFieldExpressionBuilderFactory)
        => _defaultFieldExpressionBuilderFactory = defaultFieldExpressionBuilderFactory;

    public TQueryBuilder Parse(TQueryBuilder builder, string queryString)
    {
        var items = queryString
            .Replace("\r\n", " ")
            .Replace("\n", " ")
            .Replace("\t", " ")
            .SafeSplit(' ', '\"', '\"');

        builder.Filter = new ComposedEvaluatableBuilder().AddConditions(PerformQuerySearch(items) ?? PerformSimpleSearch(items));

        return builder;
    }

    private List<ComposableEvaluatableBuilder>? PerformQuerySearch(string[] items)
    {
        var itemCountIsCorrect = (items.Length - 3) % 4 == 0;
        if (!itemCountIsCorrect)
        {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            return default;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
        }
        var result = new List<ComposableEvaluatableBuilder>();
        for (int i = 0; i < items.Length && itemCountIsCorrect; i += 4)
        {
            //verify that:
            //-items[i] needs to be a valid fieldname
            //-items[i + 1] needs to be a valid operator
            //-items[i + 3] needs to be a valid combination for the next condition
            var fieldName = items[i];
            var fieldValue = items[i + 2];
            var @operator = items[i + 1];

            //remove brackets and set bracket property values for this query item.
            if (fieldName.StartsWith("("))
            {
                fieldName = fieldName.Substring(1);
            }
            if (fieldValue.EndsWith(")"))
            {
                fieldValue = fieldValue.Substring(0, fieldValue.Length - 1);
            }

            var queryOperator = GetQueryOperator(@operator);
            if (queryOperator is null)
            {
                return default;
            }

            var condition = new ComposableEvaluatableBuilder
            {
                LeftExpression = GetField(fieldName),
                Operator = queryOperator,
                RightExpression = new ConstantExpressionBuilder().WithValue(GetValue(queryOperator, fieldValue))
            };

            result.Add(condition);
        }

        return result;
    }

    private FieldExpressionBuilder GetField(string fieldName)
        => _defaultFieldExpressionBuilderFactory is null
            ? new TQueryExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName)
            : _defaultFieldExpressionBuilderFactory.Invoke().WithFieldName(fieldName);

    private object? GetValue(OperatorBuilder queryOperator, object fieldValue)
        => queryOperator is IsNullOperatorBuilder || queryOperator is IsNotNullOperatorBuilder
            ? null
            : fieldValue;

    private List<ComposableEvaluatableBuilder> PerformSimpleSearch(string[] items)
        => items
            .Where(x => !string.IsNullOrEmpty(x))
            .Select((x, i) => new
            {
                Index = i,
                Value = x,
                StartsWithPlusOrMinus = x.StartsWith("+") || x.StartsWith("-"),
                StartsWithMinus = x.StartsWith("-")
            })
            .Select(item => CreateQueryCondition(item.Value,
                                                 item.StartsWithPlusOrMinus,
                                                 item.StartsWithMinus))
            .ToList();

    private ComposableEvaluatableBuilder CreateQueryCondition(string value, bool startsWithPlusOrMinus, bool startsWithMinus)
        => new ComposableEvaluatableBuilder
        {
            LeftExpression = _defaultFieldExpressionBuilderFactory is null
                ? new TQueryExpressionBuilder()
                : _defaultFieldExpressionBuilderFactory.Invoke(),
            RightExpression = new ConstantExpressionBuilder().WithValue(startsWithPlusOrMinus
                ? value.Substring(1)
                : value),
            Operator = startsWithMinus
                ? new StringNotContainsOperatorBuilder()
                : new StringContainsOperatorBuilder()
        };

    private static OperatorBuilder? GetQueryOperator(string @operator)
        => @operator.ToUpper(CultureInfo.InvariantCulture) switch
        {
            var x when
                    x == "=" ||
                    x == "==" => new EqualsOperatorBuilder(),
            var x when
                    x == "<>" ||
                    x == "!=" ||
                    x == "#" => new NotEqualsOperatorBuilder(),
            "<" => new IsSmallerOperatorBuilder(),
            ">" => new IsGreaterOperatorBuilder(),
            "<=" => new IsSmallerOrEqualOperatorBuilder(),
            ">=" => new IsGreaterOrEqualOperatorBuilder(),
            "CONTAINS" => new StringContainsOperatorBuilder(),
            var x when
                    x == "NOTCONTAINS" ||
                    x == "NOT CONTAINS" => new StringNotContainsOperatorBuilder(),
            "IS" => new IsNullOperatorBuilder(),
            var x when
                    x == "ISNOT" ||
                    x == "IS NOT" => new IsNotNullOperatorBuilder(),
            var x when
                x == "STARTS WITH" ||
                x == "STARTSWITH" => new StartsWithOperatorBuilder(),
            var x when
                    x == "ENDS WITH" ||
                    x == "ENDSWITH" => new EndsWithOperatorBuilder(),
            var x when
                x == "NOT STARTS WITH" ||
                x == "NOTSTARTSWITH" => new NotStartsWithOperatorBuilder(),
            var x when
                x == "NOT ENDS WITH" ||
                x == "NOTENDSWITH" => new NotEndsWithOperatorBuilder(),
            _ => null // Unknown operator
        };
}
