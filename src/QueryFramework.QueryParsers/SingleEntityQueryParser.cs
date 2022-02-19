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

        builder.Conditions = PerformQuerySearch(items) ?? PerformSimpleSearch(items);

        return builder;
    }

    private List<IConditionBuilder>? PerformQuerySearch(string[] items)
    {
        var itemCountIsCorrect = (items.Length - 3) % 4 == 0;
        if (!itemCountIsCorrect)
        {
            return default;
        }
        var result = new List<IConditionBuilder>();
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
            if (queryOperator == null)
            {
                return default;
            }

            var condition = new ConditionBuilder
            {
                LeftExpression = GetField(fieldName),
                Operator = queryOperator.Value,
                RightExpression = new ConstantExpressionBuilder().WithValue(GetValue(queryOperator.Value, fieldValue))
            };

            result.Add(condition);
        }

        return result;
    }

    private IExpressionBuilder GetField(string fieldName)
        => _defaultFieldExpressionBuilderFactory == null
            ? new TQueryExpressionBuilder().WithFieldName(fieldName)
            : _defaultFieldExpressionBuilderFactory.Invoke().WithFieldName(fieldName);

    private object? GetValue(Operator queryOperator, object fieldValue)
        => queryOperator == Operator.IsNull || queryOperator == Operator.IsNotNull
            ? null
            : fieldValue;

    private List<IConditionBuilder> PerformSimpleSearch(string[] items)
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

    private IConditionBuilder CreateQueryCondition(string value, bool startsWithPlusOrMinus, bool startsWithMinus)
        => new ConditionBuilder
        {
            LeftExpression = _defaultFieldExpressionBuilderFactory == null
                ? new TQueryExpressionBuilder()
                : _defaultFieldExpressionBuilderFactory.Invoke(),
            RightExpression = new ConstantExpressionBuilder().WithValue(startsWithPlusOrMinus
                ? value.Substring(1)
                : value),
            Operator = startsWithMinus
                ? Operator.NotContains
                : Operator.Contains
        };

    private static Operator? GetQueryOperator(string @operator)
        => @operator.ToUpper(CultureInfo.InvariantCulture) switch
        {
            var x when
                    x == "=" ||
                    x == "==" => Operator.Equal,
            var x when
                    x == "<>" ||
                    x == "!=" ||
                    x == "#" => Operator.NotEqual,
            "<" => Operator.Smaller,
            ">" => Operator.Greater,
            "<=" => Operator.SmallerOrEqual,
            ">=" => Operator.GreaterOrEqual,
            "CONTAINS" => Operator.Contains,
            var x when
                    x == "NOTCONTAINS" ||
                    x == "NOT CONTAINS" => Operator.NotContains,
            "IS" => Operator.IsNull,
            var x when
                    x == "ISNOT" ||
                    x == "IS NOT" => Operator.IsNotNull,
            var x when
                x == "STARTS WITH" ||
                x == "STARTSWITH" => Operator.StartsWith,
            var x when
                    x == "ENDS WITH" ||
                    x == "ENDSWITH" => Operator.EndsWith,
            var x when
                x == "NOT STARTS WITH" ||
                x == "NOTSTARTSWITH" => Operator.NotStartsWith,
            var x when
                x == "NOT ENDS WITH" ||
                x == "NOTENDSWITH" => Operator.NotEndsWith,
            _ => null // Unknown operator
        };
}
