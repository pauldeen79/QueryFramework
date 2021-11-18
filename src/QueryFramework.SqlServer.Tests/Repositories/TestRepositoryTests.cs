using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepositoryTests
    {
        private Mock<IDatabaseCommandProcessor<TestEntity>> CommandProcessorMock { get; }
        private Mock<IDatabaseEntityRetriever<TestEntity>> RetrieverMock { get; }
        private IQueryProcessor<ITestQuery, TestEntity> QueryProcessor { get; }
        private IEnumerable<TestEntity> SourceData { get; set; } = Enumerable.Empty<TestEntity>();
        private TestRepository Sut => new TestRepository(CommandProcessorMock.Object,
                                                         RetrieverMock.Object,
                                                         QueryProcessor);

        public TestRepositoryTests()
        {
            CommandProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            RetrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
            QueryProcessor = new InMemory.QueryProcessor<ITestQuery, TestEntity>(() => SourceData);
        }

        [Fact]
        public void Can_Add_Entity()
        {
            // Arrange
            CommandProcessorMock.Setup(x => x.InvokeCommand(It.IsAny<TestEntity>(), DatabaseOperation.Insert))
                                .Returns<TestEntity, DatabaseOperation>((x, _) => { x.Id = 1; return new DatabaseCommandResult<TestEntity>(x); });
            SourceData = new[] { new TestEntity { Id = 1, Name = "Test" } };
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
            CommandProcessorMock.Setup(x => x.InvokeCommand(It.IsAny<TestEntity>(), DatabaseOperation.Update))
                                .Returns<TestEntity, DatabaseOperation>((x, _) => { x.Id = 1; return new DatabaseCommandResult<TestEntity>(x); });
            SourceData = new[] { new TestEntity { Id = 1, Name = "Test" } };
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
            CommandProcessorMock.Setup(x => x.InvokeCommand(It.IsAny<TestEntity>(), DatabaseOperation.Delete))
                                .Returns<TestEntity, DatabaseOperation>((x, _) => { x.Id = 2; return new DatabaseCommandResult<TestEntity>(x); });
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
            SourceData = new[] { new TestEntity { Id = 1, Name = "Test" } };
            
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
            SourceData = new[] { new TestEntity { Id = 1, Name = "Test" } };

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
            SourceData = new[]
            {
                new TestEntity { Id = 1, Name = "Test" },
                new TestEntity { Id = 2, Name = "Test" }
            };

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
            SourceData = new[]
            {
                new TestEntity { Id = 1, Name = "Test" },
                new TestEntity { Id = 2, Name = "Test" }
            };

            // Act
            var entities = Sut.FindPaged(new Mock<ITestQuery>().Object);

            // Assert
            entities.Should().HaveCount(2);
            entities.First().Id.Should().Be(1);
            entities.Last().Id.Should().Be(2);
        }
    }
}
