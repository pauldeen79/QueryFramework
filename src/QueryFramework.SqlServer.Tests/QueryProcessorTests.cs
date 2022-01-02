using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Commands;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.SqlServer.Tests.TestHelpers;
using Xunit;

namespace QueryFramework.SqlServer.Tests
{
    [ExcludeFromCodeCoverage]
    public class QueryProcessorTests : TestBase<QueryProcessor<ISingleEntityQuery, MyEntity>>
    {
        public QueryProcessorTests()
        {
            Fixture.Freeze<Mock<IPagedDatabaseCommandProvider<ISingleEntityQuery>>>()
                   .Setup(x => x.CreatePaged(It.IsAny<ISingleEntityQuery>(), DatabaseOperation.Select, It.IsAny<int>(), It.IsAny<int>()))
                   .Returns(new PagedDatabaseCommand(new SqlTextCommand("SELECT ...", DatabaseOperation.Select),
                                                     new SqlTextCommand("SELECT COUNT(*)...", DatabaseOperation.Select),
                                                     0,
                                                     0));
        }

        [Fact]
        public void FindPaged_Returns_MappedEntities()
        {
            // Arrange
            SetupSourceData(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = Sut.FindPaged(new SingleEntityQuery());

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
            var query = new SingleEntityQueryBuilder().Take(1).Build();

            // Act
            var actual = Sut.FindPaged(query);

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
            var actual = Sut.FindOne(new SingleEntityQueryBuilder().Where("Property".IsEqualTo("Some value")).Build());

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
            var actual = Sut.FindMany(new SingleEntityQuery());

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
        }

        private void SetupSourceData(IEnumerable<MyEntity> data, int? totalRecordCount = null)
        {
            var retrieverMock = Fixture.Freeze<Mock<IDatabaseEntityRetriever<MyEntity>>>();

            // For FindOne/FindMany
            retrieverMock.Setup(x => x.FindOne(It.IsAny<IDatabaseCommand>())).Returns(data.FirstOrDefault());
            retrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>())).Returns(data.ToList());

            // For FindPaged
            retrieverMock.Setup(x => x.FindPaged(It.IsAny<IPagedDatabaseCommand>()))
                                      .Returns<IPagedDatabaseCommand>(command => new PagedResult<MyEntity>(data, totalRecordCount ?? data.Count(), command.Offset, command.PageSize));
        }
    }
}
