namespace QueryFramework.SqlServer.Tests;

public abstract class TestBase<T>
{
    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());
    private T? _sut;
    protected T Sut
    {
        get
        {
            if (_sut is null)
            {
                _sut = Fixture.Create<T>();
            }

            return _sut;
        }
    }
}
