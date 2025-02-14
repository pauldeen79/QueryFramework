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
    public partial class QueryParameterBuilder : QueryFramework.Abstractions.Builders.IQueryParameterBuilder, System.ComponentModel.INotifyPropertyChanged
    {
        private string _name;

        private object? _value;

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.String>.Default.Equals(_name!, value!);
                _name = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(Name));
            }
        }

        public object? Value
        {
            get
            {
                return _value;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.Object>.Default.Equals(_value!, value!);
                _value = value;
                if (hasChanged) HandlePropertyChanged(nameof(Value));
            }
        }

        public QueryParameterBuilder(QueryFramework.Abstractions.IQueryParameter source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _name = source.Name;
            _value = source.Value;
        }

        public QueryParameterBuilder()
        {
            _name = string.Empty;
            SetDefaultValues();
        }

        public QueryFramework.Abstractions.IQueryParameter Build()
        {
            return new QueryFramework.Core.QueryParameter(Name, Value);
        }

        QueryFramework.Abstractions.IQueryParameter QueryFramework.Abstractions.Builders.IQueryParameterBuilder.Build()
        {
            return Build();
        }

        partial void SetDefaultValues();

        protected void HandlePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    public partial class QueryParameterValueBuilder : QueryFramework.Abstractions.Builders.IQueryParameterValueBuilder, System.ComponentModel.INotifyPropertyChanged
    {
        private string _name;

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.String>.Default.Equals(_name!, value!);
                _name = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(Name));
            }
        }

        public QueryParameterValueBuilder(QueryFramework.Abstractions.IQueryParameterValue source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _name = source.Name;
        }

        public QueryParameterValueBuilder()
        {
            _name = string.Empty;
            SetDefaultValues();
        }

        public QueryFramework.Abstractions.IQueryParameterValue Build()
        {
            return new QueryFramework.Core.QueryParameterValue(Name);
        }

        QueryFramework.Abstractions.IQueryParameterValue QueryFramework.Abstractions.Builders.IQueryParameterValueBuilder.Build()
        {
            return Build();
        }

        partial void SetDefaultValues();

        protected void HandlePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    public partial class QuerySortOrderBuilder : QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder, System.ComponentModel.INotifyPropertyChanged
    {
        private ExpressionFramework.Domain.Builders.ExpressionBuilder _fieldNameExpression;

        private QueryFramework.Abstractions.Domains.QuerySortOrderDirection _order;

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public ExpressionFramework.Domain.Builders.ExpressionBuilder FieldNameExpression
        {
            get
            {
                return _fieldNameExpression;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<ExpressionFramework.Domain.Builders.ExpressionBuilder>.Default.Equals(_fieldNameExpression!, value!);
                _fieldNameExpression = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(FieldNameExpression));
            }
        }

        public QueryFramework.Abstractions.Domains.QuerySortOrderDirection Order
        {
            get
            {
                return _order;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<QueryFramework.Abstractions.Domains.QuerySortOrderDirection>.Default.Equals(_order, value);
                _order = value;
                if (hasChanged) HandlePropertyChanged(nameof(Order));
            }
        }

        public QuerySortOrderBuilder(QueryFramework.Abstractions.IQuerySortOrder source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _fieldNameExpression = source.FieldNameExpression?.ToBuilder()!;
            _order = source.Order;
        }

        public QuerySortOrderBuilder()
        {
            _fieldNameExpression = default(ExpressionFramework.Domain.Builders.ExpressionBuilder)!;
            SetDefaultValues();
        }

        public QueryFramework.Abstractions.IQuerySortOrder Build()
        {
            return new QueryFramework.Core.QuerySortOrder(FieldNameExpression?.Build()!, Order);
        }

        QueryFramework.Abstractions.IQuerySortOrder QueryFramework.Abstractions.Builders.IQuerySortOrderBuilder.Build()
        {
            return Build();
        }

        partial void SetDefaultValues();

        protected void HandlePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
#nullable disable
