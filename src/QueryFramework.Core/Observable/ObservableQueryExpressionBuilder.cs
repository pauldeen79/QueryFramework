using System.ComponentModel;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Core.Observable
{
    public class ObservableQueryExpressionBuilder : IQueryExpressionBuilder, INotifyPropertyChanged
    {
        private string _fieldName;
        private string _expression;

        public string FieldName
        {
            get => _fieldName;
            set
            {
                _fieldName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FieldName)));
            }
        }

        public string Expression
        {
            get => _expression;
            set
            {
                _expression = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Expression)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IQueryExpression Build()
        {
            return new QueryExpression(FieldName, Expression);
        }
        public ObservableQueryExpressionBuilder() : this(null)
        {
        }
        public ObservableQueryExpressionBuilder(IQueryExpression source)
        {
            if (source != null)
            {
                FieldName = source.FieldName;
                Expression = !(source is IExpressionContainer expressionContainer)
                    ? source.Expression
                    : expressionContainer.SourceExpression;
            }
        }
        public ObservableQueryExpressionBuilder(string fieldName, string expression = null)
        {
            FieldName = fieldName;
            Expression = expression;
        }
    }
}
