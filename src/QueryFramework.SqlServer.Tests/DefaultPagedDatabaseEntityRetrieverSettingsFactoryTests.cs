namespace QueryFramework.SqlServer.Tests;

public class DefaultPagedDatabaseEntityRetrieverSettingsFactoryTests
{
    [Fact]
    public void Create_Throws_When_Provider_Returns_Null_Result()
    {
        // Arrange
        var providerMock = new PagedDatabaseEntityRetrieverSettingsProviderMock();
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IPagedDatabaseEntityRetrieverSettings?>(_ => null);
        var sut = new DefaultPagedDatabaseEntityRetrieverSettingsFactory(new[] { providerMock });
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        sut.Invoking(x => x.Create(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Paged database entity retriever settings provider of type [QueryFramework.SqlServer.Tests.TestHelpers.PagedDatabaseEntityRetrieverSettingsProviderMock] provided an empty result");
    }

    [Fact]
    public void Create_Throws_When_No_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new PagedDatabaseEntityRetrieverSettingsProviderMock();
        providerMock.ReturnValue = false;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IPagedDatabaseEntityRetrieverSettings?>(_ => null);
        var sut = new DefaultPagedDatabaseEntityRetrieverSettingsFactory(new[] { providerMock });
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        sut.Invoking(x => x.Create(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Query type [QueryFramework.Core.Queries.SingleEntityQuery] does not have a paged database entity retriever settings provider");
    }

    [Fact]
    public void Create_Returns_Result_Of_Provider_When_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new PagedDatabaseEntityRetrieverSettingsProviderMock();
        var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
        var settings = settingsMock.Object;
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IPagedDatabaseEntityRetrieverSettings?>(_ => settings);
        var sut = new DefaultPagedDatabaseEntityRetrieverSettingsFactory(new[] { providerMock });
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        var actual = sut.Create(query);

        // Assert
        actual.Should().BeSameAs(settings);
    }
}
