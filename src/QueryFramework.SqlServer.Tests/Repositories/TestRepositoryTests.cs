using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepositoryTests
    {
        [Fact]
        public void Can_Add_Entity()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new TestEntity { Id = 1, Name = "Test" } });
            var sut = new TestRepository(connection);
            var entity = new TestEntity { Name = "Test" };

            // Act
            entity = sut.Add(entity);

            // Assert
            entity.Id.Should().Be(1);
        }

        [Fact]
        public void Can_Update_Entity()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new TestEntity { Id = 1, Name = "Test" } });
            var sut = new TestRepository(connection);
            var entity = new TestEntity { Name = "Test" };

            // Act
            entity = sut.Update(entity);

            // Assert
            entity.Id.Should().Be(1);
        }

        [Fact]
        public void Can_Delete_Entity()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForNonQueryCommand(1);
            var sut = new TestRepository(connection);
            var entity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            entity = sut.Delete(entity);

            // Assert
            entity.Id.Should().Be(2);
        }

        [Fact]
        public void Can_Find_Entity()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new TestEntity { Id = 1, Name = "Test" } });
            var sut = new TestRepository(connection);
            
            // Act
            var entity = sut.Find(new TestEntityIdentity { Id = 1 });

            // Assert
            entity.Id.Should().Be(1);
            entity.Name.Should().Be("Test");
        }

        [Fact]
        public void Can_FindOne_Entity()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new TestEntity { Id = 1, Name = "Test" } });
            var sut = new TestRepository(connection);

            // Act
            var entity = sut.FindOne(new Mock<IDatabaseCommand>().Object);

            // Assert
            entity.Id.Should().Be(1);
            entity.Name.Should().Be("Test");
        }

        [Fact]
        public void Can_FindMany_Entity()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[]
            {
                new TestEntity { Id = 1, Name = "Test" },
                new TestEntity { Id = 2, Name = "Test" }
            });
            var sut = new TestRepository(connection);

            // Act
            var entities = sut.FindMany(new Mock<IDatabaseCommand>().Object);

            // Assert
            entities.Should().HaveCount(2);
            entities.First().Id.Should().Be(1);
            entities.Last().Id.Should().Be(2);
        }

        [Fact]
        public void Can_Query_Entity()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[]
            {
                new TestEntity { Id = 1, Name = "Test" },
                new TestEntity { Id = 2, Name = "Test" }
            });
            var sut = new TestRepository(connection);

            // Act
            var entities = sut.Execute(new Mock<ITestQuery>().Object);

            // Assert
            entities.Should().HaveCount(2);
            entities.First().Id.Should().Be(1);
            entities.Last().Id.Should().Be(2);
        }
    }
}
