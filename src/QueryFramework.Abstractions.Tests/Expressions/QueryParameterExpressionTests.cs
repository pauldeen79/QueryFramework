﻿namespace QueryFramework.Abstractions.Tests.Expressions;

public class QueryParameterExpressionTests : TestBase
{
    public class Constructor : QueryParameterExpressionTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            // Act & Assert
            Action a = () => _ = new QueryParameterExpression(parameterName: null!);
            a.ShouldThrow<ValidationException>();
        }

        [Fact]
        public void Throws_On_Empty_Argument()
        {
            // Act & Assert
            Action a = () => _ = new QueryParameterExpression(parameterName: string.Empty);
            a.ShouldThrow<ValidationException>();
        }

        [Fact]
        public void Fills_ParameterName_Correctly_On_Succesful_Construction()
        {
            // Act
            var sut = new QueryParameterExpression("Test");

            // Assert
            sut.ParameterName.ShouldBe("Test");
        }
    }

    public class Evaluate : QueryParameterExpressionTests
    {
        [Fact]
        public void Returns_Invalid_When_Context_Is_Not_Of_Type_ParameterizedQuery()
        {
            // Arrange
            var sut = new QueryParameterExpression("UnknownName");
            var context = this;

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Context should be of type IParameterizedQuery");
        }

        [Fact]
        public void Returns_Invalid_When_Context_Is_Type_ParameterizedQuery_But_Parameter_Could_Not_Be_Found()
        {
            // Arrange
            var sut = new QueryParameterExpression("UnknownName");
            var context = Fixture.Freeze<IParameterizedQuery>();
            var parameter = Fixture.Freeze<IQueryParameter>();
            parameter.Name.Returns("ParameterName");
            parameter.Value.Returns("Value");
            context.Parameters.Returns(new[] { parameter }.ToList().AsReadOnly());

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Parameter with name [UnknownName] could not be found");
        }

        [Fact]
        public void Returns_Ok_When_Context_Is_Type_ParameterizedQuery_And_Parameter_Could_Be_Found()
        {
            // Arrange
            var sut = new QueryParameterExpression("ParameterName");
            var context = Fixture.Freeze<IParameterizedQuery>();
            var parameter = Fixture.Freeze<IQueryParameter>();
            parameter.Name.Returns("ParameterName");
            parameter.Value.Returns("Value");
            context.Parameters.Returns(new[] { parameter }.ToList().AsReadOnly());

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo("Value");
        }
    }

    public class ToBuilder : QueryParameterExpressionTests
    {
        [Fact]
        public void Returns_Builder_Instance_Correctly()
        {
            // Arrange
            var sut = new QueryParameterExpression("ParameterName");

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<QueryParameterExpressionBuilder>();
            ((QueryParameterExpressionBuilder)result).ParameterName.ShouldBe("ParameterName");
        }
    }
}
