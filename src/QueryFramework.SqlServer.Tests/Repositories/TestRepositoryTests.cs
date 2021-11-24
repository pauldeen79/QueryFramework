﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepositoryTests : TestBase<TestRepository>
    {
        [Fact]
        public void Can_Add_Entity()
        {
            // Arrange
            Fixture.Freeze<Mock<IDatabaseCommandProcessor<TestEntity>>>()
                .Setup(x => x.ExecuteCommand(It.IsAny<IDatabaseCommand>(), It.IsAny<TestEntity>()))
                .Returns<IDatabaseCommand, TestEntity>((_, x) => { x.Id = 1; return new DatabaseCommandResult<TestEntity>(x); });
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
            Fixture.Freeze<Mock<IDatabaseCommandProcessor<TestEntity>>>()
                .Setup(x => x.ExecuteCommand(It.IsAny<IDatabaseCommand>(), It.IsAny<TestEntity>()))
                .Returns<IDatabaseCommand, TestEntity>((_, x) => { x.Id = 1; return new DatabaseCommandResult<TestEntity>(x); });
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
            Fixture.Freeze<Mock<IDatabaseCommandProcessor<TestEntity>>>()
                .Setup(x => x.ExecuteCommand(It.IsAny<IDatabaseCommand>(), It.IsAny<TestEntity>()))
                .Returns<IDatabaseCommand, TestEntity>((_, x) => { x.Id = 2; return new DatabaseCommandResult<TestEntity>(x); });
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
            Fixture.Freeze<Mock<IDatabaseEntityRetriever<TestEntity>>>()
                .Setup(x => x.FindOne(It.IsAny<IDatabaseCommand>()))
                .Returns(new TestEntity { Id = 1, Name = "Test" });

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
            Fixture.Freeze<Mock<IDatabaseEntityRetriever<TestEntity>>>()
                .Setup(x => x.FindOne(It.IsAny<IDatabaseCommand>()))
                .Returns(new TestEntity { Id = 1, Name = "Test" });

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
            Fixture.Freeze<Mock<IDatabaseEntityRetriever<TestEntity>>>()
                .Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>()))
                .Returns(new[]
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
        public void Can_FindPaged_Entities()
        {
            // Arrange
            Fixture.Freeze<Mock<IDatabaseEntityRetriever<TestEntity>>>()
                .Setup(x => x.FindPaged(It.IsAny<IPagedDatabaseCommand>()))
                .Returns(new PagedResult<TestEntity>(new[]
                {
                    new TestEntity { Id = 1, Name = "Test" },
                    new TestEntity { Id = 2, Name = "Test" }
                }, 2, 0, 10));

            // Act
            var entities = Sut.FindPaged(new Mock<ITestQuery>().Object);

            // Assert
            entities.Should().HaveCount(2);
            entities.First().Id.Should().Be(1);
            entities.Last().Id.Should().Be(2);
        }

        [Fact]
        public void Can_FindUsingCommand()
        {
            // Arrange
            Fixture.Freeze<Mock<IDatabaseEntityRetriever<TestEntity>>>()
                .Setup(x => x.FindOne(It.IsAny<IDatabaseCommand>()))
                .Returns(new TestEntity { Id = 1, Name = "Test" });

            // Act
            var entity = Sut.FindUsingCommand(new TestEntityIdentity { Id = 1 });

            // Assert
            entity.Should().NotBeNull();
            if (entity != null)
            {
                entity.Id.Should().Be(1);
                entity.Name.Should().Be("Test");
            }
        }
    }
}
