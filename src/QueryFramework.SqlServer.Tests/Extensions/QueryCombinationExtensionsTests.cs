using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class QueryCombinationExtensionsTests
    {
        [Fact]
        public void ToSql_Throws_On_Invalid_QueryCombination()
        {
            // Act & Assert
            ((QueryCombination)99).Invoking(x => x.ToSql())
                                  .Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(QueryCombination.And, "AND")]
        [InlineData(QueryCombination.Or, "OR")]
        public void ToSql_Converts_Valid_QueryCombination_Correctly(QueryCombination input, string expectedOutput)
        {
            // Act
            var actual = input.ToSql();

            // Assert
            actual.Should().Be(expectedOutput);
        }
    }
}
