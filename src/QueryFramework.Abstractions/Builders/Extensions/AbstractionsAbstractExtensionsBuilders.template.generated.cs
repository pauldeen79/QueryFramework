﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 8.0.5
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable
namespace QueryFramework.Abstractions.Builders.Extensions
{
    public static partial class DataObjectNameQueryBuilderExtensions
    {
        public static T WithDataObjectName<T>(this T instance, string dataObjectName)
            where T : QueryFramework.Abstractions.Builders.IDataObjectNameQueryBuilder
        {
            if (dataObjectName is null) throw new System.ArgumentNullException(nameof(dataObjectName));
            instance.DataObjectName = dataObjectName;
            return instance;
        }
    }
    public static partial class FieldSelectionQueryBuilderExtensions
    {
        public static T AddFieldNames<T>(this T instance, System.Collections.Generic.IEnumerable<string> fieldNames)
            where T : QueryFramework.Abstractions.Builders.IFieldSelectionQueryBuilder
        {
            if (fieldNames is null) throw new System.ArgumentNullException(nameof(fieldNames));
            return instance.AddFieldNames<T>(fieldNames.ToArray());
        }

        public static T AddFieldNames<T>(this T instance, params string[] fieldNames)
            where T : QueryFramework.Abstractions.Builders.IFieldSelectionQueryBuilder
        {
            if (fieldNames is null) throw new System.ArgumentNullException(nameof(fieldNames));
            foreach (var item in fieldNames) instance.FieldNames.Add(item);
            return instance;
        }

        public static T WithDistinct<T>(this T instance, bool distinct = true)
            where T : QueryFramework.Abstractions.Builders.IFieldSelectionQueryBuilder
        {
            instance.Distinct = distinct;
            return instance;
        }

        public static T WithGetAllFields<T>(this T instance, bool getAllFields = true)
            where T : QueryFramework.Abstractions.Builders.IFieldSelectionQueryBuilder
        {
            instance.GetAllFields = getAllFields;
            return instance;
        }
    }
    public static partial class GroupingQueryBuilderExtensions
    {
        public static T AddGroupByFields<T>(this T instance, System.Collections.Generic.IEnumerable<ExpressionFramework.Domain.Builders.ExpressionBuilder> groupByFields)
            where T : QueryFramework.Abstractions.Builders.IGroupingQueryBuilder
        {
            if (groupByFields is null) throw new System.ArgumentNullException(nameof(groupByFields));
            return instance.AddGroupByFields<T>(groupByFields.ToArray());
        }

        public static T AddGroupByFields<T>(this T instance, params ExpressionFramework.Domain.Builders.ExpressionBuilder[] groupByFields)
            where T : QueryFramework.Abstractions.Builders.IGroupingQueryBuilder
        {
            if (groupByFields is null) throw new System.ArgumentNullException(nameof(groupByFields));
            foreach (var item in groupByFields) instance.GroupByFields.Add(item);
            return instance;
        }

        public static T WithGroupByFilter<T>(this T instance, ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder groupByFilter)
            where T : QueryFramework.Abstractions.Builders.IGroupingQueryBuilder
        {
            if (groupByFilter is null) throw new System.ArgumentNullException(nameof(groupByFilter));
            instance.GroupByFilter = groupByFilter;
            return instance;
        }
    }
    public static partial class ParameterizedQueryBuilderExtensions
    {
        public static T AddParameters<T>(this T instance, System.Collections.Generic.IEnumerable<QueryFramework.Abstractions.Builders.IQueryParameterBuilder> parameters)
            where T : QueryFramework.Abstractions.Builders.IParameterizedQueryBuilder
        {
            if (parameters is null) throw new System.ArgumentNullException(nameof(parameters));
            return instance.AddParameters<T>(parameters.ToArray());
        }

        public static T AddParameters<T>(this T instance, params QueryFramework.Abstractions.Builders.IQueryParameterBuilder[] parameters)
            where T : QueryFramework.Abstractions.Builders.IParameterizedQueryBuilder
        {
            if (parameters is null) throw new System.ArgumentNullException(nameof(parameters));
            foreach (var item in parameters) instance.Parameters.Add(item);
            return instance;
        }
    }
    public static partial class QueryBuilderExtensions
    {
        public static T AddOrderByFields<T>(this T instance, System.Collections.Generic.IEnumerable<QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder> orderByFields)
            where T : QueryFramework.Abstractions.Builders.IQueryBuilder
        {
            if (orderByFields is null) throw new System.ArgumentNullException(nameof(orderByFields));
            return instance.AddOrderByFields<T>(orderByFields.ToArray());
        }

        public static T AddOrderByFields<T>(this T instance, params QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder[] orderByFields)
            where T : QueryFramework.Abstractions.Builders.IQueryBuilder
        {
            if (orderByFields is null) throw new System.ArgumentNullException(nameof(orderByFields));
            foreach (var item in orderByFields) instance.OrderByFields.Add(item);
            return instance;
        }

        public static T WithLimit<T>(this T instance, System.Nullable<int> limit)
            where T : QueryFramework.Abstractions.Builders.IQueryBuilder
        {
            instance.Limit = limit;
            return instance;
        }

        public static T WithOffset<T>(this T instance, System.Nullable<int> offset)
            where T : QueryFramework.Abstractions.Builders.IQueryBuilder
        {
            instance.Offset = offset;
            return instance;
        }

        public static T WithFilter<T>(this T instance, ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder filter)
            where T : QueryFramework.Abstractions.Builders.IQueryBuilder
        {
            if (filter is null) throw new System.ArgumentNullException(nameof(filter));
            instance.Filter = filter;
            return instance;
        }
    }
    public static partial class QueryParameterBuilderExtensions
    {
        public static T WithName<T>(this T instance, string name)
            where T : QueryFramework.Abstractions.Builders.IQueryParameterBuilder
        {
            if (name is null) throw new System.ArgumentNullException(nameof(name));
            instance.Name = name;
            return instance;
        }

        public static T WithValue<T>(this T instance, object? value)
            where T : QueryFramework.Abstractions.Builders.IQueryParameterBuilder
        {
            instance.Value = value;
            return instance;
        }
    }
    public static partial class QueryParameterValueBuilderExtensions
    {
        public static T WithName<T>(this T instance, string name)
            where T : QueryFramework.Abstractions.Builders.IQueryParameterValueBuilder
        {
            if (name is null) throw new System.ArgumentNullException(nameof(name));
            instance.Name = name;
            return instance;
        }
    }
    public static partial class QuerySortOrderBuilderExtensions
    {
        public static T WithFieldNameExpression<T>(this T instance, ExpressionFramework.Domain.Builders.ExpressionBuilder fieldNameExpression)
            where T : QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder
        {
            if (fieldNameExpression is null) throw new System.ArgumentNullException(nameof(fieldNameExpression));
            instance.FieldNameExpression = fieldNameExpression;
            return instance;
        }

        public static T WithOrder<T>(this T instance, QueryFramework.Abstractions.Domains.QuerySortOrderDirection order)
            where T : QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder
        {
            instance.Order = order;
            return instance;
        }
    }
    public static partial class SingleEntityQueryBuilderExtensions
    {
    }
}
#nullable disable
