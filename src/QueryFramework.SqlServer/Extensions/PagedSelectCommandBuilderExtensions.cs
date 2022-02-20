namespace QueryFramework.SqlServer.Extensions;

internal static class PagedSelectCommandBuilderExtensions
{
    internal static PagedSelectCommandBuilder Select(this PagedSelectCommandBuilder instance,
                                                     IPagedDatabaseEntityRetrieverSettings settings,
                                                     IQueryFieldInfo fieldInfo,
                                                     IFieldSelectionQuery? fieldSelectionQuery,
                                                     ISqlExpressionEvaluator evaluator)
        => fieldSelectionQuery == null || fieldSelectionQuery.GetAllFields
            ? instance.AppendSelectFieldsForAllFields(settings, fieldInfo)
            : instance.AppendSelectFieldsForSpecifiedFields(fieldSelectionQuery, fieldInfo, evaluator);

    private static PagedSelectCommandBuilder AppendSelectFieldsForAllFields(this PagedSelectCommandBuilder instance,
                                                                            IPagedDatabaseEntityRetrieverSettings settings,
                                                                            IQueryFieldInfo fieldInfo)
    {
        var allFields = fieldInfo.GetAllFields();
        if (!allFields.Any())
        {
            instance.Select(settings.Fields.WhenNullOrWhitespace("*"));
        }
        else
        {
            instance.Select(string.Join(", ", allFields.Select(x => fieldInfo.GetDatabaseFieldName(x)).OfType<string>()));
        }

        return instance;
    }

