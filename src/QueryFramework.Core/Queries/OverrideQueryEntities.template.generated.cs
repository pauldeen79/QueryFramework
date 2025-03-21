﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 9.0.3
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace QueryFramework.Core.Queries
{
    public partial record DataObjectNameQuery : QueryFramework.Core.Query, QueryFramework.Abstractions.IQuery, QueryFramework.Abstractions.IDataObjectNameQuery
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        public string DataObjectName
        {
            get;
        }

        public DataObjectNameQuery(System.Nullable<int> limit, System.Nullable<int> offset, ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable filter, System.Collections.Generic.IEnumerable<QueryFramework.Abstractions.IQuerySortOrder> orderByFields, string dataObjectName) : base(limit, offset, filter, orderByFields)
        {
            this.DataObjectName = dataObjectName;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public override QueryFramework.Abstractions.Builders.IQueryBuilder ToBuilder()
        {
            return ToTypedBuilder();
        }

        public QueryFramework.Core.Builders.Queries.DataObjectNameQueryBuilder ToTypedBuilder()
        {
            return new QueryFramework.Core.Builders.Queries.DataObjectNameQueryBuilder(this);
        }

        QueryFramework.Abstractions.Builders.IQueryBuilder QueryFramework.Abstractions.IQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }

        QueryFramework.Abstractions.Builders.IDataObjectNameQueryBuilder QueryFramework.Abstractions.IDataObjectNameQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }
    }
    public partial record FieldSelectionQuery : QueryFramework.Core.Query, QueryFramework.Abstractions.IQuery, QueryFramework.Abstractions.IFieldSelectionQuery
    {
        public bool Distinct
        {
            get;
        }

        public bool GetAllFields
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public System.Collections.Generic.IReadOnlyCollection<string> FieldNames
        {
            get;
        }

        public FieldSelectionQuery(System.Nullable<int> limit, System.Nullable<int> offset, ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable filter, System.Collections.Generic.IEnumerable<QueryFramework.Abstractions.IQuerySortOrder> orderByFields, bool distinct, bool getAllFields, System.Collections.Generic.IEnumerable<string> fieldNames) : base(limit, offset, filter, orderByFields)
        {
            this.Distinct = distinct;
            this.GetAllFields = getAllFields;
            this.FieldNames = fieldNames is null ? null! : new CrossCutting.Common.ReadOnlyValueCollection<System.String>(fieldNames);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public override QueryFramework.Abstractions.Builders.IQueryBuilder ToBuilder()
        {
            return ToTypedBuilder();
        }

        public QueryFramework.Core.Builders.Queries.FieldSelectionQueryBuilder ToTypedBuilder()
        {
            return new QueryFramework.Core.Builders.Queries.FieldSelectionQueryBuilder(this);
        }

        QueryFramework.Abstractions.Builders.IQueryBuilder QueryFramework.Abstractions.IQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }

        QueryFramework.Abstractions.Builders.IFieldSelectionQueryBuilder QueryFramework.Abstractions.IFieldSelectionQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }
    }
    public partial record GroupingQuery : QueryFramework.Core.Query, QueryFramework.Abstractions.IQuery, QueryFramework.Abstractions.IGroupingQuery
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public System.Collections.Generic.IReadOnlyCollection<ExpressionFramework.Domain.Expression> GroupByFields
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable GroupByFilter
        {
            get;
        }

        public GroupingQuery(System.Nullable<int> limit, System.Nullable<int> offset, ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable filter, System.Collections.Generic.IEnumerable<QueryFramework.Abstractions.IQuerySortOrder> orderByFields, System.Collections.Generic.IEnumerable<ExpressionFramework.Domain.Expression> groupByFields, ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable groupByFilter) : base(limit, offset, filter, orderByFields)
        {
            this.GroupByFields = groupByFields is null ? null! : new CrossCutting.Common.ReadOnlyValueCollection<ExpressionFramework.Domain.Expression>(groupByFields);
            this.GroupByFilter = groupByFilter;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public override QueryFramework.Abstractions.Builders.IQueryBuilder ToBuilder()
        {
            return ToTypedBuilder();
        }

        public QueryFramework.Core.Builders.Queries.GroupingQueryBuilder ToTypedBuilder()
        {
            return new QueryFramework.Core.Builders.Queries.GroupingQueryBuilder(this);
        }

        QueryFramework.Abstractions.Builders.IQueryBuilder QueryFramework.Abstractions.IQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }

        QueryFramework.Abstractions.Builders.IGroupingQueryBuilder QueryFramework.Abstractions.IGroupingQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }
    }
    public partial record ParameterizedQuery : QueryFramework.Core.Query, QueryFramework.Abstractions.IQuery, QueryFramework.Abstractions.IParameterizedQuery
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public System.Collections.Generic.IReadOnlyCollection<QueryFramework.Abstractions.IQueryParameter> Parameters
        {
            get;
        }

        public ParameterizedQuery(System.Nullable<int> limit, System.Nullable<int> offset, ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable filter, System.Collections.Generic.IEnumerable<QueryFramework.Abstractions.IQuerySortOrder> orderByFields, System.Collections.Generic.IEnumerable<QueryFramework.Abstractions.IQueryParameter> parameters) : base(limit, offset, filter, orderByFields)
        {
            this.Parameters = parameters is null ? null! : new CrossCutting.Common.ReadOnlyValueCollection<QueryFramework.Abstractions.IQueryParameter>(parameters);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public override QueryFramework.Abstractions.Builders.IQueryBuilder ToBuilder()
        {
            return ToTypedBuilder();
        }

        public QueryFramework.Core.Builders.Queries.ParameterizedQueryBuilder ToTypedBuilder()
        {
            return new QueryFramework.Core.Builders.Queries.ParameterizedQueryBuilder(this);
        }

        QueryFramework.Abstractions.Builders.IQueryBuilder QueryFramework.Abstractions.IQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }

        QueryFramework.Abstractions.Builders.IParameterizedQueryBuilder QueryFramework.Abstractions.IParameterizedQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }
    }
    public partial record SingleEntityQuery : QueryFramework.Core.Query, QueryFramework.Abstractions.IQuery, QueryFramework.Abstractions.ISingleEntityQuery
    {
        public SingleEntityQuery(System.Nullable<int> limit, System.Nullable<int> offset, ExpressionFramework.Domain.Evaluatables.ComposedEvaluatable filter, System.Collections.Generic.IEnumerable<QueryFramework.Abstractions.IQuerySortOrder> orderByFields) : base(limit, offset, filter, orderByFields)
        {
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public override QueryFramework.Abstractions.Builders.IQueryBuilder ToBuilder()
        {
            return ToTypedBuilder();
        }

        public QueryFramework.Core.Builders.Queries.SingleEntityQueryBuilder ToTypedBuilder()
        {
            return new QueryFramework.Core.Builders.Queries.SingleEntityQueryBuilder(this);
        }

        QueryFramework.Abstractions.Builders.IQueryBuilder QueryFramework.Abstractions.IQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }

        QueryFramework.Abstractions.Builders.ISingleEntityQueryBuilder QueryFramework.Abstractions.ISingleEntityQuery.ToBuilder()
        {
            return ToTypedBuilder();
        }
    }
}
#nullable disable
