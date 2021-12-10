using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core.Queries;
using Xunit;

namespace QueryFramework.Core.Tests.Queries
{
    [ExcludeFromCodeCoverage]
    public class SingleEntityQueryTests
    {
        [Fact]
        public void Can_Construct_SingleEntityQuery_With_Default_Values()
        {
            // Act
            var sut = new SingleEntityQuery();

            // Assert
            sut.Conditions.Should().BeEmpty();
            sut.Limit.Should().BeNull();
            sut.Offset.Should().BeNull();
            sut.OrderByFields.Should().BeEmpty();
        }

        [Fact]
        public void Can_Construct_SingleEntityQuery_With_Custom_Values()
        {
            // Arrange
            var conditions = new[] { new QueryCondition("field", Abstractions.QueryOperator.Equal, "value") };
            var orderByFields = new[] { new QuerySortOrder("field") };
            var limit = 1;
            var offset = 2;

            // Act
            var sut = new SingleEntityQuery(limit, offset, conditions, orderByFields);

            // Assert
            sut.Conditions.Should().BeEquivalentTo(conditions);
            sut.Limit.Should().Be(limit);
            sut.Offset.Should().Be(offset);
            sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
        }

        [Fact]
        public void Constructing_SingleEntityQuery_With_ValidationError_Leads_To_Exception()
        {
            // Arrange
            var action = new Action(() => _ = new FieldSelectionQuery(null, null, false, true, new[] { new QueryCondition("field", QueryOperator.Equal, openBracket: true) }, Enumerable.Empty<IQuerySortOrder>(), Enumerable.Empty<IQueryExpression>()));

            // Act
            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Can_Compare_SingleEntityQuery_With_Equal_Values()
        {
            // Arrange
            var q1 = new SingleEntityQuery(5, 64, new[] { new QueryCondition("Field", Abstractions.QueryOperator.Equal, "A") }, Enumerable.Empty<IQuerySortOrder>());
            var q2 = new SingleEntityQuery(5, 64, new[] { new QueryCondition("Field", Abstractions.QueryOperator.Equal, "A") }, Enumerable.Empty<IQuerySortOrder>());

            // Act
            var actual = q1.Equals(q2);

            // Asset
            actual.Should().BeTrue();
        }
    }
}
