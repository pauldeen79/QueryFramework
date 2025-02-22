namespace QueryFramework.InMemory.Tests;

public class DefaultDataFactoryTests
{
    [Fact]
    public void Create_Throws_When_Provider_Returns_Null_Result()
    {
        // Arrange
        var providerMock = new DataProviderMock();
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<IQuery, IEnumerable<object>?>(_ => null);
        var sut = new DefaultDataFactory(new[] { providerMock }, Enumerable.Empty<IContextDataProvider>());
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        Action a = () => sut.GetData<object>(query);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Data provider of type [QueryFramework.InMemory.Tests.TestHelpers.DataProviderMock] for data type [System.Object] provided an empty result");
    }

    [Fact]
    public void Create_Throws_When_Context_Aware_Provider_Returns_Null_Result()
    {
        // Arrange
        var providerMock = new ContextDataProviderMock();
        providerMock.ReturnValue = true;
        providerMock.ContextResultDelegate = new Func<IQuery, object?, IEnumerable<object>?>((_, _) => null);
        var sut = new DefaultDataFactory(Enumerable.Empty<IDataProvider>(), new[] { providerMock });
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        Action a = () => sut.GetData<object>(query, default);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Data provider of type [QueryFramework.InMemory.Tests.TestHelpers.ContextDataProviderMock] for data type [System.Object] provided an empty result");
    }

    [Fact]
    public void Create_Throws_When_No_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new DataProviderMock();
        providerMock.ReturnValue = false;
        providerMock.ResultDelegate = new Func<IQuery, IEnumerable<object>?>(_ => null);
        var sut = new DefaultDataFactory(new[] { providerMock }, Enumerable.Empty<IContextDataProvider>());
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        Action a = () => sut.GetData<object>(query);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Query type [QueryFramework.Core.Queries.SingleEntityQuery] for data type [System.Object] does not have a data provider");
    }

    [Fact]
    public void Create_Returns_Result_Of_Provider_When_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new DataProviderMock();
        var data = Enumerable.Empty<object>();
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<IQuery, IEnumerable<object>?>(_ => data);
        var sut = new DefaultDataFactory(new[] { providerMock }, Enumerable.Empty<IContextDataProvider>());
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        var actual = sut.GetData<object>(query);

        // Assert
        actual.ShouldBeSameAs(data);
    }

    [Fact]
    public void Create_Returns_Result_Of_Provider_With_Context_When_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new ContextDataProviderMock();
        var data = Enumerable.Empty<object>();
        providerMock.ReturnValue = true;
        providerMock.ContextResultDelegate = new Func<IQuery, object?, IEnumerable<object>?>((_, ctx) => ctx as IEnumerable<object> ?? Enumerable.Range(1, 10).Cast<object>());
        var sut = new DefaultDataFactory(Enumerable.Empty<IDataProvider>(), new[] { providerMock });
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        var actual = sut.GetData<object>(query, data);

        // Assert
        actual.ShouldBeSameAs(data);
    }
}
