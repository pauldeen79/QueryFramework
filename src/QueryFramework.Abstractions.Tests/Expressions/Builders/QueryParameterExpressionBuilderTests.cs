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
            sut.ShouldNotBeNull();
        }
    }

    public class CopyConstrucotr
    {
        [Fact]
        public void Throws_On_Null_Instance()
        {
            // Act & Assert
            Action a = () => _ = new QueryParameterExpressionBuilder(source: null!);
            a.ShouldThrow<ArgumentNullException>()
             .ParamName.ShouldBe("source");

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
            Action a = () => _ = sut.WithParameterName(parameterName: null!);
            a.ShouldThrow<ArgumentNullException>()
             .ParamName.ShouldBe("parameterName");
        }
    }
}
