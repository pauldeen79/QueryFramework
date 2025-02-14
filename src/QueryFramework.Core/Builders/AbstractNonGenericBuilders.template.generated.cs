﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 9.0.2
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace QueryFramework.Core.Builders
{
    public abstract partial class QueryBuilder : QueryFramework.Abstractions.Builders.IQueryBuilder, System.ComponentModel.INotifyPropertyChanged
    {
        private System.Nullable<int> _limit;

        private System.Nullable<int> _offset;

        private ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder _filter;

        private System.Collections.Generic.List<QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder> _orderByFields;

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        public System.Nullable<int> Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.Nullable<System.Int32>>.Default.Equals(_limit, value);
                _limit = value;
                if (hasChanged) HandlePropertyChanged(nameof(Limit));
            }
        }

        public System.Nullable<int> Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.Nullable<System.Int32>>.Default.Equals(_offset, value);
                _offset = value;
                if (hasChanged) HandlePropertyChanged(nameof(Offset));
            }
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder>.Default.Equals(_filter!, value!);
                _filter = value;
                if (hasChanged) HandlePropertyChanged(nameof(Filter));
            }
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public System.Collections.Generic.List<QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder> OrderByFields
        {
            get
            {
                return _orderByFields;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.Collections.Generic.IReadOnlyCollection<QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder>>.Default.Equals(_orderByFields!, value!);
                _orderByFields = value;
                if (hasChanged) HandlePropertyChanged(nameof(OrderByFields));
            }
        }

        protected QueryBuilder(QueryFramework.Abstractions.IQuery source)
        {
            _orderByFields = new System.Collections.Generic.List<QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder>();
            _limit = source.Limit;
            _offset = source.Offset;
            _filter = new ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder(source.Filter);
            foreach (var item in source.OrderByFields.Select(x => x.ToBuilder())) _orderByFields.Add(item);
        }

        protected QueryBuilder()
        {
            _orderByFields = new System.Collections.Generic.List<QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder>();
            _filter = new ExpressionFramework.Domain.Builders.Evaluatables.ComposedEvaluatableBuilder()!;
            SetDefaultValues();
        }

        public abstract QueryFramework.Core.Query Build();

        QueryFramework.Abstractions.IQuery QueryFramework.Abstractions.Builders.IQueryBuilder.Build()
        {
            return Build();
        }

        partial void SetDefaultValues();

        public static implicit operator QueryFramework.Core.Query(QueryBuilder entity)
        {
            return entity.Build();
        }

        protected void HandlePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
#nullable disable
