namespace QueryFramework.SqlServer.Extensions;

internal static class PagedSelectCommandBuilderExtensions
{
    internal static PagedSelectCommandBuilder Select(this PagedSelectCommandBuilder instance,
                                                     IPagedDatabaseEntityRetrieverSettings settings,
                                                     IQueryFieldInfo fieldInfo,
                                                     IFieldSelectionQuery? fieldSelectionQuery,
                                                     ISqlExpressionEvaluator evaluator,
                                                     ParameterBag parameterBag)
        => fieldSelectionQuery == null || fieldSelectionQuery.GetAllFields
            ? instance.AppendSelectFieldsForAllFields(settings, fieldInfo)
            : instance.AppendSelectFieldsForSpecifiedFields(fieldSelectionQuery, fieldInfo, evaluator, parameterBag);

    private static PagedSelectCommandBuilder AppendSelectFieldsForAllFields(this PagedSelectCommandBuilder instance,
                                                                            IPagedDatabaseEntityRetrieverSettings settings,
                                                                            IQueryFieldInfo fieldInfo)
    {
        var allFields = fieldInfo.GetAllFields();
        return allFields.Any()
            ? instance.Select(string.Join(", ", allFields.Select(x => fieldInfo.GetDatabaseFieldName(x)).OfType<string>()))
            : instance.Select(settings.Fields.WhenNullOrWhitespace("*"));
    }

