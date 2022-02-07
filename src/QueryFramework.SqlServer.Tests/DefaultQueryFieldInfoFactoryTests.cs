namespace QueryFramework.SqlServer.Tests;

public class DefaultQueryFieldInfoFactoryTests
{
    [Fact]
    public void Create_Throws_When_Provider_Returns_Null_Result()
    {
        // Arrange
        var providerMock = new QueryFieldInfoProviderMock();
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IQueryFieldInfo?>(_ => null);
        var sut = new DefaultQueryFieldInfoFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.Create(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Query field info provider of type [QueryFramework.SqlServer.Tests.TestHelpers.QueryFieldInfoProviderMock] provided an empty result");
    }

    [Fact]
    public void Create_Throws_When_No_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new QueryFieldInfoProviderMock();
        providerMock.ReturnValue = false;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IQueryFieldInfo?>(_ => null);
        var sut = new DefaultQueryFieldInfoFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.Create(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Query type [QueryFramework.Core.Queries.SingleEntityQuery] does not have a query field info provider");
    }

    [Fact]
    public void Create_Returns_Result_Of_Provider_When_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new QueryFieldInfoProviderMock();
        var queryFieldInfoMock = new Mock<IQueryFieldInfo>();
        var queryFieldInfo = queryFieldInfoMock.Object;
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IQueryFieldInfo?>(_ => queryFieldInfo);
        var sut = new DefaultQueryFieldInfoFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        var actual = sut.Create(query);

        // Assert
        actual.Should().BeSameAs(queryFieldInfo);
    }
}
