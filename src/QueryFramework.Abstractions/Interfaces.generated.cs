﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.2
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryFramework.Abstractions
{
#nullable enable
    public partial interface IQueryParameter
    {
        string Name
        {
            get;
        }

        object Value
        {
            get;
        }
    }
#nullable restore

#nullable enable
    public partial interface IQueryParameterValue
    {
        string Name
        {
            get;
        }
    }
#nullable restore

#nullable enable
    public partial interface IQuerySortOrder
    {
        ExpressionFramework.Abstractions.DomainModel.IExpression Field
        {
            get;
        }

        QueryFramework.Abstractions.QuerySortOrderDirection Order
        {
            get;
        }
    }
#nullable restore
}
