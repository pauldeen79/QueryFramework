using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Queries.Builders;
using Xunit;

namespace QueryFramework.QueryParsers.Tests
{
    [ExcludeFromCodeCoverage]
    public class SingleEntityQueryParserTests
    {
        [Theory]
        [InlineData("CONTAINS", "MyValue", QueryOperator.Contains)]
        [InlineData("ENDSWITH", "MyValue", QueryOperator.EndsWith)]
        [InlineData("\"ENDS WITH\"", "MyValue", QueryOperator.EndsWith)]
        [InlineData("=", "MyValue", QueryOperator.Equal)]
        [InlineData("==", "MyValue", QueryOperator.Equal)]
        [InlineData(">=", "MyValue", QueryOperator.GreaterOrEqual)]
        [InlineData(">", "MyValue", QueryOperator.Greater)]
        [InlineData("\"IS NOT\"", "NULL", QueryOperator.IsNotNull)]
        [InlineData("IS", "NULL", QueryOperator.IsNull)]
        [InlineData("<=", "MyValue", QueryOperator.LowerOrEqual)]
        [InlineData("<", "MyValue", QueryOperator.Lower)]
        [InlineData("NOTCONTAINS", "MyValue", QueryOperator.NotContains)]
        [InlineData("\"NOT CONTAINS\"", "MyValue", QueryOperator.NotContains)]
        [InlineData("NOTENDSWITH", "MyValue", QueryOperator.NotEndsWith)]
        [InlineData("\"NOT ENDS WITH\"", "MyValue", QueryOperator.NotEndsWith)]
        [InlineData("<>", "MyValue", QueryOperator.NotEqual)]
        [InlineData("!=", "MyValue", QueryOperator.NotEqual)]
        [InlineData("#", "MyValue", QueryOperator.NotEqual)]
        [InlineData("NOTSTARTSWITH", "MyValue", QueryOperator.NotStartsWith)]
        [InlineData("\"NOT STARTS WITH\"", "MyValue", QueryOperator.NotStartsWith)]
        [InlineData("STARTSWITH", "MyValue", QueryOperator.StartsWith)]
        [InlineData("\"STARTS WITH\"", "MyValue", QueryOperator.StartsWith)]
        public void Can_Parse_EntityQuery_With_Operator(string @operator, string value, QueryOperator expectedOperator)
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, $"MyFieldName {@operator} {value}");