    private static PagedSelectCommandBuilder AppendSelectFieldsForSpecifiedFields(this PagedSelectCommandBuilder instance,
                                                                                  IFieldSelectionQuery fieldSelectionQuery,
                                                                                  IQueryFieldInfo fieldInfo,
                                                                                  ISqlExpressionEvaluator evaluator,
                                                                                  ParameterBag parameterBag)
    {
        foreach (var expression in fieldSelectionQuery.Fields.Select((x, index) => new { Item = x, Index = index }))
        {
            if (expression.Index > 0)
            {
                instance.Select(", ");
            }

            instance.Select(evaluator.GetSqlExpression(expression.Item, fieldInfo, parameterBag));
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder Distinct(this PagedSelectCommandBuilder instance,
                                                       IFieldSelectionQuery? fieldSelectionQuery)
        => instance.DistinctValues(fieldSelectionQuery?.Distinct == true);

    internal static PagedSelectCommandBuilder Top(this PagedSelectCommandBuilder instance,
                                                  ISingleEntityQuery query,
                                                  IPagedDatabaseEntityRetrieverSettings settings)
    {
        var limit = query.Limit.IfNotGreaterThan(settings.OverridePageSize);

        return limit > 0
            ? instance.WithTop(limit)
            : instance;
    }

    internal static PagedSelectCommandBuilder Offset(this PagedSelectCommandBuilder instance,
                                                     ISingleEntityQuery query)
        => query.Offset.GetValueOrDefault() > 0
            ? instance.Skip(query.Offset.GetValueOrDefault())
            : instance;

    internal static PagedSelectCommandBuilder From(this PagedSelectCommandBuilder instance,
                                                   ISingleEntityQuery query,
                                                   IPagedDatabaseEntityRetrieverSettings settings)
        => instance.From(query.GetTableName(settings.TableName));

    internal static PagedSelectCommandBuilder Where(this PagedSelectCommandBuilder instance,
                                                    ISingleEntityQuery query,
                                                    IPagedDatabaseEntityRetrieverSettings settings,
                                                    IQueryFieldInfo fieldInfo,
                                                    ISqlExpressionEvaluator evaluator,
                                                    ParameterBag parameterBag)
    {
        if (!query.Conditions.Any() && string.IsNullOrEmpty(settings.DefaultWhere))
        {
            return instance;
        }

        if (!string.IsNullOrEmpty(settings.DefaultWhere))
        {
            instance.Where(settings.DefaultWhere);
        }

        foreach (var queryCondition in query.Conditions)
        {
            instance.AppendQueryCondition
            (
                queryCondition,
                fieldInfo,
                evaluator,
                parameterBag,
                queryCondition.Combination == Combination.And
                    ? instance.And
                    : instance.Or
            );
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder GroupBy(this PagedSelectCommandBuilder instance,
                                                      IGroupingQuery? groupingQuery,
                                                      IQueryFieldInfo fieldInfo,
                                                      ISqlExpressionEvaluator evaluator,
                                                      ParameterBag parameterBag)
    {
        if (groupingQuery == null || !groupingQuery.GroupByFields.Any())
        {
            return instance;
        }

        foreach (var groupBy in groupingQuery.GroupByFields.Select((x, index) => new { Item = x, Index = index }))
        {
            if (groupBy.Index > 0)
            {
                instance.GroupBy(", ");
            }

            instance.GroupBy(evaluator.GetSqlExpression(groupBy.Item, fieldInfo, parameterBag));
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder Having(this PagedSelectCommandBuilder instance,
                                                     IGroupingQuery? groupingQuery,
                                                     IQueryFieldInfo fieldInfo,
                                                     ISqlExpressionEvaluator evaluator,
                                                     ParameterBag parameterBag)
    {
        if (groupingQuery == null || !groupingQuery.HavingFields.Any())
        {
            return instance;
        }

        foreach (var having in groupingQuery.HavingFields.Select((x, index) => new { Item = x, Index = index }))
        {
            if (having.Index > 0)
            {
                instance.Having($" {having.Item.Combination.ToSql()} ");
            }
            instance.AppendQueryCondition
            (
                having.Item,
                fieldInfo,
                evaluator,
                parameterBag,
                instance.Having
            );
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder OrderBy(this PagedSelectCommandBuilder instance,
                                                      ISingleEntityQuery query,
                                                      IPagedDatabaseEntityRetrieverSettings settings,
                                                      IQueryFieldInfo fieldInfo,
                                                      ISqlExpressionEvaluator evaluator,
                                                      ParameterBag parameterBag)
    {
        if (query.Offset.HasValue && query.Offset.Value >= 0)
        {
            //do not use order by (this will be taken care of by the row_number function)
            return instance;
        }
        else if (query.OrderByFields.Any() || !string.IsNullOrEmpty(settings.DefaultOrderBy))
        {
            return instance.AppendOrderBy(query.OrderByFields, settings, fieldInfo, evaluator, parameterBag);
        }
        else
        {
            return instance;
        }
    }

    private static PagedSelectCommandBuilder AppendOrderBy(this PagedSelectCommandBuilder instance,
                                                           IEnumerable<IQuerySortOrder> orderByFields,
                                                           IPagedDatabaseEntityRetrieverSettings settings,
                                                           IQueryFieldInfo fieldInfo,
                                                           ISqlExpressionEvaluator evaluator,
                                                           ParameterBag parameterBag)
    {
        foreach (var querySortOrder in orderByFields.Select((x, index) => new { Item = x, Index = index }))
        {
            if (querySortOrder.Index > 0)
            {
                instance.OrderBy(", ");
            }

            instance.OrderBy($"{evaluator.GetSqlExpression(querySortOrder.Item.Field, fieldInfo, parameterBag)} {querySortOrder.Item.ToSql()}");
        }

        if (!orderByFields.Any() && !string.IsNullOrEmpty(settings.DefaultOrderBy))
        {
            instance.OrderBy(settings.DefaultOrderBy);
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder WithParameters(this PagedSelectCommandBuilder instance,
                                                             IParameterizedQuery? parameterizedQuery,
                                                             ParameterBag parameterBag)
    {
        if (parameterizedQuery != null)
        {
            foreach (var parameter in parameterizedQuery.Parameters)
            {
                instance.AppendParameter(parameter.Name, parameter.Value);
            }
        }

        foreach (var parameter in parameterBag.Parameters)
        {
            instance.AppendParameter(parameter.Key, parameter.Value!);
        }

        return instance; 
    }

    internal static PagedSelectCommandBuilder AppendQueryCondition(this PagedSelectCommandBuilder instance,
                                                                   ICondition condition,
                                                                   IQueryFieldInfo fieldInfo,
                                                                   ISqlExpressionEvaluator evaluator,
                                                                   ParameterBag parameterBag,
                                                                   Func<string, PagedSelectCommandBuilder> actionDelegate)
    {
        var builder = new StringBuilder();

        if (condition.StartGroup)
        {
            builder.Append("(");
        }

        if (!condition.Operator.In(Operator.Contains,
                                   Operator.NotContains,
                                   Operator.EndsWith,
                                   Operator.NotEndsWith,
                                   Operator.StartsWith,
                                   Operator.NotStartsWith))
        {
            builder.Append(condition.Operator.ToNot());

            if (condition.Operator.In(Operator.IsNullOrEmpty, Operator.IsNotNullOrEmpty))
            {
                builder.Append("COALESCE(");
            }
            else if (condition.Operator.In(Operator.IsNullOrWhiteSpace, Operator.IsNotNullOrWhiteSpace))
            {
                builder.Append("COALESCE(TRIM(");
            }

            builder.Append(evaluator.GetSqlExpression(condition.LeftExpression, fieldInfo, parameterBag));

            if (condition.Operator.In(Operator.IsNullOrEmpty, Operator.IsNotNullOrEmpty))
            {
                builder.Append(",'')");
            }
            else if (condition.Operator.In(Operator.IsNullOrWhiteSpace, Operator.IsNotNullOrWhiteSpace))
            {
                builder.Append("),'')");
            }
        }

        AppendOperatorAndValue(condition, fieldInfo, builder, evaluator, parameterBag);

        if (condition.EndGroup)
        {
            builder.Append(")");
        }

        actionDelegate.Invoke(builder.ToString());

        return instance;
    }

    private static void AppendOperatorAndValue(ICondition condition,
                                               IQueryFieldInfo fieldInfo,
                                               StringBuilder builder,
                                               ISqlExpressionEvaluator evaluator,
                                               ParameterBag parameterBag)
    {
        var leftExpressionSql = new Func<string>(() => evaluator.GetSqlExpression(condition.LeftExpression, fieldInfo, parameterBag));
        var rightExpressionSql = new Func<string>(() => evaluator.GetSqlExpression(condition.RightExpression, fieldInfo, parameterBag));
        var length = new Func<string>(() => evaluator.GetLengthExpression(condition.RightExpression, fieldInfo));

        var sqlToAppend = condition.Operator switch
        {
            Operator.IsNull => " IS NULL",
            Operator.IsNotNull => " IS NOT NULL",
            Operator.IsNullOrEmpty => " = ''",
            Operator.IsNotNullOrEmpty => " <> ''",
            Operator.IsNullOrWhiteSpace => " = ''",
            Operator.IsNotNullOrWhiteSpace => " <> ''",
            Operator.Contains => $"CHARINDEX({rightExpressionSql()}, {leftExpressionSql()}) > 0",
            Operator.NotContains => $"CHARINDEX({rightExpressionSql()}, {leftExpressionSql()}) = 0",
            Operator.StartsWith => $"LEFT({leftExpressionSql()}, {length()}) = {rightExpressionSql()}",
            Operator.NotStartsWith => $"LEFT({leftExpressionSql()}, {length()}) <> {rightExpressionSql()}",
            Operator.EndsWith => $"RIGHT({leftExpressionSql()}, {length()}) = {rightExpressionSql()}",
            Operator.NotEndsWith => $"RIGHT({leftExpressionSql()}, {length()}) <> {rightExpressionSql()}",
            _ => $" {condition.Operator.ToSql()} {rightExpressionSql()}"
        };

        builder.Append(sqlToAppend);
    }
}
