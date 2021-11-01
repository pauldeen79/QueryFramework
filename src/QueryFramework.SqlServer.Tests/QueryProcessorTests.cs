using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Tests.TestHelpers;
using Xunit;

namespace QueryFramework.SqlServer.Tests
{
    [ExcludeFromCodeCoverage]
    public class QueryProcessorTests
    {
        private Mock<IDatabaseEntityRetriever<MyEntity>> RetrieverMock { get; }
        private Mock<IDataReaderMapper<MyEntity>> MapperMock { get; }
        private Mock<IQueryProcessorSettings> QueryProcessorSettingsMock { get; }
        private Mock<IDatabaseCommandGenerator> DatabaseCommandGeneratorMock { get; }
        private QueryProcessor<ISingleEntityQuery, MyEntity> Sut
            => new QueryProcessor<ISingleEntityQuery, MyEntity>(RetrieverMock.Object,
                                                                QueryProcessorSettingsMock.Object,
                                                                DatabaseCommandGeneratorMock.Object);

        public QueryProcessorTests()
        {
            RetrieverMock = new Mock<IDatabaseEntityRetriever<MyEntity>>();
            MapperMock = new Mock<IDataReaderMapper<MyEntity>>();
            MapperMock.Setup(x => x.Map(It.IsAny<IDataReader>()))
                      .Returns<IDataReader>(reader => new MyEntity { Property = reader.GetString(0) });
            QueryProcessorSettingsMock = new Mock<IQueryProcessorSettings>();
            DatabaseCommandGeneratorMock = new Mock<IDatabaseCommandGenerator>();
        }

        [Fact]
        public void FindPaged_Returns_MappedEntities()
        {
            // Arrange
            SetupSourceData(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = Sut.FindPaged(new Mock<ISingleEntityQuery>().Object);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
            actual.TotalRecordCount.Should().Be(1);
        }

        [Fact]
        public void FindPaged_Fills_TotalRecordCount_On_Paged_Query()
        {
            // Arrange
            SetupSourceData(new[] { new MyEntity { Property = "Value" } }, totalRecordCount: 10);
            var queryMock = new Mock<ISingleEntityQuery>();
            queryMock.SetupGet(x => x.Limit)
                     .Returns(1);

            // Act
            var actual = Sut.FindPaged(queryMock.Object);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
            actual.TotalRecordCount.Should().Be(10);
        }

        [Fact]
        public void FindOne_Returns_MappedEntity()
        {
            // Arrange
            SetupSourceData(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = Sut.FindOne(new Mock<ISingleEntityQuery>().Object);

            // Assert
            actual.Should().NotBeNull();
            if (actual != null)
            {
                actual.Property.Should().Be("Value");
            }
        }

        [Fact]
        public void FindMany_Returns_MappedEntities()
        {
            // Arrange
            SetupSourceData(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = Sut.FindMany(new Mock<ISingleEntityQuery>().Object);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
        }

        private void SetupSourceData(IEnumerable<MyEntity> data, int? totalRecordCount = null)
        {
            // For FindOne/FindMany
            RetrieverMock.Setup(x => x.FindOne(It.IsAny<IDatabaseCommand>())).Returns(data.FirstOrDefault());
            RetrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>())).Returns(data.ToList());

            // For FindPaged
            RetrieverMock.Setup(x => x.FindPaged(It.IsAny<IDatabaseCommand>(), It.IsAny<IDatabaseCommand>(), It.IsAny<int>(), It.IsAny<int>()))
                                      .Returns<IDatabaseCommand, IDatabaseCommand, int, int>((_, _, offset, pageSize) => CreatePagedResult(data, totalRecordCount ?? data.Count(), offset, pageSize));
        }

        private IPagedResult<MyEntity> CreatePagedResult(IEnumerable<MyEntity> data, int totalRecordCount, int offset, int pageSize)
            => new PagedResult<MyEntity>(data, totalRecordCount, offset, pageSize);
    }
}
