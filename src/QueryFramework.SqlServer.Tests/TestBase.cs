using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace QueryFramework.SqlServer.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class TestBase<T>
    {
        protected IFixture Fixture { get; } = new Fixture().Customize(new AutoMoqCustomization());
        protected T Sut => Fixture.Create<T>();
    }
}
