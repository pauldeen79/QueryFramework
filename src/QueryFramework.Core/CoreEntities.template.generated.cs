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
namespace QueryFramework.Core
{
    public partial record QueryParameter : QueryFramework.Abstractions.IQueryParameter
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public string Name
        {
            get;
        }

        public object? Value
        {
            get;
        }

        public QueryParameter(string name, object? value)
        {
            this.Name = name;
            this.Value = value;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public QueryFramework.Abstractions.Builders.IQueryParameterBuilder ToBuilder()
        {
            return new QueryFramework.Core.Builders.QueryParameterBuilder(this);
        }
    }
    public partial record QueryParameterValue : QueryFramework.Abstractions.IQueryParameterValue
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public string Name
        {
            get;
        }

        public QueryParameterValue(string name)
        {
            this.Name = name;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public QueryFramework.Abstractions.Builders.IQueryParameterValueBuilder ToBuilder()
        {
            return new QueryFramework.Core.Builders.QueryParameterValueBuilder(this);
        }
    }
    public partial record QuerySortOrder : QueryFramework.Abstractions.IQuerySortOrder
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public ExpressionFramework.Domain.Expression FieldNameExpression
        {
            get;
        }

        public QueryFramework.Abstractions.Domains.QuerySortOrderDirection Order
        {
            get;
        }

        public QuerySortOrder(ExpressionFramework.Domain.Expression fieldNameExpression, QueryFramework.Abstractions.Domains.QuerySortOrderDirection order)
        {
            this.FieldNameExpression = fieldNameExpression;
            this.Order = order;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder ToBuilder()
        {
            return new QueryFramework.Core.Builders.QuerySortOrderBuilder(this);
        }
    }
}
#nullable disable