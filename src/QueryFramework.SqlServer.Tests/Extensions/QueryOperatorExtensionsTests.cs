using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class QueryOperatorExtensionsTests
    {
        [Fact]
        public void ToSql_Throws_On_Invalid_QueryOperator()
        {
            // Act & Assert
            ((QueryOperator)99).Invoking(x => x.ToSql())
                               .Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(QueryOperator.Contains)]
        [InlineData(QueryOperator.EndsWith)]
        [InlineData(QueryOperator.IsNotNull)]
        [InlineData(QueryOperator.IsNotNullOrEmpty)]
        [InlineData(QueryOperator.IsNull)]
        [InlineData(QueryOperator.IsNullOrEmpty)]
        [InlineData(QueryOperator.NotContains)]
        [InlineData(QueryOperator.NotEndsWith)]
        [InlineData(QueryOperator.NotStartsWith)]
        [InlineData(QueryOperator.StartsWith)]
        public void ToSql_Throws_On_Unsupported_QueryOperator(QueryOperator input)
        {
            // Act & Assert
            input.Invoking(x => x.ToSql())
                 .Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(QueryOperator.Equal, "=")]
        [InlineData(QueryOperator.GreaterOrEqual, ">=")]
        [InlineData(QueryOperator.Greater, ">")]
        [InlineData(QueryOperator.LowerOrEqual, "<=")]
        [InlineData(QueryOperator.Lower, "<")]
        [InlineData(QueryOperator.NotEqual, "<>")]
        public void ToSql_Converts_Valid_QueryOperator_Correctly(QueryOperator input, string expectedOutput)
        {
            // Act
            var actual = input.ToSql();

            // Assert
            actual.Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData(QueryOperator.NotContains, "NOT ")]
        [InlineData(QueryOperator.NotEndsWith, "NOT ")]
        [InlineData(QueryOperator.NotStartsWith, "NOT ")]
        [InlineData(QueryOperator.Equal, "")]
        [InlineData(QueryOperator.Contains, "")]
        public void ToNot_Returns_Correct_Result(QueryOperator input, string expectedOutput)
        {
            // Act
            var actual = input.ToNot();

            // Assert
            actual.Should().Be(expectedOutput);
        }
    }
}
