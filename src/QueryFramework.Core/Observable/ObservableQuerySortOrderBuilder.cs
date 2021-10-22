using System.ComponentModel;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Observable
{
    public class ObservableQuerySortOrderBuilder : IQuerySortOrderBuilder, INotifyPropertyChanged
    {
        private IQueryExpressionBuilder _field;
        private QuerySortOrderDirection _order;

        public event PropertyChangedEventHandler PropertyChanged;

        public IQueryExpressionBuilder Field
        {
            get => _field;
            set
            {
                _field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Field)));
            }
        }

        public QuerySortOrderDirection Order
        {
            get => _order;
            set
            {
                _order = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Order)));
            }
        }

        public IQuerySortOrder Build()
        {
            return new QuerySortOrder(Field.Build(), Order);
        }
        public ObservableQuerySortOrderBuilder(IQuerySortOrder source = null)
        {
            Field = new QueryExpressionBuilder();
            if (source != null)
            {
                Field.Update(source.Field);
                Order = source.Order;
            }
        }
        public ObservableQuerySortOrderBuilder(IQueryExpression expression, QuerySortOrderDirection order = QuerySortOrderDirection.Ascending)
        {
            Field = expression.ToBuilder();
            Order = order;
        }
        public ObservableQuerySortOrderBuilder(string fieldName, QuerySortOrderDirection order = QuerySortOrderDirection.Ascending)
        {
            Field = new QueryExpressionBuilder(fieldName);
            Order = order;
        }
    }
}
