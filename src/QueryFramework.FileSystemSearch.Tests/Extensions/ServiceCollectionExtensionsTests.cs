namespace QueryFramework.FileSystemSearch.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void All_Dependencies_Can_Be_Resolved()
    {
        // Act
        var action = new Action(() => _ = new ServiceCollection().AddFileSystemSearch()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true }));

        // Assert
        action.Should().NotThrow();
    }
}
