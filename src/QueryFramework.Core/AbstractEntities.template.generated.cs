﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 8.0.8
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
    public abstract partial record Query : QueryFramework.Abstractions.IQuery
    {
        public System.Nullable<int> Limit
        {
            get;
        }

        public System.Nullable<int> Offset
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable Filter
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public System.Collections.Generic.IReadOnlyCollection<QueryFramework.Abstractions.IQuerySortOrder> OrderByFields
        {
            get;
        }

        protected Query(System.Nullable<int> limit, System.Nullable<int> offset, ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable filter, System.Collections.Generic.IEnumerable<QueryFramework.Abstractions.IQuerySortOrder> orderByFields)
        {
            if (filter is null) throw new System.ArgumentNullException(nameof(filter));
            if (orderByFields is null) throw new System.ArgumentNullException(nameof(orderByFields));
            this.Limit = limit;
            this.Offset = offset;
            this.Filter = filter;
            this.OrderByFields = new CrossCutting.Common.ReadOnlyValueCollection<QueryFramework.Abstractions.IQuerySortOrder>(orderByFields);
        }

        public abstract QueryFramework.Abstractions.Builders.IQueryBuilder ToBuilder();
    }
}
#nullable disable
