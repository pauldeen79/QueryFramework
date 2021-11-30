using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Core;
using QueryFramework.SqlServer.Tests.Repositories;
using Xunit;

namespace QueryFramework.SqlServer.Tests
{
    [ExcludeFromCodeCoverage]
    public class IntegrationTests
    {
        private QueryProcessor<TestQuery, TestEntity> Sut { get; }
        public Mock<IDatabaseEntityRetriever<TestEntity>> RetrieverMock { get; }

        public IntegrationTests()
        {
            RetrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
            var settings = new PagedDatabaseEntityRetrieverSettings("MyTable", "", "", "", null);
            Sut = new QueryProcessor<TestQuery, TestEntity>
            (
                RetrieverMock.Object,
                new QueryPagedDatabaseCommandProvider<TestQuery>(new DefaultQueryFieldProvider(), settings)
            );
        }

        [Fact]
        public void Can_Query_All_Records()
        {
            // Arrange
            var query = new TestQuery();
            var expectedResult = new[] { new TestEntity(), new TestEntity() };
            RetrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>()))
                         .Returns(expectedResult);

            // Act
            var actual = Sut.FindMany(query);

            // Assert
            actual.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Can_Query_Filtered_Records()
        {
            // Arrange
            var query = new TestQuery(null, null, new[] { new QueryCondition("Name", QueryOperator.Equal, "Test") }, Enumerable.Empty<IQuerySortOrder>());
            var expectedResult = new[] { new TestEntity(), new TestEntity() };
            RetrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>()))
                         .Returns(expectedResult);

            // Act
            var actual = Sut.FindMany(query);

            // Assert
            actual.Should().BeEquivalentTo(expectedResult);
        }
    }
}
