namespace QueryFramework.SqlServer.Tests.Repositories;

public class TestRepositoryTests : TestBase<TestRepository>
{
    [Fact]
    public void Can_Add_Entity()
    {
        // Arrange
        Fixture.Freeze<IDatabaseCommandProcessor<TestEntity>>()
            .ExecuteCommand(Arg.Any<IDatabaseCommand>(), Arg.Any<TestEntity>())
            .Returns(x => { x.ArgAt<TestEntity>(1).Id = 1; return new DatabaseCommandResult<TestEntity>(x.ArgAt<TestEntity>(1)); });
        var entity = new TestEntity { Name = "Test" };

        // Act
        entity = Sut.Add(entity);

        // Assert
        entity.ShouldNotBeNull();
        if (entity is not null)
        {
            entity.Id.ShouldBe(1);
        }
    }

    [Fact]
    public void Can_Update_Entity()
    {
        // Arrange
        Fixture.Freeze<IDatabaseCommandProcessor<TestEntity>>()
            .ExecuteCommand(Arg.Any<IDatabaseCommand>(), Arg.Any<TestEntity>())
            .Returns(x => { x.ArgAt<TestEntity>(1).Id = 1; return new DatabaseCommandResult<TestEntity>(x.ArgAt<TestEntity>(1)); });
        var entity = new TestEntity { Name = "Test" };

        // Act
        entity = Sut.Update(entity);

        // Assert
        entity.ShouldNotBeNull();
        if (entity is not null)
        {
            entity.Id.ShouldBe(1);
        }
    }

    [Fact]
    public void Can_Delete_Entity()
    {
        // Arrange
        Fixture.Freeze<IDatabaseCommandProcessor<TestEntity>>()
            .ExecuteCommand(Arg.Any<IDatabaseCommand>(), Arg.Any<TestEntity>())
            .Returns(x => { x.ArgAt<TestEntity>(1).Id = 2; return new DatabaseCommandResult<TestEntity>(x.ArgAt<TestEntity>(1)); });
        var entity = new TestEntity { Id = 1, Name = "Test" };

        // Act
        entity = Sut.Delete(entity);

        // Assert
        entity.ShouldNotBeNull();
        if (entity is not null)
        {
            entity.Id.ShouldBe(2);
        }
    }

    [Fact]
    public void Can_Find_Entity()
    {
        // Arrange
        Fixture.Freeze<IDatabaseEntityRetriever<TestEntity>>()
            .FindOne(Arg.Any<IDatabaseCommand>())
            .Returns(new TestEntity { Id = 1, Name = "Test" });

        // Act
        var entity = Sut.Find(new TestEntityIdentity { Id = 1 });

        // Assert
        entity.ShouldNotBeNull();
        if (entity is not null)
        {
            entity.Id.ShouldBe(1);
            entity.Name.ShouldBe("Test");
        }
    }

    [Fact]
    public void Can_FindUsingCommand()
    {
        // Arrange
        Fixture.Freeze<IDatabaseEntityRetriever<TestEntity>>()
            .FindOne(Arg.Any<IDatabaseCommand>())
            .Returns(new TestEntity { Id = 1, Name = "Test" });

        // Act
        var entity = Sut.FindUsingCommand(new TestEntityIdentity { Id = 1 });

        // Assert
        entity.ShouldNotBeNull();
        if (entity is not null)
        {
            entity.Id.ShouldBe(1);
            entity.Name.ShouldBe("Test");
        }
    }
}
