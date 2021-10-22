using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace QueryFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public class QueryResultTests
    {
        [Fact]
        public void Can_Construct_And_Read_QueryResult_With_Null_Records()
        {
            // Arrange
            var sut = new QueryResult<MyEntity>(null, 1);

            // Act
            var actual = sut.ToList();

            // Assert
            actual.Should().BeEmpty();
            sut.TotalRecordCount.Should().Be(1);
            sut.Count.Should().Be(0);
        }

        [Fact]
        public void Can_Construct_And_Read_QueryResult_With_NotNull_Records()
        {
            // Arrange
            var sut = new QueryResult<MyEntity>(new[] { new MyEntity { Property = "1" }, new MyEntity { Property = "2" } }, 3);

            // Act
            var actual = sut.ToList();

            // Assert
            actual.Should().HaveCount(2);
            actual.ElementAt(0).Property.Should().Be("1");
            actual.ElementAt(1).Property.Should().Be("2");
            sut.TotalRecordCount.Should().Be(3);
            sut.Count.Should().Be(2);
        }

        private class MyEntity
        {
            public string Property { get; set; }
        }
    }
}
