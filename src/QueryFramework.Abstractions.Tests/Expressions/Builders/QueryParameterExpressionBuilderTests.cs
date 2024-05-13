namespace QueryFramework.Abstractions.Tests.Expressions.Builders;

public class QueryParameterExpressionBuilderTests
{
    public class DefaultConstructor
    {
        [Fact]
        public void Constructs_Correctly()
        {
            // Act
            var sut = new QueryParameterExpressionBuilder();

            // Assert
            sut.Should().NotBeNull();
        }
    }

    public class CopyConstrucotr
    {
        [Fact]
        public void Throws_On_Null_Instance()
        {
            // Act & Assert
            this.Invoking(_ => new QueryParameterExpressionBuilder(source: null!))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("source");

        }
    }

    public class WithParameterName
    {
        [Fact]
        public void Throws_On_Null_Value()
        {
            // Arrange
            var sut = new QueryParameterExpressionBuilder();

            // Act & Assert
            sut.Invoking(x => _ = x.WithParameterName(parameterName: null!))
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("parameterName");
        }
    }
}