    private static PagedSelectCommandBuilder AppendSelectFieldsForSpecifiedFields(this PagedSelectCommandBuilder instance,
                                                                                  IFieldSelectionQuery fieldSelectionQuery,
                                                                                  IQueryFieldInfo fieldInfo,
                                                                                  ISqlExpressionEvaluator evaluator)
    {
        var counter = 0;
        foreach (var expression in fieldSelectionQuery.Fields)
        {
            if (counter > 0)
            {
                instance.Select(", ");
            }

            //TODO: Fix parameter counter
            instance.Select(evaluator.GetSqlExpression(expression, fieldInfo, -1));
            counter++;
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
                                                    out int paramCounter)
    {
        if (!query.Conditions.Any() && string.IsNullOrEmpty(settings.DefaultWhere))
        {
            paramCounter = 0;
            return instance;
        }

        paramCounter = 0;

        if (!string.IsNullOrEmpty(settings.DefaultWhere))
        {
            instance.Where(settings.DefaultWhere);
        }

        foreach (var queryCondition in query.Conditions)
        {
            paramCounter = instance.AppendQueryCondition
            (
                paramCounter,
                queryCondition,
                fieldInfo,
                evaluator,
                instance.Where
            );
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder GroupBy(this PagedSelectCommandBuilder instance,
                                                      IGroupingQuery? groupingQuery,
                                                      IQueryFieldInfo fieldInfo,
                                                      ISqlExpressionEvaluator evaluator)
    {
        if (groupingQuery == null || !groupingQuery.GroupByFields.Any())
        {
            return instance;
        }

        var fieldCounter = 0;
        foreach (var groupBy in groupingQuery.GroupByFields)
        {
            if (fieldCounter > 0)
            {
                instance.GroupBy(", ");
            }

            //TODO: Fix parameter counter
            instance.GroupBy(evaluator.GetSqlExpression(groupBy, fieldInfo, -1));
            fieldCounter++;
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder Having(this PagedSelectCommandBuilder instance,
                                                     IGroupingQuery? groupingQuery,
                                                     IQueryFieldInfo fieldInfo,
                                                     ISqlExpressionEvaluator evaluator,
                                                     ref int paramCounter)
    {
        if (groupingQuery == null || !groupingQuery.HavingFields.Any())
        {
            return instance;
        }

        var fieldCounter = 0;
        foreach (var having in groupingQuery.HavingFields)
        {
            if (fieldCounter > 0)
            {
                instance.Having($" AND ");
            }
            paramCounter = instance.AppendQueryCondition
            (
                paramCounter,
                having,
                fieldInfo,
                evaluator,
                instance.Having
            );
            fieldCounter++;
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder OrderBy(this PagedSelectCommandBuilder instance,
                                                      ISingleEntityQuery query,
                                                      IPagedDatabaseEntityRetrieverSettings settings,
                                                      IQueryFieldInfo fieldInfo,
                                                      ISqlExpressionEvaluator evaluator)
    {
        if (query.Offset.HasValue && query.Offset.Value >= 0)
        {
            //do not use order by (this will be taken care of by the row_number function)
            return instance;
        }
        else if (query.OrderByFields.Any() || !string.IsNullOrEmpty(settings.DefaultOrderBy))
        {
            return instance.AppendOrderBy(query.OrderByFields, settings, fieldInfo, evaluator);
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
                                                           ISqlExpressionEvaluator evaluator)
    {
        foreach (var querySortOrder in orderByFields.Select((field, index) => new { SortOrder = field, Index = index }))
        {
            if (querySortOrder.Index > 0)
            {
                instance.OrderBy(", ");
            }

            //TODO: Fix parameter counter
            instance.OrderBy($"{evaluator.GetSqlExpression(querySortOrder.SortOrder.Field, fieldInfo, -1)} {querySortOrder.SortOrder.ToSql()}");
        }

        if (!orderByFields.Any() && !string.IsNullOrEmpty(settings.DefaultOrderBy))
        {
            instance.OrderBy(settings.DefaultOrderBy);
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder WithParameters(this PagedSelectCommandBuilder instance,
                                                             IParameterizedQuery? parameterizedQuery)
        => parameterizedQuery == null
            ? instance
            : parameterizedQuery.Parameters.Aggregate(instance, (acc, parameter) => acc.AppendParameter(parameter.Name, parameter.Value));

    internal static int AppendQueryCondition(this PagedSelectCommandBuilder instance,
                                             int paramCounter,
                                             ICondition condition,
                                             IQueryFieldInfo fieldInfo,
                                             ISqlExpressionEvaluator evaluator,
                                             Func<string, PagedSelectCommandBuilder> actionDelegate)
    {
        var builder = new StringBuilder();

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

            builder.Append(evaluator.GetSqlExpression(condition.LeftExpression, fieldInfo, paramCounter));

            if (condition.Operator.In(Operator.IsNullOrEmpty, Operator.IsNotNullOrEmpty))
            {
                builder.Append(",'')");
            }
            else if (condition.Operator.In(Operator.IsNullOrWhiteSpace, Operator.IsNotNullOrWhiteSpace))
            {
                builder.Append("),'')");
            }
        }

        AppendOperatorAndValue(instance, paramCounter, condition, fieldInfo, builder, evaluator);

        actionDelegate.Invoke(builder.ToString());

        return paramCounter + 1;
    }

    private static void AppendOperatorAndValue(PagedSelectCommandBuilder instance,
                                               int paramCounter,
                                               ICondition condition,
                                               IQueryFieldInfo fieldInfo,
                                               StringBuilder builder,
                                               ISqlExpressionEvaluator evaluator)
    {
        var leftExpressionSql = new Func<string>(() => evaluator.GetSqlExpression(condition.LeftExpression, fieldInfo, paramCounter));
        var rightExpressionSql = new Func<string>(() => evaluator.GetSqlExpression(condition.RightExpression, fieldInfo, paramCounter));
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

        if (!condition.Operator.In(Operator.IsNull,
                                   Operator.IsNotNull,
                                   Operator.IsNullOrEmpty,
                                   Operator.IsNotNullOrEmpty))
        {
            AppendParameterIfNecessary(instance, paramCounter, condition);
        }
    }

    private static void AppendParameterIfNecessary(PagedSelectCommandBuilder instance,
                                                   int paramCounter,
                                                   ICondition condition)
    {
        var constantValue = condition.RightExpression.GetConstantValue();
        if (constantValue == null || constantValue is IQueryParameterValue)
        {
            return;
        }

        instance.AppendParameter(string.Format("p{0}", paramCounter),
                                 constantValue is KeyValuePair<string, object> keyValuePair
                                     ? keyValuePair.Value
                                     : constantValue);
    }
}
