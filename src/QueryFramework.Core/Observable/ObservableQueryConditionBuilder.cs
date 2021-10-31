using System.ComponentModel;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;

namespace QueryFramework.Core.Observable
{
    public class ObservableQueryConditionBuilder : IQueryConditionBuilder, INotifyPropertyChanged
    {
        private bool _openBracket;
        private bool _closeBracket;
        private IQueryExpressionBuilder _field;
        private QueryOperator _operator;
        private object? _value;
        private QueryCombination _combination;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool OpenBracket
        {
            get => _openBracket;
            set
            {
                _openBracket = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OpenBracket)));
            }
        }
        public bool CloseBracket
        {
            get => _closeBracket;
            set
            {
                _closeBracket = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CloseBracket)));
            }
        }
        public IQueryExpressionBuilder Field
        {
            get => _field;
            set
            {
                _field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Field)));
            }
        }
        public QueryOperator Operator
        {
            get => _operator;
            set
            {
                _operator = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Operator)));
            }
        }
        public object? Value
        {
            get => _value;
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
        public QueryCombination Combination
        {
            get => _combination;
            set
            {
                _combination = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Combination)));
            }
        }
        public IQueryCondition Build()
        {
            return new QueryCondition(Field.Build(), Operator, Value, OpenBracket, CloseBracket, Combination);
        }
        public ObservableQueryConditionBuilder(IQueryCondition? source = null)
        {
            _field = new ObservableQueryExpressionBuilder();
            if (source != null)
            {
                OpenBracket = source.OpenBracket;
                CloseBracket = source.CloseBracket;
                Field.Update(source.Field);
                Operator = source.Operator;
                Value = source.Value;
                Combination = source.Combination;
            }
        }
        public ObservableQueryConditionBuilder(IQueryExpression expression,
                                               QueryOperator queryOperator,
                                               object? value = null,
                                               bool openBracket = false,
                                               bool closeBracket = false,
                                               QueryCombination combination = QueryCombination.And)
        {
            _field = new ObservableQueryExpressionBuilder(expression);
            Operator = queryOperator;
            Value = value;
            OpenBracket = openBracket;
            CloseBracket = closeBracket;
            Combination = combination;
        }
        public ObservableQueryConditionBuilder(string fieldName,
                                               QueryOperator queryOperator,
                                               object? value = null,
                                               bool openBracket = false,
                                               bool closeBracket = false,
                                               QueryCombination combination = QueryCombination.And)
        {
            _field = new ObservableQueryExpressionBuilder(new QueryExpression(fieldName));
            Operator = queryOperator;
            Value = value;
            OpenBracket = openBracket;
            CloseBracket = closeBracket;
            Combination = combination;
        }
    }
}
