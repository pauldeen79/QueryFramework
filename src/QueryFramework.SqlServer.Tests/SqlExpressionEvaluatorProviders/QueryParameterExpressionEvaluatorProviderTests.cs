namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class QueryParameterExpressionEvaluatorProviderTests : TestBase<QueryParameterExpressionEvaluatorProvider>
{
    public class TryGetSqlExpression : QueryParameterExpressionEvaluatorProviderTests
    {
        [Fact]
        public void Returns_Null_When_Expression_Is_Not_Of_Type_QueryParameterExpression()
        {
            // Arrange
            var sut = Sut;
            var expression = new EmptyExpressionBuilder().Build();
            var queryMock = Fixture.Freeze<IQuery>();
            var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
            var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();
            var parameterBag = new ParameterBag();

            // Act
            var actual = sut.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result);

            // Assert
            actual.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void Returns_Null_When_Query_Is_Not_Of_Type_ParameterizedQuery()
        {
            // Arrange
            var sut = Sut;
            var expression = new QueryParameterExpressionBuilder().WithParameterName("MyParameter").Build();
            var queryMock = Fixture.Freeze<IQuery>();
            var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
            var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();
            var parameterBag = new ParameterBag();

            // Act
            var actual = sut.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result);

            // Assert
            actual.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void Returns_Null_When_Query_Does_Not_Contain_The_Requested_Parameter()
        {
            // Arrange
            var sut = Sut;
            var expression = new QueryParameterExpressionBuilder().WithParameterName("WrongParameter").Build();
            var queryMock = Fixture.Freeze<IParameterizedQuery>();
            var parameter = Fixture.Freeze<IQueryParameter>();
            parameter.Name.Returns("MyParameter");
            parameter.Value.Returns("Value");
            queryMock.Parameters.Returns(new[] { parameter }.ToList().AsReadOnly());
            var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
            var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();
            var parameterBag = new ParameterBag();

            // Act
            var actual = sut.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result);

            // Assert
            actual.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void Returns_Correct_Value_When_Query_Does_Contains_The_Requested_Parameter()
        {
            // Arrange
            var sut = Sut;
            var expression = new QueryParameterExpressionBuilder().WithParameterName("MyParameter").Build();
            var queryMock = Fixture.Freeze<IParameterizedQuery>();
            var parameter = Fixture.Freeze<IQueryParameter>();
            parameter.Name.Returns("MyParameter");
            parameter.Value.Returns("Value");
            queryMock.Parameters.Returns(new[] { parameter }.ToList().AsReadOnly());
            var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
            var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();
            var parameterBag = new ParameterBag();

            // Act
            var actual = sut.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result);

            // Assert
            actual.Should().BeTrue();
            result.Should().Be("@p0");
            parameterBag.Parameters.Select(x => x.Value).Should().BeEquivalentTo(new object[] { "Value" });
        }
    }

    public class TryGetLengthExpression : QueryParameterExpressionEvaluatorProviderTests
    {
        [Fact]
        public void Returns_Null_When_Expression_Is_Not_Of_Type_QueryParameterExpression()
        {
            // Arrange
            var sut = Sut;
            var expression = new EmptyExpressionBuilder().Build();
            var queryMock = Fixture.Freeze<IQuery>();
            var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
            var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();

            // Act
            var actual = sut.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result);

            // Assert
            actual.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void Returns_Null_When_Query_Is_Not_Of_Type_ParameterizedQuery()
        {
            // Arrange
            var sut = Sut;
            var expression = new QueryParameterExpressionBuilder().WithParameterName("MyParameter").Build();
            var queryMock = Fixture.Freeze<IQuery>();
            var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
            var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();

            // Act
            var actual = sut.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result);

            // Assert
            actual.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void Returns_Null_When_Query_Does_Not_Contain_The_Requested_Parameter()
        {
            // Arrange
            var sut = Sut;
            var expression = new QueryParameterExpressionBuilder().WithParameterName("WrongParameter").Build();
            var queryMock = Fixture.Freeze<IParameterizedQuery>();
            var parameter = Fixture.Freeze<IQueryParameter>();
            parameter.Name.Returns("MyParameter");
            parameter.Value.Returns("Value");
            queryMock.Parameters.Returns(new[] { parameter }.ToList().AsReadOnly());
            var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
            var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();

            // Act
            var actual = sut.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result);

            // Assert
            actual.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void Returns_Correct_Value_When_Query_Does_Contains_The_Requested_Parameter()
        {
            // Arrange
            var sut = Sut;
            var expression = new QueryParameterExpressionBuilder().WithParameterName("MyParameter").Build();
            var queryMock = Fixture.Freeze<IParameterizedQuery>();
            var parameter = Fixture.Freeze<IQueryParameter>();
            parameter.Name.Returns("MyParameter");
            parameter.Value.Returns("Value");
            queryMock.Parameters.Returns(new[] { parameter }.ToList().AsReadOnly());
            var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
            var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();

            // Act
            var actual = sut.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result);

            // Assert
            actual.Should().BeTrue();
            result.Should().Be("Value".Length.ToString());
        }
    }
}
