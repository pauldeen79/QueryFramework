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
        var paramCounter = 0;
        foreach (var expression in fieldSelectionQuery.Fields)
        {
            if (paramCounter > 0)
            {
                instance.Select(", ");
            }

            var fieldName = expression.GetFieldName();
            if (fieldName == null)
            {
                throw new InvalidOperationException($"Query fields contains no field in expression [{expression}]");
            }
            var correctedFieldName = fieldInfo.GetDatabaseFieldName(fieldName);
            if (correctedFieldName == null)
            {
                throw new InvalidOperationException($"Query fields contains unknown field in expression [{expression}]");
            }

            //Note that for now, we assume that custom expressions don't override field name logic, only expression logic
            var correctedExpression = new FieldExpression(correctedFieldName, expression.Function);

            instance.Select(evaluator.GetSqlExpression(correctedExpression));
            paramCounter++;
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

            var fieldName = groupBy.GetFieldName();
            if (fieldName == null)
            {
                throw new InvalidOperationException($"Query group by fields contains no field in expression [{groupBy}]");
            }
            var correctedFieldName = fieldInfo.GetDatabaseFieldName(fieldName);
            if (correctedFieldName == null)
            {
                throw new InvalidOperationException($"Query group by fields contains unknown field [{fieldName}]");
            }
            var corrected = new FieldExpression(correctedFieldName, groupBy.Function);
            instance.GroupBy(evaluator.GetSqlExpression(corrected));
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

            var fieldName = querySortOrder.SortOrder.Field.GetFieldName();
            if (fieldName == null)
            {
                throw new InvalidOperationException($"Query order by fields contains no field in expression [{querySortOrder.SortOrder.Field}]");
            }
            var newFieldName = fieldInfo.GetDatabaseFieldName(fieldName);
            if (newFieldName == null)
            {
                throw new InvalidOperationException(string.Format("Query order by fields contains unknown field [{0}]", fieldName));
            }
            var newQuerySortOrder = new QuerySortOrder(new FieldExpression(newFieldName, null), querySortOrder.SortOrder.Order);
            instance.OrderBy($"{evaluator.GetSqlExpression(newQuerySortOrder.Field)} {newQuerySortOrder.ToSql()}");
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

        var fieldExpression = condition.LeftExpression;

        var leftFieldName = condition.LeftExpression.GetFieldName();
        var rightFieldName =  condition.RightExpression.GetFieldName();
        if (leftFieldName == null && rightFieldName == null)
        {
            //TODO: Write code for constant, and think of a way to prevent sql injection
            throw new NotImplementedException("Need to implement two constant values here");
        }
        if (leftFieldName != null && rightFieldName != null)
        {
            //TODO: Refactor code, so this scenario is also supported
            throw new NotSupportedException("At this moment, only one expression with a field name is supported for one condition");
        }
        var leftConstantValue = condition.LeftExpression.GetConstantValue();
        var rightConstantValue = condition.RightExpression.GetConstantValue();
        if (leftConstantValue != null && rightConstantValue != null)
        {
            //TODO: Refactor code, so this scenario is also supported
            throw new NotSupportedException("At this moment, only one expression with a constant value is supported for one condition");
        }
        var fieldName = leftFieldName ?? rightFieldName ?? string.Empty;
        var customFieldName = fieldInfo.GetDatabaseFieldName(fieldName);
        if (customFieldName == null)
        {
            throw new InvalidOperationException($"Query conditions contains unknown field [{fieldName}]");
        }

        fieldExpression = new FieldExpression(customFieldName, fieldExpression.Function);

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

            builder.Append(evaluator.GetSqlExpression(fieldExpression));

            if (condition.Operator.In(Operator.IsNullOrEmpty, Operator.IsNotNullOrEmpty))
            {
                builder.Append(",'')");
            }
            else if (condition.Operator.In(Operator.IsNullOrWhiteSpace, Operator.IsNotNullOrWhiteSpace))
            {
                builder.Append("),'')");
            }
        }

        var constantValue = leftConstantValue ?? rightConstantValue;
        var paramName = GetQueryParameterName(paramCounter, constantValue);

        AppendOperatorAndValue(instance, paramCounter, condition, fieldExpression, paramName, constantValue, builder, evaluator);

        actionDelegate.Invoke(builder.ToString());

        return paramCounter + 1;
    }

    private static void AppendOperatorAndValue(PagedSelectCommandBuilder instance,
                                               int paramCounter,
                                               ICondition condition,
                                               IExpression expression,
                                               string paramName,
                                               object? constantValue,
                                               StringBuilder builder,
                                               ISqlExpressionEvaluator evaluator)
    {
        var sqlToAppend = condition.Operator switch
        {
            Operator.IsNull => " IS NULL",
            Operator.IsNotNull => " IS NOT NULL",
            Operator.IsNullOrEmpty => " = ''",
            Operator.IsNotNullOrEmpty => " <> ''",
            Operator.IsNullOrWhiteSpace => " = ''",
            Operator.IsNotNullOrWhiteSpace => " <> ''",
            Operator.Contains => $"CHARINDEX({paramName}, {evaluator.GetSqlExpression(expression)}) > 0",
            Operator.NotContains => $"CHARINDEX({paramName}, {evaluator.GetSqlExpression(expression)}) = 0",
            Operator.StartsWith => $"LEFT({evaluator.GetSqlExpression(expression)}, {constantValue.ToStringWithNullCheck().Length}) = {paramName}",
            Operator.NotStartsWith => $"LEFT({evaluator.GetSqlExpression(expression)}, {constantValue.ToStringWithNullCheck().Length}) <> {paramName}",
            Operator.EndsWith => $"RIGHT({evaluator.GetSqlExpression(expression)}, {constantValue.ToStringWithNullCheck().Length}) = {paramName}",
            Operator.NotEndsWith => $"RIGHT({evaluator.GetSqlExpression(expression)}, {constantValue.ToStringWithNullCheck().Length}) <> {paramName}",
            _ => $" {condition.Operator.ToSql()} {paramName}"
        };

        builder.Append(sqlToAppend);

        if (!condition.Operator.In(Operator.IsNull,
                                   Operator.IsNotNull,
                                   Operator.IsNullOrEmpty,
                                   Operator.IsNotNullOrEmpty))
        {
            AppendParameterIfNecessary(instance, paramCounter, constantValue);
        }
    }

    private static void AppendParameterIfNecessary(PagedSelectCommandBuilder instance,
                                                   int paramCounter,
                                                   object? constantValue)
    {
        if (constantValue is IQueryParameterValue)
        {
            return;
        }

        instance.AppendParameter(string.Format("p{0}", paramCounter),
                                 constantValue is KeyValuePair<string, object> keyValuePair
                                     ? keyValuePair.Value
                                     : constantValue ?? new object());
    }

    private static string GetQueryParameterName(int paramCounter, object? value)
    {
        if (value is KeyValuePair<string, object> keyValuePair)
        {
            return $"@{keyValuePair.Key}";
        }

        if (value is IQueryParameterValue queryParameterValue)
        {
            return $"@{queryParameterValue.Name}";
        }

        return $"@p{paramCounter}";
    }
}
