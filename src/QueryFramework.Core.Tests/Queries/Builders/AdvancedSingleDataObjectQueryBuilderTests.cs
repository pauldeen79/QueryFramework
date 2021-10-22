using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Queries.Builders;
using Xunit;

namespace QueryFramework.Core.Tests.Queries.Builders
{
    [ExcludeFromCodeCoverage]
    public class AdvancedSingleDataObjectQueryBuilderTests
    {
        [Fact]
        public void Can_Construct_AdvancedSingleDataObjectQueryBuilder_With_Default_Values()
        {
            // Act
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Assert
            sut.Conditions.Should().BeEmpty();
            sut.Distinct.Should().BeFalse();
            sut.Fields.Should().BeEmpty();
            sut.GetAllFields.Should().BeFalse();
            sut.GroupByFields.Should().BeEmpty();
            sut.HavingFields.Should().BeEmpty();
            sut.Limit.Should().BeNull();
            sut.Offset.Should().BeNull();
            sut.OrderByFields.Should().BeEmpty();
        }

        [Fact]
        public void Can_Construct_AdvancedSingleDataObjectQueryBuilder_With_Custom_Values()
        {
            // Arrange
            var conditions = new[] { new QueryConditionBuilder("field", QueryOperator.Equal, "value") };
            var orderByFields = new[] { new QuerySortOrderBuilder("field") };
            var limit = 1;
            var offset = 2;
            var distinct = true;
            var getAllFields = true;
            var fields = new[] { new QueryExpressionBuilder("field") };
            var groupByFields = new[] { new QueryExpressionBuilder("field") };
            var havingFields = new[] { new QueryConditionBuilder("field", QueryOperator.Equal, "value") };
            var dataObjectName = "dataObjectName";
            var parameters = new[] { new QueryParameterBuilder("name", "value") };

            // Act
            var sut = new AdvancedSingleDataObjectQueryBuilder
            {
                Conditions = conditions.Cast<IQueryConditionBuilder>().ToList(),
                OrderByFields = orderByFields.Cast<IQuerySortOrderBuilder>().ToList(),
                Limit = limit,
                Offset = offset,
                Distinct = distinct,
                GetAllFields = getAllFields,
                Fields = fields.Cast<IQueryExpressionBuilder>().ToList(),
                GroupByFields = groupByFields.Cast<IQueryExpressionBuilder>().ToList(),
                HavingFields = havingFields.Cast<IQueryConditionBuilder>().ToList(),
                DataObjectName = dataObjectName,
                Parameters = parameters.Cast<IQueryParameterBuilder>().ToList()
            };

            // Assert
            sut.Conditions.Should().BeEquivalentTo(conditions);
            sut.Distinct.Should().Be(distinct);
            sut.Fields.Should().BeEquivalentTo(fields);
            sut.GetAllFields.Should().Be(getAllFields);
            sut.GroupByFields.Should().BeEquivalentTo(groupByFields);
            sut.HavingFields.Should().BeEquivalentTo(havingFields);
            sut.Limit.Should().Be(limit);
            sut.Offset.Should().Be(offset);
            sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
            sut.DataObjectName.Should().Be(dataObjectName);
            sut.Parameters.Should().BeEquivalentTo(parameters);
        }

        [Fact]
        public void Can_Build_Entity_From_Builder()
        {
            // Arrange
            var conditions = new[] { new QueryConditionBuilder("field", QueryOperator.Equal, "value") };
            var orderByFields = new[] { new QuerySortOrderBuilder("field") };
            var limit = 1;
            var offset = 2;
            var distinct = true;
            var getAllFields = true;
            var fields = new[] { new QueryExpressionBuilder("field") };
            var groupByFields = new[] { new QueryExpressionBuilder("field") };
            var havingFields = new[] { new QueryConditionBuilder("field", QueryOperator.Equal, "value") };
            var dataObjectName = "dataObjectName";
            var parameters = new[] { new QueryParameterBuilder("name", "value") };
            var sut = new AdvancedSingleDataObjectQueryBuilder
            {
                Conditions = conditions.Cast<IQueryConditionBuilder>().ToList(),
                OrderByFields = orderByFields.Cast<IQuerySortOrderBuilder>().ToList(),
                Limit = limit,
                Offset = offset,
                Distinct = distinct,
                GetAllFields = getAllFields,
                Fields = fields.Cast<IQueryExpressionBuilder>().ToList(),
                GroupByFields = groupByFields.Cast<IQueryExpressionBuilder>().ToList(),
                HavingFields = havingFields.Cast<IQueryConditionBuilder>().ToList(),
                DataObjectName = dataObjectName,
                Parameters = parameters.Cast<IQueryParameterBuilder>().ToList()
            };

            // Act
            var actual = sut.Build();

            // Assert
            actual.Should().NotBeNull();
            actual.Distinct.Should().Be(sut.Distinct);
            actual.GetAllFields.Should().Be(sut.GetAllFields);
            actual.Limit.Should().Be(sut.Limit);
            actual.Offset.Should().Be(sut.Offset);
            actual.Conditions.Should().HaveCount(sut.Conditions.Count);
            actual.Fields.Should().HaveCount(sut.Fields.Count);
            actual.GroupByFields.Should().HaveCount(sut.GroupByFields.Count);
            actual.HavingFields.Should().HaveCount(sut.HavingFields.Count);
            actual.OrderByFields.Should().HaveCount(sut.OrderByFields.Count);
            actual.DataObjectName.Should().Be(sut.DataObjectName);
            actual.Parameters.Should().HaveCount(sut.Parameters .Count);
        }
    }
}
