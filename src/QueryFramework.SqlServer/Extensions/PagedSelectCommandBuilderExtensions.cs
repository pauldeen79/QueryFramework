namespace QueryFramework.SqlServer.Extensions;

internal static class PagedSelectCommandBuilderExtensions
{
    internal static PagedSelectCommandBuilder Select(this PagedSelectCommandBuilder instance,
                                                     IPagedDatabaseEntityRetrieverSettings settings,
                                                     IQueryFieldInfo fieldInfo,
                                                     IFieldSelectionQuery? fieldSelectionQuery,
                                                     ISqlExpressionEvaluator evaluator,
                                                     ParameterBag parameterBag,
                                                     object? context)
        => fieldSelectionQuery is null || fieldSelectionQuery.GetAllFields
            ? instance.AppendSelectFieldsForAllFields(settings, fieldInfo)
            : instance.AppendSelectFieldsForSpecifiedFields(fieldSelectionQuery, fieldInfo, evaluator, parameterBag, context);

    private static PagedSelectCommandBuilder AppendSelectFieldsForAllFields(this PagedSelectCommandBuilder instance,
                                                                            IPagedDatabaseEntityRetrieverSettings settings,
                                                                            IQueryFieldInfo fieldInfo)
    {
        var allFields = fieldInfo.GetAllFields();
        return allFields.Any()
            ? instance.Select(string.Join(", ", allFields.Select(fieldInfo.GetDatabaseFieldName).Where(x => !string.IsNullOrEmpty(x))))
            : instance.Select(settings.Fields.WhenNullOrWhitespace("*"));
    }