            // Assert
            actual.Conditions.Should().HaveCount(1);
            actual.Conditions.First().CloseBracket.Should().BeFalse();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
            actual.Conditions.First().Field.FieldName.Should().Be("MyFieldName");
            actual.Conditions.First().OpenBracket.Should().BeFalse();
            actual.Conditions.First().Operator.Should().Be(expectedOperator);
            actual.Conditions.First().Value.Should().Be(value == "NULL" ? null : value);
        }

        [Fact]
        public void Can_Parse_EntityQuery_With_Space_In_Value()
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, $"MyFieldName = \"My Value\"");

            // Assert
            actual.Conditions.Should().HaveCount(1);
            actual.Conditions.First().CloseBracket.Should().BeFalse();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
            actual.Conditions.First().Field.FieldName.Should().Be("MyFieldName");
            actual.Conditions.First().OpenBracket.Should().BeFalse();
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Equal);
            actual.Conditions.First().Value.Should().Be("My Value");
        }

        [Theory]
        [InlineData("AND", QueryCombination.And)]
        [InlineData("OR", QueryCombination.Or)]
        public void Can_Parse_EntityQuery_With_Multiple_Items(string combination, QueryCombination secondQueryCombination)
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, $"MyFieldName = MyFirstValue {combination} MyOtherFieldName != MySecondValue");

            // Assert
            actual.Conditions.Should().HaveCount(2);
            actual.Conditions.First().CloseBracket.Should().BeFalse();
            actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
            actual.Conditions.First().Field.FieldName.Should().Be("MyFieldName");
            actual.Conditions.First().OpenBracket.Should().BeFalse();
            actual.Conditions.First().Operator.Should().Be(QueryOperator.Equal);
            actual.Conditions.First().Value.Should().Be("MyFirstValue");
            actual.Conditions.Last().CloseBracket.Should().BeFalse();
            actual.Conditions.Last().Combination.Should().Be(secondQueryCombination);
            actual.Conditions.Last().Field.FieldName.Should().Be("MyOtherFieldName");
            actual.Conditions.Last().OpenBracket.Should().BeFalse();
            actual.Conditions.Last().Operator.Should().Be(QueryOperator.NotEqual);
            actual.Conditions.Last().Value.Should().Be("MySecondValue");
        }

        [Fact]
        public void Can_Parse_EntityQuery_With_Brackets()
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, "(MyFieldName = MyFirstValue AND MyOtherFieldName != MySecondValue)");

            // Assert
            actual.Conditions.Should().HaveCount(2);
            actual.Conditions.First().CloseBracket.Should().BeFalse();
            actual.Conditions.Last().CloseBracket.Should().BeTrue();
            actual.Conditions.First().OpenBracket.Should().BeTrue();
            actual.Conditions.Last().OpenBracket.Should().BeFalse();
            actual.Conditions.First().Field.FieldName.Should().Be("MyFieldName");
            actual.Conditions.Last().Value.Should().Be("MySecondValue");
        }

        [Theory]
        [InlineData("=", "XOR")]
        [InlineData("?", "OR")]
        [InlineData("?", "KABOOM")]
        public void Can_Parse_SimpleQuery(string @operator, string combination)
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, $"MyFieldName {@operator} MyFirstValue {combination} MyOtherFieldName != MySecondValue");

            // Assert
            actual.Conditions.Should().HaveCount(7);
            actual.Conditions.All(x => x.Field.FieldName == "PrefilledField").Should().BeTrue();
            actual.Conditions.All(x => x.Combination == QueryCombination.Or).Should().BeTrue();
            actual.Conditions.All(x => x.Operator == QueryOperator.Contains).Should().BeTrue();

            actual.Conditions.ElementAt(0).Value.Should().Be("MyFieldName");
            actual.Conditions.ElementAt(1).Value.Should().Be(@operator);
            actual.Conditions.ElementAt(2).Value.Should().Be("MyFirstValue");
            actual.Conditions.ElementAt(3).Value.Should().Be(combination);
            actual.Conditions.ElementAt(4).Value.Should().Be("MyOtherFieldName");
            actual.Conditions.ElementAt(5).Value.Should().Be("!=");
            actual.Conditions.ElementAt(6).Value.Should().Be("MySecondValue");

            actual.Conditions.ElementAt(0).OpenBracket.Should().BeTrue();
            actual.Conditions.ElementAt(1).OpenBracket.Should().BeFalse();
            actual.Conditions.ElementAt(2).OpenBracket.Should().BeFalse();
            actual.Conditions.ElementAt(3).OpenBracket.Should().BeFalse();
            actual.Conditions.ElementAt(4).OpenBracket.Should().BeFalse();
            actual.Conditions.ElementAt(5).OpenBracket.Should().BeFalse();
            actual.Conditions.ElementAt(6).OpenBracket.Should().BeFalse();

            actual.Conditions.ElementAt(0).CloseBracket.Should().BeFalse();
            actual.Conditions.ElementAt(1).CloseBracket.Should().BeFalse();
            actual.Conditions.ElementAt(2).CloseBracket.Should().BeFalse();
            actual.Conditions.ElementAt(3).CloseBracket.Should().BeFalse();
            actual.Conditions.ElementAt(4).CloseBracket.Should().BeFalse();
            actual.Conditions.ElementAt(5).CloseBracket.Should().BeFalse();
            actual.Conditions.ElementAt(6).CloseBracket.Should().BeTrue();
        }

        [Theory]
        [InlineData("-", QueryOperator.NotContains, QueryCombination.And)]
        [InlineData("+", QueryOperator.Contains, QueryCombination.And)]
        [InlineData("", QueryOperator.Contains, QueryCombination.Or)]
        public void Can_Parse_SimpleQuery_With_Sign(string sign, QueryOperator expectedOperator, QueryCombination expectedCombination)
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, $"{sign}First {sign}Second");

            // Assert
            actual.Conditions.Should().HaveCount(2);
            actual.Conditions.All(x => x.Field.FieldName == "PrefilledField").Should().BeTrue();
            actual.Conditions.All(x => x.Combination == expectedCombination).Should().BeTrue();
            actual.Conditions.All(x => x.Operator == expectedOperator).Should().BeTrue();
        }

        [Fact]
        public void Can_Parse_SimpleQuery_With_Two_Words()
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, "First Second");

            // Assert
            actual.Conditions.Should().HaveCount(2);
        }

        [Fact]
        public void Can_Parse_SimpleQuery_With_Invalid_Combination()
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, "MyFieldName = MyFirstValue ? MyOtherFieldName != MySecondValue");

            // Assert
            actual.Conditions.Should().HaveCount(7);
        }

        [Fact]
        public void Can_Parse_SimpleQuery_With_Invalid_Operator()
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, "MyFieldName = MyFirstValue AND MyOtherFieldName ? MySecondValue");

            // Assert
            actual.Conditions.Should().HaveCount(7);
        }

        [Fact]
        public void Can_Parse_Empty_Query()
        {
            // Arrange
            var builder = new SingleEntityQueryBuilder();
            var sut = CreateSut();

            // Act
            var actual = sut.Parse(builder, string.Empty);

            // Assert
            actual.Conditions.Should().BeEmpty();
        }

        private static SingleEntityQueryParser<ISingleEntityQueryBuilder, QueryExpressionBuilder> CreateSut()
            => new(() => new QueryExpressionBuilder("PrefilledField"));
    }
}
