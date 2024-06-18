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
namespace QueryFramework.Core.Builders.Queries
{
    public partial class DataObjectNameQueryBuilder : QueryFramework.Core.Builders.QueryBuilder<DataObjectNameQueryBuilder, QueryFramework.Core.Queries.DataObjectNameQuery>, QueryFramework.Abstractions.Builders.IQueryBuilder, QueryFramework.Abstractions.Builders.IDataObjectNameQueryBuilder
    {
        private string _dataObjectName;

        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings = true)]
        public string DataObjectName
        {
            get
            {
                return _dataObjectName;
            }
            set
            {
                _dataObjectName = value ?? throw new System.ArgumentNullException(nameof(value));
                HandlePropertyChanged(nameof(DataObjectName));
            }
        }

        public DataObjectNameQueryBuilder(QueryFramework.Abstractions.IDataObjectNameQuery source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _dataObjectName = source.DataObjectName;
        }

        public DataObjectNameQueryBuilder() : base()
        {
            _dataObjectName = string.Empty;
            SetDefaultValues();
        }

        public override QueryFramework.Core.Queries.DataObjectNameQuery BuildTyped()
        {
            return new QueryFramework.Core.Queries.DataObjectNameQuery(Limit, Offset, Filter.BuildTyped(), OrderByFields.Select(x => x.Build()!).ToList().AsReadOnly(), DataObjectName);
        }

        partial void SetDefaultValues();
    }
    public partial class FieldSelectionQueryBuilder : QueryFramework.Core.Builders.QueryBuilder<FieldSelectionQueryBuilder, QueryFramework.Core.Queries.FieldSelectionQuery>, QueryFramework.Abstractions.Builders.IQueryBuilder, QueryFramework.Abstractions.Builders.IFieldSelectionQueryBuilder
    {
        private bool _distinct;

        private bool _getAllFields;

        private System.Collections.Generic.List<string> _fieldNames;

        public bool Distinct
        {
            get
            {
                return _distinct;
            }
            set
            {
                _distinct = value;
                HandlePropertyChanged(nameof(Distinct));
            }
        }

        public bool GetAllFields
        {
            get
            {
                return _getAllFields;
            }
            set
            {
                _getAllFields = value;
                HandlePropertyChanged(nameof(GetAllFields));
            }
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public System.Collections.Generic.List<string> FieldNames
        {
            get
            {
                return _fieldNames;
            }
            set
            {
                _fieldNames = value ?? throw new System.ArgumentNullException(nameof(value));
                HandlePropertyChanged(nameof(FieldNames));
            }
        }

        public FieldSelectionQueryBuilder(QueryFramework.Abstractions.IFieldSelectionQuery source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _fieldNames = new System.Collections.Generic.List<string>();
            _distinct = source.Distinct;
            _getAllFields = source.GetAllFields;
            if (source.FieldNames is not null) foreach (var item in source.FieldNames) _fieldNames.Add(item);
        }

        public FieldSelectionQueryBuilder() : base()
        {
            _fieldNames = new System.Collections.Generic.List<string>();
            SetDefaultValues();
        }

        public override QueryFramework.Core.Queries.FieldSelectionQuery BuildTyped()
        {
            return new QueryFramework.Core.Queries.FieldSelectionQuery(Limit, Offset, Filter.BuildTyped(), OrderByFields.Select(x => x.Build()!).ToList().AsReadOnly(), Distinct, GetAllFields, FieldNames);
        }

        partial void SetDefaultValues();
    }
    public partial class GroupingQueryBuilder : QueryFramework.Core.Builders.QueryBuilder<GroupingQueryBuilder, QueryFramework.Core.Queries.GroupingQuery>, QueryFramework.Abstractions.Builders.IQueryBuilder, QueryFramework.Abstractions.Builders.IGroupingQueryBuilder
    {
        private System.Collections.Generic.List<ExpressionFramework.Domain.Builders.ExpressionBuilder> _groupByFields;

        private ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder _groupByFilter;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public System.Collections.Generic.List<ExpressionFramework.Domain.Builders.ExpressionBuilder> GroupByFields
        {
            get
            {
                return _groupByFields;
            }
            set
            {
                _groupByFields = value ?? throw new System.ArgumentNullException(nameof(value));
                HandlePropertyChanged(nameof(GroupByFields));
            }
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder GroupByFilter
        {
            get
            {
                return _groupByFilter;
            }
            set
            {
                _groupByFilter = value ?? throw new System.ArgumentNullException(nameof(value));
                HandlePropertyChanged(nameof(GroupByFilter));
            }
        }

        public GroupingQueryBuilder(QueryFramework.Abstractions.IGroupingQuery source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _groupByFields = new System.Collections.Generic.List<ExpressionFramework.Domain.Builders.ExpressionBuilder>();
            if (source.GroupByFields is not null) foreach (var item in source.GroupByFields.Select(x => x.ToBuilder())) _groupByFields.Add(item);
            _groupByFilter = new ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder(source.GroupByFilter);
        }

        public GroupingQueryBuilder() : base()
        {
            _groupByFields = new System.Collections.Generic.List<ExpressionFramework.Domain.Builders.ExpressionBuilder>();
            _groupByFilter = new ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder()!;
            SetDefaultValues();
        }

        public override QueryFramework.Core.Queries.GroupingQuery BuildTyped()
        {
            return new QueryFramework.Core.Queries.GroupingQuery(Limit, Offset, Filter.BuildTyped(), OrderByFields.Select(x => x.Build()!).ToList().AsReadOnly(), GroupByFields.Select(x => x.Build()!).ToList().AsReadOnly(), GroupByFilter.BuildTyped());
        }

        partial void SetDefaultValues();
    }
    public partial class ParameterizedQueryBuilder : QueryFramework.Core.Builders.QueryBuilder<ParameterizedQueryBuilder, QueryFramework.Core.Queries.ParameterizedQuery>, QueryFramework.Abstractions.Builders.IQueryBuilder, QueryFramework.Abstractions.Builders.IParameterizedQueryBuilder
    {
        private System.Collections.Generic.List<QueryFramework.Abstractions.Builders.IQueryParameterBuilder> _parameters;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public System.Collections.Generic.List<QueryFramework.Abstractions.Builders.IQueryParameterBuilder> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value ?? throw new System.ArgumentNullException(nameof(value));
                HandlePropertyChanged(nameof(Parameters));
            }
        }

        public ParameterizedQueryBuilder(QueryFramework.Abstractions.IParameterizedQuery source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _parameters = new System.Collections.Generic.List<QueryFramework.Abstractions.Builders.IQueryParameterBuilder>();
            if (source.Parameters is not null) foreach (var item in source.Parameters.Select(x => x.ToBuilder())) _parameters.Add(item);
        }

        public ParameterizedQueryBuilder() : base()
        {
            _parameters = new System.Collections.Generic.List<QueryFramework.Abstractions.Builders.IQueryParameterBuilder>();
            SetDefaultValues();
        }

        public override QueryFramework.Core.Queries.ParameterizedQuery BuildTyped()
        {
            return new QueryFramework.Core.Queries.ParameterizedQuery(Limit, Offset, Filter.BuildTyped(), OrderByFields.Select(x => x.Build()!).ToList().AsReadOnly(), Parameters.Select(x => x.Build()!).ToList().AsReadOnly());
        }

        partial void SetDefaultValues();
    }
    public partial class SingleEntityQueryBuilder : QueryFramework.Core.Builders.QueryBuilder<SingleEntityQueryBuilder, QueryFramework.Core.Queries.SingleEntityQuery>, QueryFramework.Abstractions.Builders.IQueryBuilder, QueryFramework.Abstractions.Builders.ISingleEntityQueryBuilder
    {
        public SingleEntityQueryBuilder(QueryFramework.Abstractions.ISingleEntityQuery source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
        }

        public SingleEntityQueryBuilder() : base()
        {
            SetDefaultValues();
        }

        public override QueryFramework.Core.Queries.SingleEntityQuery BuildTyped()
        {
            return new QueryFramework.Core.Queries.SingleEntityQuery(Limit, Offset, Filter.BuildTyped(), OrderByFields.Select(x => x.Build()!).ToList().AsReadOnly());
        }

        partial void SetDefaultValues();
    }
}
#nullable disable