    private static PagedSelectCommandBuilder AppendSelectFieldsForSpecifiedFields(this PagedSelectCommandBuilder instance,
                                                                                  IFieldSelectionQuery fieldSelectionQuery,
                                                                                  IQueryFieldInfo fieldInfo,
                                                                                  ISqlExpressionEvaluator evaluator,
                                                                                  ParameterBag parameterBag,
                                                                                  object? context)
    {
        foreach (var expression in fieldSelectionQuery.FieldNames.Select((x, index) => new { Item = x, Index = index }))
        {
            if (expression.Index > 0)
            {
                instance.Select(", ");
            }

            instance.Select(evaluator.GetSqlExpression(new FieldExpression(new ContextExpression(), new TypedConstantExpression<string>(expression.Item)), fieldInfo, parameterBag, context));
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder Distinct(this PagedSelectCommandBuilder instance,
                                                       IFieldSelectionQuery? fieldSelectionQuery)
        => instance.DistinctValues(fieldSelectionQuery?.Distinct == true);

    internal static PagedSelectCommandBuilder Top(this PagedSelectCommandBuilder instance,
                                                  IQuery query,
                                                  IPagedDatabaseEntityRetrieverSettings settings)
    {
        var limit = query.Limit.IfNotGreaterThan(settings.OverridePageSize);

        return limit > 0
            ? instance.WithTop(limit)
            : instance;
    }

    internal static PagedSelectCommandBuilder Offset(this PagedSelectCommandBuilder instance,
                                                     IQuery query)
        => query.Offset.GetValueOrDefault() > 0
            ? instance.Skip(query.Offset.GetValueOrDefault())
            : instance;

    internal static PagedSelectCommandBuilder From(this PagedSelectCommandBuilder instance,
                                                   IQuery query,
                                                   IPagedDatabaseEntityRetrieverSettings settings)
        => instance.From(query.GetTableName(settings.TableName));

    internal static PagedSelectCommandBuilder Where(this PagedSelectCommandBuilder instance,
                                                    IQuery query,
                                                    IPagedDatabaseEntityRetrieverSettings settings,
                                                    IQueryFieldInfo fieldInfo,
                                                    ISqlExpressionEvaluator evaluator,
                                                    ParameterBag parameterBag,
                                                    object? context)
    {
        if (!query.Filter.Conditions.Any() && string.IsNullOrEmpty(settings.DefaultWhere))
        {
            return instance;
        }

        if (!string.IsNullOrEmpty(settings.DefaultWhere))
        {
            instance.Where(settings.DefaultWhere);
        }

        foreach (var queryCondition in query.Filter.Conditions)
        {
            instance.AppendQueryCondition
            (
                queryCondition,
                fieldInfo,
                evaluator,
                parameterBag,
                context,
                (queryCondition.Combination ?? Combination.And) == Combination.And
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
                                                      ParameterBag parameterBag,
                                                      object? context)
    {
        if (groupingQuery is null || !groupingQuery.GroupByFields.Any())
        {
            return instance;
        }

        foreach (var groupBy in groupingQuery.GroupByFields.Select((x, index) => new { Item = x, Index = index }))
        {
            if (groupBy.Index > 0)
            {
                instance.GroupBy(", ");
            }

            instance.GroupBy(evaluator.GetSqlExpression(groupBy.Item, fieldInfo, parameterBag, context));
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder Having(this PagedSelectCommandBuilder instance,
                                                     IGroupingQuery? groupingQuery,
                                                     IQueryFieldInfo fieldInfo,
                                                     ISqlExpressionEvaluator evaluator,
                                                     ParameterBag parameterBag,
                                                     object? context)
    {
        if (groupingQuery is null || !groupingQuery.GroupByFilter.Conditions.Any())
        {
            return instance;
        }

        foreach (var having in groupingQuery.GroupByFilter.Conditions.Select((x, index) => new { Item = x, Index = index }))
        {
            if (having.Index > 0)
            {
                instance.Having($" {(having.Item.Combination ?? Combination.And).ToSql()} ");
            }
            instance.AppendQueryCondition
            (
                having.Item,
                fieldInfo,
                evaluator,
                parameterBag,
                context,
                instance.Having
            );
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder OrderBy(this PagedSelectCommandBuilder instance,
                                                      IQuery query,
                                                      IPagedDatabaseEntityRetrieverSettings settings,
                                                      IQueryFieldInfo fieldInfo,
                                                      ISqlExpressionEvaluator evaluator,
                                                      ParameterBag parameterBag,
                                                      object? context)
    {
        if (query.Offset.HasValue && query.Offset.Value >= 0)
        {
            //do not use order by (this will be taken care of by the row_number Expression)
            return instance;
        }
        else if (query.OrderByFields.Any() || !string.IsNullOrEmpty(settings.DefaultOrderBy))
        {
            return instance.AppendOrderBy(query.OrderByFields, settings, fieldInfo, evaluator, parameterBag, context);
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
                                                           ParameterBag parameterBag,
                                                           object? context)
    {
        foreach (var querySortOrder in orderByFields.Select((x, index) => new { Item = x, Index = index }))
        {
            if (querySortOrder.Index > 0)
            {
                instance.OrderBy(", ");
            }

            instance.OrderBy($"{evaluator.GetSqlExpression(querySortOrder.Item.FieldNameExpression, fieldInfo, parameterBag, context)} {querySortOrder.Item.ToSql()}");
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
        if (parameterizedQuery is not null)
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
                                                                   ComposableEvaluatable condition,
                                                                   IQueryFieldInfo fieldInfo,
                                                                   ISqlExpressionEvaluator evaluator,
                                                                   ParameterBag parameterBag,
                                                                   object? context,
                                                                   Func<string, PagedSelectCommandBuilder> actionDelegate)
    {
        var builder = new StringBuilder();

        if (condition.StartGroup)
        {
            builder.Append("(");
        }

        if (!condition.Operator.GetType().In(typeof(StringContainsOperator),
                                             typeof(StringNotContainsOperator),
                                             typeof(EndsWithOperator),
                                             typeof(NotEndsWithOperator),
                                             typeof(StartsWithOperator),
                                             typeof(NotStartsWithOperator)))
        {
            builder.Append(condition.Operator.ToNot());

            if (condition.Operator.GetType().In(typeof(IsNullOrEmptyOperator), typeof(IsNotNullOrEmptyOperator)))
            {
                builder.Append("COALESCE(");
            }
            else if (condition.Operator.GetType().In(typeof(IsNullOrWhiteSpaceOperator), typeof(IsNotNullOrWhiteSpaceOperator)))
            {
                builder.Append("COALESCE(TRIM(");
            }

            builder.Append(evaluator.GetSqlExpression(condition.LeftExpression, fieldInfo, parameterBag, context));

            if (condition.Operator.GetType().In(typeof(IsNullOrEmptyOperator), typeof(IsNotNullOrEmptyOperator)))
            {
                builder.Append(",'')");
            }
            else if (condition.Operator.GetType().In(typeof(IsNullOrWhiteSpaceOperator), typeof(IsNotNullOrWhiteSpaceOperator)))
            {
                builder.Append("),'')");
            }
        }

        AppendOperatorAndValue(condition, fieldInfo, builder, evaluator, parameterBag, context);

        if (condition.EndGroup)
        {
            builder.Append(")");
        }

        actionDelegate.Invoke(builder.ToString());

        return instance;
    }

    private static void AppendOperatorAndValue(ComposableEvaluatable condition,
                                               IQueryFieldInfo fieldInfo,
                                               StringBuilder builder,
                                               ISqlExpressionEvaluator evaluator,
                                               ParameterBag parameterBag,
                                               object? context)
    {
        var leftExpressionSql = new Func<string>(() => evaluator.GetSqlExpression(condition.LeftExpression, fieldInfo, parameterBag, context));
        var rightExpressionSql = new Func<string>(() => evaluator.GetSqlExpression(condition.RightExpression, fieldInfo, parameterBag, context));
        var length = new Func<string>(() => evaluator.GetLengthExpression(condition.RightExpression, fieldInfo, context));

        var sqlToAppend = condition.Operator switch
        {
            IsNullOperator => " IS NULL",
            IsNotNullOperator => " IS NOT NULL",
            IsNullOrEmptyOperator => " = ''",
            IsNotNullOrEmptyOperator => " <> ''",
            IsNullOrWhiteSpaceOperator => " = ''",
            IsNotNullOrWhiteSpaceOperator => " <> ''",
            StringContainsOperator => $"CHARINDEX({rightExpressionSql()}, {leftExpressionSql()}) > 0",
            StringNotContainsOperator => $"CHARINDEX({rightExpressionSql()}, {leftExpressionSql()}) = 0",
            StartsWithOperator => $"LEFT({leftExpressionSql()}, {length()}) = {rightExpressionSql()}",
            NotStartsWithOperator => $"LEFT({leftExpressionSql()}, {length()}) <> {rightExpressionSql()}",
            EndsWithOperator => $"RIGHT({leftExpressionSql()}, {length()}) = {rightExpressionSql()}",
            NotEndsWithOperator => $"RIGHT({leftExpressionSql()}, {length()}) <> {rightExpressionSql()}",
            _ => $" {condition.Operator.ToSql()} {rightExpressionSql()}"
        };

        builder.Append(sqlToAppend);
    }
}
