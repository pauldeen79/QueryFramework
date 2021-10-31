using System;
using System.Collections.Generic;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Abstractions;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Abstractions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public sealed class TestRepositoryTests : IDisposable
    {
        private DbConnection Connection { get; }
        private IQueryProcessor<ITestQuery, TestEntity> QueryProcessor { get; set; }
        private IDataReaderMapper<TestEntity> Mapper { get; }
        private IEnumerable<TestEntity> SourceData { get; set; }
        private TestRepository Sut => new TestRepository(Connection, QueryProcessor, Mapper);

        public TestRepositoryTests()
        {
            Connection = new DbConnection();
            Mapper = new TestEntityMapper();
            
            // Only needed to satisfy compiler. Both are filled in SetupSourceData, but this is not detected...
            QueryProcessor = new Mock<IQueryProcessor<ITestQuery, TestEntity>>().Object; // overwritten in 
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
            entity.Id.Should().Be(1);
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
            entity.Id.Should().Be(1);
        }

        [Fact]
        public void Can_Delete_Entity()
        {
            // Arrange
            Connection.AddResultForNonQueryCommand(1);
            var entity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            entity = Sut.Delete(entity);

            // Assert
            entity.Id.Should().Be(2);
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
            entity.Id.Should().Be(1);
            entity.Name.Should().Be("Test");
        }

        [Fact]
        public void Can_FindOne_Entity()
        {
            // Arrange
            SetupSourceData(new[] { new TestEntity { Id = 1, Name = "Test" } });

            // Act
            var entity = Sut.FindOne(new Mock<IDatabaseCommand>().Object);

            // Assert
            entity.Should().NotBeNull();
            entity.Id.Should().Be(1);
            entity.Name.Should().Be("Test");
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
            var entities = Sut.FindMany(new Mock<IDatabaseCommand>().Object);

            // Assert
            entities.Should().HaveCount(2);
            entities.First().Should().NotBeNull();
            entities.First().Id.Should().Be(1);
            entities.Last().Should().NotBeNull();
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
            entities.First().Should().NotBeNull();
            entities.First().Id.Should().Be(1);
            entities.Last().Should().NotBeNull();
            entities.Last().Id.Should().Be(2);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        private void SetupSourceData(IEnumerable<TestEntity> data)
        {
            // For Add/Update/Delete
            Connection.AddResultForDataReader(data);

            // For Query
            SourceData = data;
            QueryProcessor = new InMemory.QueryProcessor<ITestQuery, TestEntity>(SourceData);
        }
    }
}
