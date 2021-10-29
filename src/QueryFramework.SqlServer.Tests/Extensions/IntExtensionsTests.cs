using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.SqlServer.Extensions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class IntExtensionsTests
    {
        [Theory]
        [InlineData(null, null, 0)]
        [InlineData(100, 10, 10)]
        [InlineData(100, 1000, 100)]
        [InlineData(0, 100, 100)]
        public void DetermineLimit_Returns_Correct_Value(int? queryLimit, int? overrideLimit, int expectedResult)
        {
            // Act
            var actual = queryLimit.DetermineLimit(overrideLimit);

            // Asset
            actual.Should().Be(expectedResult);
        }
    }
}
