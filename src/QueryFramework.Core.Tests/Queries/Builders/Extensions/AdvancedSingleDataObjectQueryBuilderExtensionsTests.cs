using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using Xunit;

namespace QueryFramework.Core.Tests.Queries.Builders.Extensions
{
    [ExcludeFromCodeCoverage]
    public class AdvancedSingleDataObjectQueryBuilderExtensionsTests
    {
        [Fact]
        public void Can_Use_From_To_Set_DataObjectName()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.From("tableName");

            // Assert
            actual.DataObjectName.Should().Be("tableName");
        }

        [Fact]
        public void Can_Use_WithParameters_With_QueryParameterBuilder_To_Add_Parameter()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.WithParameters(new QueryParameterBuilder("name", "value"));

            // Assert
            actual.Parameters.Should().HaveCount(1);
            actual.Parameters.First().Name.Should().Be("name");
            actual.Parameters.First().Value.Should().Be("value");
        }

        [Fact]
        public void Can_Use_WithParameters_With_QueryParameter_To_Add_Parameter()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.WithParameters(new QueryParameter("name", "value"));

            // Assert
            actual.Parameters.Should().HaveCount(1);
            actual.Parameters.First().Name.Should().Be("name");
            actual.Parameters.First().Value.Should().Be("value");
        }

        [Fact]
        public void Can_Use_WithParameter_With_Name_String_And_Object_Value_To_Add_Parameter()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.WithParameter("name", "value");

            // Assert
            actual.Parameters.Should().HaveCount(1);
            actual.Parameters.First().Name.Should().Be("name");
            actual.Parameters.First().Value.Should().Be("value");
        }

        [Fact]
        public void Can_Use_WithParameters_With_KeyValuePair_To_Add_Parameter()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.WithParameters(new KeyValuePair<string, object>("name", "value"));

            // Assert
            actual.Parameters.Should().HaveCount(1);
            actual.Parameters.First().Name.Should().Be("name");
            actual.Parameters.First().Value.Should().Be("value");
        }

        [Fact]
        public void Can_Use_GroupBy_With_QueryExpressionBuilder_To_Add_GroupBy_Field()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.GroupBy(new QueryExpressionBuilder("FieldName", "Expression"));

            // Assert
            actual.GroupByFields.Should().HaveCount(1);
            actual.GroupByFields.First().FieldName.Should().Be("FieldName");
            actual.GroupByFields.First().Expression.Should().Be("Expression");
        }

        [Fact]
        public void Can_Use_GroupBy_With_QueryExpression_To_Add_GroupBy_Field()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.GroupBy(new QueryExpression("FieldName", "Expression"));

            // Assert
            actual.GroupByFields.Should().HaveCount(1);
            actual.GroupByFields.First().FieldName.Should().Be("FieldName");
            actual.GroupByFields.First().Expression.Should().Be("Expression");
        }

        [Fact]
        public void Can_Use_GroupBy_With_FieldName_String_To_Add_GroupBy_Field()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.GroupBy("FieldName");

            // Assert
            actual.GroupByFields.Should().HaveCount(1);
            actual.GroupByFields.First().FieldName.Should().Be("FieldName");
            actual.GroupByFields.First().Expression.Should().BeNull();
        }

        [Fact]
        public void Can_Use_GroupBy_With_FieldName_Strings_To_Add_Multiple_GroupBy_Fields()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.GroupBy("FieldName1", "FieldName2");

            // Assert
            actual.GroupByFields.Should().HaveCount(2);
            actual.GroupByFields.ElementAt(0).FieldName.Should().Be("FieldName1");
            actual.GroupByFields.ElementAt(0).Expression.Should().BeNull();
            actual.GroupByFields.ElementAt(1).FieldName.Should().Be("FieldName2");
            actual.GroupByFields.ElementAt(1).Expression.Should().BeNull();
        }

        [Fact]
        public void Can_Use_Having_To_Add_HavingField_With_QueryCondition()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.Having(new QueryCondition("FieldName", QueryOperator.Contains, "Value"));

            // Assert
            actual.HavingFields.Should().HaveCount(1);
            actual.HavingFields.First().Field.FieldName.Should().Be("FieldName");
            actual.HavingFields.First().Operator.Should().Be(QueryOperator.Contains);
            actual.HavingFields.First().Value.Should().Be("Value");
        }

        [Fact]
        public void Can_Use_Having_To_Add_HavingField_With_QueryConditionBuilder()
        {
            // Arrange
            var sut = new AdvancedSingleDataObjectQueryBuilder();

            // Act
            var actual = sut.Having(new QueryConditionBuilder("FieldName", QueryOperator.Contains, "Value"));

            // Assert
            actual.HavingFields.Should().HaveCount(1);
            actual.HavingFields.First().Field.FieldName.Should().Be("FieldName");
            actual.HavingFields.First().Operator.Should().Be(QueryOperator.Contains);
            actual.HavingFields.First().Value.Should().Be("Value");
        }
    }
}
