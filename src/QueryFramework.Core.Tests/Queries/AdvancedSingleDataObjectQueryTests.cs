using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.Core.Queries;
using Xunit;

namespace QueryFramework.Core.Tests.Queries
{
    [ExcludeFromCodeCoverage]
    public class AdvancedSingleDataObjectQueryTests
    {
        [Fact]
        public void Can_Construct_AdvancedSingleDataObjectQuery_With_Default_Values()
        {
            // Act
            var sut = new AdvancedSingleDataObjectQuery();

            // Assert
            sut.Conditions.Should().BeEmpty();
            sut.DataObjectName.Should().BeNull();
            sut.Distinct.Should().BeFalse();
            sut.Fields.Should().BeEmpty();
            sut.GetAllFields.Should().BeFalse();
            sut.GroupByFields.Should().BeEmpty();
            sut.HavingFields.Should().BeEmpty();
            sut.Limit.Should().BeNull();
            sut.Offset.Should().BeNull();
            sut.OrderByFields.Should().BeEmpty();
            sut.Parameters.Should().BeEmpty();
        }

        [Fact]
        public void Can_Construct_AdvancedSingleDataObjectQuery_With_Custom_Values()
        {
            // Arrange
            var conditions = new[] { new QueryCondition("field", Abstractions.QueryOperator.Equal, "value") };
            var dataObjectName = "dataObjectName";
            var orderByFields = new[] { new QuerySortOrder("field") };
            var limit = 1;
            var offset = 2;
            var distinct = true;
            var getAllFields = true;
            var fields = new[] { new QueryExpression("field") };
            var groupByFields = new[] { new QueryExpression("field") };
            var havingFields = new[] { new QueryCondition("field", Abstractions.QueryOperator.Equal, "value") };
            var parameters = new[] { new QueryParameter("name", "value") };

            // Act
            var sut = new AdvancedSingleDataObjectQuery(dataObjectName, conditions, orderByFields, limit, offset, distinct, getAllFields, fields, groupByFields, havingFields, parameters);

            // Assert
            sut.Conditions.Should().BeEquivalentTo(conditions);
            sut.DataObjectName.Should().Be(dataObjectName);
            sut.Distinct.Should().Be(distinct);
            sut.Fields.Should().BeEquivalentTo(fields);
            sut.GetAllFields.Should().Be(getAllFields);
            sut.GroupByFields.Should().BeEquivalentTo(groupByFields);
            sut.HavingFields.Should().BeEquivalentTo(havingFields);
            sut.Limit.Should().Be(limit);
            sut.Offset.Should().Be(offset);
            sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
            sut.Parameters.Should().BeEquivalentTo(parameters);
        }
    }
}
