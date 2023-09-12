namespace QueryFramework.SqlServer.Tests;

public class DefaultDatabaseEntityRetrieverFactoryTests
{
    [Fact]
    public void Create_Throws_When_Provider_Returns_Null_Result()
    {
        // Arrange
        var providerMock = new DatabaseEntityRetrieverProviderMock<object>();
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IDatabaseEntityRetriever<object>?>(_ => null);
        var sut = new DefaultDatabaseEntityRetrieverFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.Create<object>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Database entity retriever provider of data type [System.Object] for query type [QueryFramework.Core.Queries.SingleEntityQuery] provided an empty result");
    }

    [Fact]
    public void Create_Throws_When_No_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new DatabaseEntityRetrieverProviderMock<object>();
        providerMock.ReturnValue = false;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IDatabaseEntityRetriever<object>?>(_ => null);
        var sut = new DefaultDatabaseEntityRetrieverFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.Create<object>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Data type [System.Object] does not have a database entity retriever provider for query type [QueryFramework.Core.Queries.SingleEntityQuery]");
    }

    [Fact]
    public void Create_Returns_Result_Of_Provider_When_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new DatabaseEntityRetrieverProviderMock<object>();
        var settingsMock = Substitute.For<IDatabaseEntityRetriever<object>>();
        var settings = settingsMock;
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IDatabaseEntityRetriever<object>?>(_ => settings);
        var sut = new DefaultDatabaseEntityRetrieverFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        var actual = sut.Create<object>(query);

        // Assert
        actual.Should().BeSameAs(settings);
    }
}
