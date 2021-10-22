﻿using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
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
            var sut = new SingleEntityQuery(conditions, orderByFields, limit, offset);

            // Assert
            sut.Conditions.Should().BeEquivalentTo(conditions);
            sut.Limit.Should().Be(limit);
            sut.Offset.Should().Be(offset);
            sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
        }
    }
}
