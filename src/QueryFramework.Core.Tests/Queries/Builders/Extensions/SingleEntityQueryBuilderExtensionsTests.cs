using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using Xunit;

namespace QueryFramework.Core.Tests.Queries.Builders.Extensions
{
    [ExcludeFromCodeCoverage]
    public class SingleEntityQueryBuilderExtensionsTests
    {
        [Fact]
        public void Can_Use_Where_With_QueryConditionBuilder_To_Add_Condition()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.Where(new QueryConditionBuilder("field", QueryOperator.Greater, "value", true, true, QueryCombination.Or));

            // Assert
            actual.Conditions.Should().HaveCount(1);
            actual.Conditions.First().Field.FieldName.Should().Be("field");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeTrue();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.Or);
        }

        [Fact]
        public void Can_Use_Where_With_QueryCondition_To_Add_Condition()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.Where(new QueryCondition("field", QueryOperator.Greater, "value", true, true, QueryCombination.Or));

            // Assert
            actual.Conditions.Should().HaveCount(1);
            actual.Conditions.First().Field.FieldName.Should().Be("field");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeTrue();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.Or);
        }

        [Fact]
        public void Can_Use_Or_With_QueryConditionBuilder_To_Add_Condition()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.Or(new QueryConditionBuilder("field", QueryOperator.Greater, "value", true, true, QueryCombination.And));

            // Assert
            actual.Conditions.Should().HaveCount(1);
            actual.Conditions.First().Field.FieldName.Should().Be("field");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeTrue();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.Or);
        }

        [Fact]
        public void Can_Use_Or_With_QueryCondition_To_Add_Condition()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.Or(new QueryCondition("field", QueryOperator.Greater, "value", true, true, QueryCombination.And));

            // Assert
            actual.Conditions.Should().HaveCount(1);
            actual.Conditions.First().Field.FieldName.Should().Be("field");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeTrue();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.Or);
        }

        [Fact]
        public void Can_Use_And_With_QueryConditionBuilder_To_Add_Condition()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.And(new QueryConditionBuilder("field", QueryOperator.Greater, "value", true, true, QueryCombination.Or));

            // Assert
            actual.Conditions.Should().HaveCount(1);
            actual.Conditions.First().Field.FieldName.Should().Be("field");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeTrue();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
        }

        [Fact]
        public void Can_Use_And_With_QueryCondition_To_Add_Condition()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.And(new QueryCondition("field", QueryOperator.Greater, "value", true, true, QueryCombination.Or));

            // Assert
            actual.Conditions.Should().HaveCount(1);
            actual.Conditions.First().Field.FieldName.Should().Be("field");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeTrue();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
        }

        [Fact]
        public void Can_Use_AndAny_With_QueryConditionBuilders_To_Add_Conditions()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.AndAny
            (
                new QueryConditionBuilder("field1", QueryOperator.Greater, "value1"),
                new QueryConditionBuilder("field2", QueryOperator.Greater, "value2")
            );

            // Assert
            actual.Conditions.Should().HaveCount(2);
            actual.Conditions.First().Field.FieldName.Should().Be("field1");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value1");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeFalse();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
            actual.Conditions.Last().Field.FieldName.Should().Be("field2");
            actual.Conditions.Last().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.Last().Value.Should().Be("value2");
            actual.Conditions.Last().OpenBracket.Should().BeFalse();
            actual.Conditions.Last().CloseBracket.Should().BeTrue();
            actual.Conditions.Last().Combination.Should().Be(QueryCombination.Or);
        }

        [Fact]
        public void Can_Use_AndAny_With_QueryConditions_To_Add_Conditions()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.AndAny
            (
                new QueryCondition("field1", QueryOperator.Greater, "value1"),
                new QueryCondition("field2", QueryOperator.Greater, "value2")
            );

            // Assert
            actual.Conditions.Should().HaveCount(2);
            actual.Conditions.First().Field.FieldName.Should().Be("field1");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value1");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeFalse();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
            actual.Conditions.Last().Field.FieldName.Should().Be("field2");
            actual.Conditions.Last().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.Last().Value.Should().Be("value2");
            actual.Conditions.Last().OpenBracket.Should().BeFalse();
            actual.Conditions.Last().CloseBracket.Should().BeTrue();
            actual.Conditions.Last().Combination.Should().Be(QueryCombination.Or);
        }

        [Fact]
        public void Can_Use_OrAll_With_QueryConditionBuilders_To_Add_Conditions()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrAll
            (
                new QueryConditionBuilder("field1", QueryOperator.Greater, "value1"),
                new QueryConditionBuilder("field2", QueryOperator.Greater, "value2")
            );

            // Assert
            actual.Conditions.Should().HaveCount(2);
            actual.Conditions.First().Field.FieldName.Should().Be("field1");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value1");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeFalse();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.Or);
            actual.Conditions.Last().Field.FieldName.Should().Be("field2");
            actual.Conditions.Last().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.Last().Value.Should().Be("value2");
            actual.Conditions.Last().OpenBracket.Should().BeFalse();
            actual.Conditions.Last().CloseBracket.Should().BeTrue();
            actual.Conditions.Last().Combination.Should().Be(QueryCombination.And);
        }

        [Fact]
        public void Can_Use_OrAll_With_QueryConditions_To_Add_Conditions()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrAll
            (
                new QueryCondition("field1", QueryOperator.Greater, "value1"),
                new QueryCondition("field2", QueryOperator.Greater, "value2")
            );

            // Assert
            actual.Conditions.Should().HaveCount(2);
            actual.Conditions.First().Field.FieldName.Should().Be("field1");
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.First().Value.Should().Be("value1");
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.First().CloseBracket.Should().BeFalse();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.Or);
            actual.Conditions.Last().Field.FieldName.Should().Be("field2");
            actual.Conditions.Last().Operator.Should().Be(QueryOperator.Greater);
            actual.Conditions.Last().Value.Should().Be("value2");
            actual.Conditions.Last().OpenBracket.Should().BeFalse();
            actual.Conditions.Last().CloseBracket.Should().BeTrue();
            actual.Conditions.Last().Combination.Should().Be(QueryCombination.And);
        }

        [Fact]
        public void Can_Use_OrderBy_With_FieldName_Strings_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderBy("Field1", "Field2", "Field3");

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
        }

        [Fact]
        public void Can_Use_OrderBy_With_QueryExpressions_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderBy(new QueryExpression("Field1"), new QueryExpression("Field2"), new QueryExpression("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
        }

        [Fact]
        public void Can_Use_OrderBy_With_QueryExpressionBuilders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderBy(new QueryExpressionBuilder("Field1"), new QueryExpressionBuilder("Field2"), new QueryExpressionBuilder("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
        }

        [Fact]
        public void Can_Use_OrderBy_With_QuerySortOrders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderBy(new QuerySortOrder("Field1"), new QuerySortOrder("Field2"), new QuerySortOrder("Field3", QuerySortOrderDirection.Descending));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_OrderBy_With_QuerySortOrderBuilders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderBy(new QuerySortOrderBuilder("Field1"), new QuerySortOrderBuilder("Field2"), new QuerySortOrderBuilder("Field3", QuerySortOrderDirection.Descending));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_OrderByDescending_With_FieldName_Strings_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderByDescending("Field1", "Field2", "Field3");

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_OrderByDescending_With_QueryExpressions_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderByDescending(new QueryExpression("Field1"), new QueryExpression("Field2"), new QueryExpression("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_OrderByDescending_With_QueryExpressionBuilders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderByDescending(new QueryExpressionBuilder("Field1"), new QueryExpressionBuilder("Field2"), new QueryExpressionBuilder("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_OrderByDescending_With_QuerySortOrders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderByDescending(new QuerySortOrder("Field1"), new QuerySortOrder("Field2"), new QuerySortOrder("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_OrderByDescending_With_QuerySortOrderBuilders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.OrderByDescending(new QuerySortOrderBuilder("Field1"), new QuerySortOrderBuilder("Field2"), new QuerySortOrderBuilder("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_ThenBy_With_FieldName_Strings_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderBy("Field1");

            // Act
            var actual = sut.ThenBy("Field2", "Field3");

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
        }

        [Fact]
        public void Can_Use_ThenBy_With_QueryExpressions_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderBy(new QueryExpression("Field1"), new QueryExpression("Field2"));

            // Act
            var actual = sut.ThenBy(new QueryExpression("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
        }

        [Fact]
        public void Can_Use_ThenBy_With_QueryExpressionBuilders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderBy(new QueryExpressionBuilder("Field1"));

            // Act
            var actual = sut.ThenBy(new QueryExpressionBuilder("Field2"), new QueryExpressionBuilder("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
        }

        [Fact]
        public void Can_Use_ThenBy_With_QuerySortOrders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderBy(new QuerySortOrder("Field1"), new QuerySortOrder("Field2"));

            // Act
            var actual = sut.ThenBy(new QuerySortOrder("Field3", QuerySortOrderDirection.Descending));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_ThenBy_With_QuerySortOrderBuilders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderBy(new QuerySortOrderBuilder("Field1"));

            // Act
            var actual = sut.ThenBy(new QuerySortOrderBuilder("Field2"), new QuerySortOrderBuilder("Field3", QuerySortOrderDirection.Descending));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_ThenByDescending_With_FieldName_Strings_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderBy("Field1", "Field2");

            // Act
            var actual = sut.ThenByDescending("Field3");

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_ThenByDescending_With_QueryExpressions_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderByDescending(new QueryExpression("Field1"), new QueryExpression("Field2"));

            // Act
            var actual = sut.ThenByDescending(new QueryExpression("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_ThenByDescending_With_QueryExpressionBuilders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderByDescending(new QueryExpressionBuilder("Field1")).ThenBy(new QueryExpressionBuilder("Field2"));

            // Act
            var actual = sut.ThenByDescending(new QueryExpressionBuilder("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_ThenByDescending_With_QuerySortOrders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderBy(new QuerySortOrder("Field1"));

            // Act
            var actual = sut.ThenByDescending(new QuerySortOrder("Field2"), new QuerySortOrder("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_ThenByDescending_With_QuerySortOrderBuilders_To_Add_OrderByClauses()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder().OrderBy(new QuerySortOrderBuilder("Field1"), new QuerySortOrderBuilder("Field2"));

            // Act
            var actual = sut.ThenByDescending(new QuerySortOrderBuilder("Field3"));

            // Assert
            actual.OrderByFields.Should().HaveCount(3);
            actual.OrderByFields.ElementAt(0).Field.FieldName.Should().Be("Field1");
            actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(1).Field.FieldName.Should().Be("Field2");
            actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
            actual.OrderByFields.ElementAt(2).Field.FieldName.Should().Be("Field3");
            actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Use_Offset_To_Set_Offset()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.Offset(100);

            // Assert
            actual.Offset.Should().Be(100);
        }

        [Fact]
        public void Can_Use_Limit_To_Set_Limit()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.Limit(200);

            // Assert
            actual.Limit.Should().Be(200);
        }

        [Fact]
        public void Can_Use_Skip_To_Set_Offset()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.Skip(100);

            // Assert
            actual.Offset.Should().Be(100);
        }

        [Fact]
        public void Can_Use_Take_To_Set_Limit()
        {
            // Arrange
            var sut = new SingleEntityQueryBuilder();

            // Act
            var actual = sut.Take(200);

            // Assert
            actual.Limit.Should().Be(200);
        }
    }
}
