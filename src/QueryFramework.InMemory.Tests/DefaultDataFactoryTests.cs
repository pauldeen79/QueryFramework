namespace QueryFramework.InMemory.Tests;

public class DefaultDataFactoryTests
{
    [Fact]
    public void Create_Throws_When_Provider_Returns_Null_Result()
    {
        // Arrange
        var providerMock = new DataProviderMock();
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IEnumerable<object>?>(_ => null);
        var sut = new DefaultDataFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.GetData<object>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Data provider of type [QueryFramework.InMemory.Tests.TestHelpers.DataProviderMock] for data type [System.Object] provided an empty result");
    }

    [Fact]
    public void Create_Throws_When_No_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new DataProviderMock();
        providerMock.ReturnValue = false;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IEnumerable<object>?>(_ => null);
        var sut = new DefaultDataFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.GetData<object>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Query type [QueryFramework.Core.Queries.SingleEntityQuery] for data type [System.Object] does not have a data provider");
    }

    [Fact]
    public void Create_Returns_Result_Of_Provider_When_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new DataProviderMock();
        var data = Enumerable.Empty<object>();
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IEnumerable<object>?>(_ => data);
        var sut = new DefaultDataFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        var actual = sut.GetData<object>(query);

        // Assert
        actual.Should().BeSameAs(data);
    }
}
