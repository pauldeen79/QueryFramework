﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 8.0.10
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace QueryFramework.Abstractions
{
    public partial interface IDataObjectNameQuery : QueryFramework.Abstractions.IQuery
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        string DataObjectName
        {
            get;
        }
    }
    public partial interface IFieldSelectionQuery : QueryFramework.Abstractions.IQuery
    {
        bool Distinct
        {
            get;
        }

        bool GetAllFields
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        System.Collections.Generic.IReadOnlyCollection<string> FieldNames
        {
            get;
        }
    }
    public partial interface IGroupingQuery : QueryFramework.Abstractions.IQuery
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        System.Collections.Generic.IReadOnlyCollection<ExpressionFramework.Domain.Expression> GroupByFields
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable GroupByFilter
        {
            get;
        }
    }
    public partial interface IParameterizedQuery : QueryFramework.Abstractions.IQuery
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        System.Collections.Generic.IReadOnlyCollection<QueryFramework.Abstractions.IQueryParameter> Parameters
        {
            get;
        }
    }
    public partial interface IQuery
    {
        System.Nullable<int> Limit
        {
            get;
        }

        System.Nullable<int> Offset
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable Filter
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        System.Collections.Generic.IReadOnlyCollection<QueryFramework.Abstractions.IQuerySortOrder> OrderByFields
        {
            get;
        }

        QueryFramework.Abstractions.Builders.IQueryBuilder ToBuilder();
    }
    public partial interface IQueryParameter
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        string Name
        {
            get;
        }

        object? Value
        {
            get;
        }

        QueryFramework.Abstractions.Builders.IQueryParameterBuilder ToBuilder();
    }
    public partial interface IQueryParameterValue
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        string Name
        {
            get;
        }

        QueryFramework.Abstractions.Builders.IQueryParameterValueBuilder ToBuilder();
    }
    public partial interface IQuerySortOrder
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        ExpressionFramework.Domain.Expression FieldNameExpression
        {
            get;
        }

        QueryFramework.Abstractions.Domains.QuerySortOrderDirection Order
        {
            get;
        }

        QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder ToBuilder();
    }
    public partial interface ISingleEntityQuery : QueryFramework.Abstractions.IQuery
    {
    }
}
#nullable disable
