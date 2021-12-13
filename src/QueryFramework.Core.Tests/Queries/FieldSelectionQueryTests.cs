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
    public class FieldSelectionQueryTests
    {
        [Fact]
        public void Can_Construct_FieldSelectionQuery_With_Default_Values()
        {
            // Act
            var sut = new FieldSelectionQuery();

            // Assert
            sut.Conditions.Should().BeEmpty();
            sut.Distinct.Should().BeFalse();
            sut.Fields.Should().BeEmpty();
            sut.GetAllFields.Should().BeFalse();
            sut.Limit.Should().BeNull();
            sut.Offset.Should().BeNull();
            sut.OrderByFields.Should().BeEmpty();
        }

        [Fact]
        public void Can_Construct_FieldSelectionQuery_With_Custom_Values()
        {
            // Arrange
            var conditions = new[] { new QueryCondition("field", QueryOperator.Equal, "value") };
            var orderByFields = new[] { new QuerySortOrder("field") };
            var limit = 1;
            var offset = 2;
            var distinct = true;
            var getAllFields = true;
            var fields = new[] { new QueryExpression("field") };

            // Act
            var sut = new FieldSelectionQuery(limit, offset, distinct, getAllFields, conditions, orderByFields, fields);

            // Assert
            sut.Conditions.Should().BeEquivalentTo(conditions);
            sut.Distinct.Should().Be(distinct);
            sut.Fields.Should().BeEquivalentTo(fields);
            sut.GetAllFields.Should().Be(getAllFields);
            sut.Limit.Should().Be(limit);
            sut.Offset.Should().Be(offset);
            sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
        }

        [Fact]
        public void Constructing_FieldSelectionQuery_With_ValidationError_Leads_To_Exception()
        {
            // Arrange
            var action = new Action(() => _ = new FieldSelectionQuery(null, null, false, true, new[] { new QueryCondition("field", QueryOperator.Equal, openBracket: true) }, Enumerable.Empty<IQuerySortOrder>(), Enumerable.Empty<IQueryExpression>()));

            // Act
            action.Should().Throw<ValidationException>();
        }
    }
}
