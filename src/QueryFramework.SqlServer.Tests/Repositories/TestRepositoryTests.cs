using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Abstractions;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepositoryTests
    {
        private Mock<IDatabaseCommandProcessor<TestEntity>> AddProcessorMock { get; }
        private Mock<IDatabaseCommandProcessor<TestEntity>> UpdateProcessorMock { get; }
        private Mock<IDatabaseCommandProcessor<TestEntity>> DeleteProcessorMock { get; }
        private Mock<IDatabaseEntityRetriever<TestEntity>> RetrieverMock { get; }
        private IQueryProcessor<ITestQuery, TestEntity> QueryProcessor { get; set; }
        private IEnumerable<TestEntity> SourceData { get; set; }
        private TestRepository Sut => new TestRepository(AddProcessorMock.Object,
                                                         UpdateProcessorMock.Object,
                                                         DeleteProcessorMock.Object,
                                                         RetrieverMock.Object,
                                                         QueryProcessor);

        public TestRepositoryTests()
        {
            AddProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            AddProcessorMock.Setup(x => x.InvokeCommand(It.IsAny<TestEntity>())).Returns<TestEntity>(x => { x.Id = 1; return x; });
            UpdateProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            UpdateProcessorMock.Setup(x => x.InvokeCommand(It.IsAny<TestEntity>())).Returns<TestEntity>(x => { x.Id = 1; return x; });
            DeleteProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            DeleteProcessorMock.Setup(x => x.InvokeCommand(It.IsAny<TestEntity>())).Returns<TestEntity>(x => { x.Id = 2; return x; });
            RetrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
            
            // Only needed to satisfy compiler. Both are filled in SetupSourceData, but this is not detected...
            QueryProcessor = new Mock<IQueryProcessor<ITestQuery, TestEntity>>().Object;
            SourceData = Enumerable.Empty<TestEntity>();
            
            SetupSourceData(Enumerable.Empty<TestEntity>());
        }

        [Fact]
        public void Can_Add_Entity()
        {
            // Arrange
            SetupSourceData(new[] { new TestEntity { Id = 1, Name = "Test" } });
            var entity = new TestEntity { Name = "Test" };

            // Act
            entity = Sut.Add(entity);

            // Assert
            entity.Should().NotBeNull();
            if (entity != null)
            {
                entity.Id.Should().Be(1);
            }
        }

        [Fact]
        public void Can_Update_Entity()
        {
            // Arrange
            SetupSourceData(new[] { new TestEntity { Id = 1, Name = "Test" } });
            var entity = new TestEntity { Name = "Test" };

            // Act
            entity = Sut.Update(entity);

            // Assert
            entity.Should().NotBeNull();
            if (entity != null)
            {
                entity.Id.Should().Be(1);
            }
        }

        [Fact]
        public void Can_Delete_Entity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            entity = Sut.Delete(entity);

            // Assert
            entity.Should().NotBeNull();
            if (entity != null)
            {
                entity.Id.Should().Be(2);
            }
        }

        [Fact]
        public void Can_Find_Entity()
        {
            // Arrange
            SetupSourceData(new[] { new TestEntity { Id = 1, Name = "Test" } });
            
            // Act
            var entity = Sut.Find(new TestEntityIdentity { Id = 1 });

            // Assert
            entity.Should().NotBeNull();
            if (entity != null)
            {
                entity.Id.Should().Be(1);
                entity.Name.Should().Be("Test");
            }
        }

        [Fact]
        public void Can_FindOne_Entity()
        {
            // Arrange
            SetupSourceData(new[] { new TestEntity { Id = 1, Name = "Test" } });

            // Act
            var entity = Sut.FindOne(new Mock<ITestQuery>().Object);

            // Assert
            entity.Should().NotBeNull();
            if (entity != null)
            {
                entity.Id.Should().Be(1);
                entity.Name.Should().Be("Test");
            }
        }

        [Fact]
        public void Can_FindMany_Entities()
        {
            // Arrange
            SetupSourceData(new[]
            {
                new TestEntity { Id = 1, Name = "Test" },
                new TestEntity { Id = 2, Name = "Test" }
            });

            // Act
            var entities = Sut.FindMany(new Mock<ITestQuery>().Object);

            // Assert
            entities.Should().HaveCount(2);
            entities.First().Id.Should().Be(1);
            entities.Last().Id.Should().Be(2);
        }

        [Fact]
        public void Can_FindPaged_With_Query()
        {
            // Arrange
            SetupSourceData(new[]
            {
                new TestEntity { Id = 1, Name = "Test" },
                new TestEntity { Id = 2, Name = "Test" }
            });

            // Act
            var entities = Sut.FindPaged(new Mock<ITestQuery>().Object);

            // Assert
            entities.Should().HaveCount(2);
            entities.First().Id.Should().Be(1);
            entities.Last().Id.Should().Be(2);
        }

        private void SetupSourceData(IEnumerable<TestEntity> data)
        {
            // For Find
            RetrieverMock.Setup(x => x.FindOne(It.IsAny<IDatabaseCommand>())).Returns(data.FirstOrDefault());
            RetrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>())).Returns(data.ToList());

            // For FindOne, FindMany and Query
            SourceData = data;
            QueryProcessor = new InMemory.QueryProcessor<ITestQuery, TestEntity>(SourceData);
        }
    }
}
