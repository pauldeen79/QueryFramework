﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.1
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryFramework.Abstractions.Builders
{
#nullable enable
    public partial interface IQueryConditionBuilder
    {
        bool OpenBracket
        {
            get;
            set;
        }

        bool CloseBracket
        {
            get;
            set;
        }

        QueryFramework.Abstractions.Builders.IQueryExpressionBuilder Field
        {
            get;
            set;
        }

        QueryFramework.Abstractions.QueryOperator Operator
        {
            get;
            set;
        }

        object? Value
        {
            get;
            set;
        }

        QueryFramework.Abstractions.QueryCombination Combination
        {
            get;
            set;
        }

        QueryFramework.Abstractions.IQueryCondition Build();
    }
#nullable restore

#nullable enable
    public partial interface IQueryExpressionBuilder
    {
        string FieldName
        {
            get;
            set;
        }

        QueryFramework.Abstractions.Builders.IQueryExpressionFunctionBuilder? Function
        {
            get;
            set;
        }

        QueryFramework.Abstractions.IQueryExpression Build();
    }
#nullable restore

#nullable enable
    public partial interface IQueryExpressionFunctionBuilder
    {
        QueryFramework.Abstractions.Builders.IQueryExpressionFunctionBuilder? InnerFunction
        {
            get;
            set;
        }

        QueryFramework.Abstractions.IQueryExpressionFunction Build();
    }
#nullable restore

#nullable enable
    public partial interface IQueryParameterBuilder
    {
        string Name
        {
            get;
            set;
        }

        object Value
        {
            get;
            set;
        }

        QueryFramework.Abstractions.IQueryParameter Build();
    }
#nullable restore

#nullable enable
    public partial interface IQueryParameterValueBuilder
    {
        string Name
        {
            get;
            set;
        }

        QueryFramework.Abstractions.IQueryParameterValue Build();
    }
#nullable restore

#nullable enable
    public partial interface IQuerySortOrderBuilder
    {
        QueryFramework.Abstractions.Builders.IQueryExpressionBuilder Field
        {
            get;
            set;
        }

        QueryFramework.Abstractions.QuerySortOrderDirection Order
        {
            get;
            set;
        }

        QueryFramework.Abstractions.IQuerySortOrder Build();
    }
#nullable restore
}
