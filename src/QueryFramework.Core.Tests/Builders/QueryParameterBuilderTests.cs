namespace QueryFramework.Core.Tests.Builders;

public class QueryParameterBuilderTests
{
    [Fact]
    public void Can_Create_QueryParameter_From_Builder()
    {
        // Arrange
        var sut = new QueryParameterBuilder
        {
            Name = "name",
            Value = "value"
        };

        // Act
        var actual = sut.Build();

        // Assert
        actual.Name.Should().Be(sut.Name);
        actual.Value.Should().Be(sut.Value);
    }

    [Fact]
    public void Can_Create_QueryParameterBuilder_From_QueryParameter()
    {
        // Arrange
        var input = new QueryParameter
        (
            name: "name",
            value: "value"
        );

        // Act
        var actual = new QueryParameterBuilder(input);

        // Assert
        actual.Name.Should().Be(input.Name);
        actual.Value.Should().Be(input.Value);
    }
}
