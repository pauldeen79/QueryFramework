namespace QueryFramework.SqlServer.Tests.Extensions;

public class OperatorExtensionsTests
{
    [Fact]
    public void ToSql_Throws_On_Invalid_Operator()
    {
        // Arrange
        var @operator = new UnsupportedOperator();
        
        // Act & Assert
        @operator.Invoking(x => x.ToSql())
                 .Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(typeof(StringContainsOperatorBuilder))]
    [InlineData(typeof(EndsWithOperatorBuilder))]
    [InlineData(typeof(IsNotNullOperatorBuilder))]
    [InlineData(typeof(IsNotNullOrEmptyOperatorBuilder))]
    [InlineData(typeof(IsNullOperatorBuilder))]
    [InlineData(typeof(IsNullOrEmptyOperatorBuilder))]
    [InlineData(typeof(StringNotContainsOperatorBuilder))]
    [InlineData(typeof(NotEndsWithOperatorBuilder))]
    [InlineData(typeof(NotStartsWithOperatorBuilder))]
    [InlineData(typeof(StartsWithOperatorBuilder))]
    public void ToSql_Throws_On_Unsupported_Operator(Type input)
    {
        // Arrange
        var @operator = ((OperatorBuilder)Activator.CreateInstance(input)!).Build();

        // Act & Assert
        @operator.Invoking(x => x.ToSql())
                 .Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(typeof(EqualsOperatorBuilder), "=")]
    [InlineData(typeof(IsGreaterOrEqualOperatorBuilder), ">=")]
    [InlineData(typeof(IsGreaterOperatorBuilder), ">")]
    [InlineData(typeof(IsSmallerOrEqualOperatorBuilder), "<=")]
    [InlineData(typeof(IsSmallerOperatorBuilder), "<")]
    [InlineData(typeof(NotEqualsOperatorBuilder), "<>")]
    public void ToSql_Converts_Valid_Operator_Correctly(Type input, string expectedOutput)
    {
        // Arrange
        var @operator = ((OperatorBuilder)Activator.CreateInstance(input)!).Build();

        // Act
        var actual = @operator.ToSql();

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Theory]
    [InlineData(typeof(StringNotContainsOperatorBuilder), "NOT ")]
    [InlineData(typeof(NotEndsWithOperatorBuilder), "NOT ")]
    [InlineData(typeof(NotStartsWithOperatorBuilder), "NOT ")]
    [InlineData(typeof(EqualsOperatorBuilder), "")]
    [InlineData(typeof(StringContainsOperatorBuilder), "")]
    public void ToNot_Returns_Correct_Result(Type input, string expectedOutput)
    {
        // Arrange
        var @operator = ((OperatorBuilder)Activator.CreateInstance(input)!).Build();

        // Act
        var actual  = @operator.ToNot();

        // Assert
        actual.Should().Be(expectedOutput);
    }

    private sealed record UnsupportedOperator : Operator
    {
        public UnsupportedOperator()
        {
        }

        public UnsupportedOperator(Operator original) : base(original)
        {
        }

        public override OperatorBuilder ToBuilder()
        {
            throw new NotImplementedException();
        }

        protected override Result<bool> Evaluate(object? leftValue, object? rightValue)
        {
            throw new NotImplementedException();
        }
    }
}
