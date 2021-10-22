using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using QueryFramework.SqlServer.Extensions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DbCommandExtensionsTests
    {
        [Fact]
        public void ExecuteSelectCommand_Gives_A_DataReader_Based_On_Input_Parameters()
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var query = new Mock<ISingleEntityQuery>();

            // Act
            var actual = sut.ExecuteSelectCommand(query.Object, "Field1, Field2", "Table");

            // Assert
            actual.Should().BeAssignableTo<IDataReader>();
        }

        [Fact]
        public void ExecuteSelectCommand_Throws_On_Null_Query()
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();

            // Act & Assert
            sut.Invoking(x => x.ExecuteSelectCommand(null))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void FillSelectCommand_Throws_On_Null_Query()
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();

            // Act & Assert
            sut.Invoking(x => x.FillSelectCommand(null))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void FillSelectCommand_Uses_DynamicQuery_Result_When_Query_Implements_IDynamicQuery()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            using var sut = connection.CreateCommand();
            var query = new DynamicQueryMock();

            // Act
            sut.ExecuteSelectCommand(query, "Field1, Field2", "Table");

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("SELECT Field1, Field2 FROM (SELECT TOP 10 Field1, Field2, ROW_NUMBER() OVER (ORDER BY (SELECT 0)) as sq_row_number FROM Table) sq WHERE sq.sq_row_number BETWEEN 11 and 20;");
        }

        [Fact]
        public void FillSelectCommand_Validates_Query_When_ValidateFieldNames_Is_Set_To_True()
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var query = new ValidatableQueryMock();

            // Act & Assert
            sut.Invoking(x => x.FillSelectCommand(query, validateFieldNames: true))
               .Should().Throw<ValidationException>();
        }

        [Fact]
        public void FillSelectCommand_Does_Not_Validate_Query_When_ValidateFieldNames_Is_Set_To_False()
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var query = new ValidatableQueryMock();

            // Act & Assert
            sut.Invoking(x => x.FillSelectCommand(query, validateFieldNames: false))
               .Should().NotThrow<ValidationException>();
        }

        [Fact]
        public void FillSelectCommand_Fills_CommandText_From_Query()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            using var sut = connection.CreateCommand();
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(10)
                .Offset(20)
                .From("Table")
                .Where("Field1".IsEqualTo("value1"))
                .Or("Field1".IsEqualTo("value2"))
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FillSelectCommand(query);

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("SELECT Field1, Field2 FROM (SELECT Field1, Field2, ROW_NUMBER() OVER (ORDER BY Field2 ASC) as sq_row_number FROM Table WHERE Field1 = @p0 OR Field1 = @p1) sq WHERE sq.sq_row_number BETWEEN 21 and 30;");
        }

        [Fact]
        public void FillSelectCommand_Fills_CommandText_From_Query_With_Distinct_Clause()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            using var sut = connection.CreateCommand();
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .SelectDistinct("Field1", "Field2")
                .From("Table")
                .Build();

            // Act
            sut.FillSelectCommand(query);

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("SELECT DISTINCT Field1, Field2 FROM Table");
        }

        [Fact]
        public void FillSelectCommand_Adds_QueryParameters_When_Available()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            using var sut = connection.CreateCommand();
            var query = new ParameterizedQueryMock
            {
                Conditions = new List<IQueryCondition> { new QueryCondition("field", QueryOperator.Equal, new QueryParameterValue("myparameter")) },
                Parameters = new List<IQueryParameter> { new QueryParameter("myparameter", "value") }
            };

            // Act
            sut.FillSelectCommand(query, tableName: "Table");

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("SELECT * FROM Table WHERE field = @myparameter");
            callback.Commands.First().Parameters.Cast<IDbDataParameter>().Should().HaveCount(1);
            callback.Commands.First().Parameters.Cast<IDbDataParameter>().First().ParameterName.Should().Be("myparameter");
            callback.Commands.First().Parameters.Cast<IDbDataParameter>().First().Value.Should().Be("value");
        }

        [Fact]
        public void FillSelectCommand_Uses_Default_OrderBy_Clause_When_Available()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            using var sut = connection.CreateCommand();
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(100)
                .From("Table")
                .Where("Field1".IsEqualTo("value"))
                .Build();

            // Act
            sut.FillSelectCommand(query, defaultOrderBy: "Field2 ASC");

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("SELECT TOP 100 Field1, Field2 FROM Table WHERE Field1 = @p0 ORDER BY Field2 ASC");
        }

        [Fact]
        public void FillSelectCommand_Uses_Default_Where_Clause_When_Available()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            using var sut = connection.CreateCommand();
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(100)
                .From("Table")
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FillSelectCommand(query, defaultWhere: "Field1 = 'Value'");

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("SELECT TOP 100 Field1, Field2 FROM Table WHERE Field1 = 'Value' ORDER BY Field2 ASC");
        }

        [Fact]
        public void FillSelectCommand_Uses_TableName_When_Available()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            using var sut = connection.CreateCommand();
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(100)
                .Where("Field1".IsEqualTo("value"))
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FillSelectCommand(query, tableName: "Table");

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("SELECT TOP 100 Field1, Field2 FROM Table WHERE Field1 = @p0 ORDER BY Field2 ASC");
        }

        [Fact]
        public void FillSelectCommand_Uses_OverrideLimit_When_Available()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            using var sut = connection.CreateCommand();
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(100)
                .From("Table")
                .Where("Field1".IsEqualTo("value"))
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FillSelectCommand(query, overrideLimit: 10);

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("SELECT TOP 10 Field1, Field2 FROM Table WHERE Field1 = @p0 ORDER BY Field2 ASC");
        }

        [Fact]
        public void FillSelectCommand_Uses_AllFieldsDelegate_When_Available()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            using var sut = connection.CreateCommand();
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .SelectAll()
                .Limit(100)
                .From("Table")
                .Where("Field1".IsEqualTo("value"))
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FillSelectCommand(query, getAllFieldsDelegate: () => new[] { "Field1" , "Field2" });

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be("SELECT TOP 100 Field1, Field2 FROM Table WHERE Field1 = @p0 ORDER BY Field2 ASC");
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(99, true)]
        public void AppendQueryCondition_Adds_Combination_Conditionally_But_Always_Increases_ParamCountner_When_ParamCounter_Is(int paramCounter, bool shouldAddCombination)
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var builder = new StringBuilder();

            // Act
            var actual = sut.AppendQueryCondition(builder, paramCounter, new QueryCondition("Field", QueryOperator.Greater, "value"));

            // Assert
            if (shouldAddCombination)
            {
                builder.ToString().Should().Be($" AND Field > @p{paramCounter}");
            }
            else
            {
                builder.ToString().Should().Be($"Field > @p{paramCounter}");
            }
            actual.Should().Be(paramCounter + 1);
        }

        [Fact]
        public void AppendQueryCondition_Adds_Brackets_When_Necessary()
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var builder = new StringBuilder();

            // Act
            sut.AppendQueryCondition(builder, 0, new QueryCondition("Field", QueryOperator.Greater, "value", true, true));

            // Assert
            builder.ToString().Should().Be("(Field > @p0)");
        }

        [Fact]
        public void AppendQueryCondition_Gets_CustomFieldName_When_Possible()
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var builder = new StringBuilder();

            // Act
            sut.AppendQueryCondition(builder,
                                     0,
                                     new QueryCondition("Field", QueryOperator.Greater, "value"),
                                     getFieldNameDelegate: x => x == "Field" ? "CustomField" : x);

            // Assert
            builder.ToString().Should().Be("CustomField > @p0");
        }

        [Fact]
        public void AppendQueryCondition_Throws_On_Invalid_CustomFieldName_When_ValidateFieldNames_Is_Set_To_True()
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var builder = new StringBuilder();

            // Act
            sut.Invoking(x => x.AppendQueryCondition(builder,
                                                     0,
                                                     new QueryCondition("Field", QueryOperator.Greater, "value"),
                                                     getFieldNameDelegate: x => x == "Field" ? null : x))
               .Should().Throw<ArgumentOutOfRangeException>().And.Message.Should().StartWith("Query conditions contains unknown field [Field]");
        }

        [Fact]
        public void AppendQueryCondition_Throws_On_Invalid_Expression_When_ExpressionValidationDelegate_Is_Not_Null()
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var builder = new StringBuilder();

            // Act
            sut.Invoking(x => x.AppendQueryCondition(builder,
                                                     0,
                                                     new QueryCondition(new QueryExpression("Field", "SUM({0})"), QueryOperator.Greater, "value"),
                                                     expressionValidationDelegate: _ => false))
               .Should().Throw<ArgumentOutOfRangeException>().And.Message.Should().StartWith("Query conditions contains invalid expression [SUM(Field)]");
        }

        [Theory]
        [InlineData(QueryOperator.IsNullOrEmpty, "COALESCE(Field,'') = ''")]
        [InlineData(QueryOperator.IsNotNullOrEmpty, "COALESCE(Field,'') <> ''")]
        [InlineData(QueryOperator.IsNull, "Field IS NULL")]
        [InlineData(QueryOperator.IsNotNull, "Field IS NOT NULL")]
        public void AppendQueryCondition_Fills_CommandText_Correctly_For_QueryOperator_Without_Value(QueryOperator queryOperator, string expectedCommandText)
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var builder = new StringBuilder();

            // Act
            sut.AppendQueryCondition(builder, 0, new QueryCondition("Field", queryOperator));

            // Assert
            builder.ToString().Should().Be(expectedCommandText);
        }

        [Theory]
        [InlineData(QueryOperator.Contains, "CHARINDEX(@p0, Field) > 0")]
        [InlineData(QueryOperator.NotContains, "CHARINDEX(@p0, Field) = 0")]
        [InlineData(QueryOperator.StartsWith, "LEFT(Field, 4) = @p0")]
        [InlineData(QueryOperator.NotStartsWith, "LEFT(Field, 4) <> @p0")]
        [InlineData(QueryOperator.EndsWith, "RIGHT(Field, 4) = @p0")]
        [InlineData(QueryOperator.NotEndsWith, "RIGHT(Field, 4) <> @p0")]
        [InlineData(QueryOperator.Equal, "Field = @p0")]
        [InlineData(QueryOperator.GreaterOrEqual, "Field >= @p0")]
        [InlineData(QueryOperator.Greater, "Field > @p0")]
        [InlineData(QueryOperator.LowerOrEqual, "Field <= @p0")]
        [InlineData(QueryOperator.Lower, "Field < @p0")]
        [InlineData(QueryOperator.NotEqual, "Field <> @p0")]
        public void AppendQueryCondition_Fills_CommandText_And_Parameters_Correctly_For_QueryOperator_With_Value(QueryOperator queryOperator, string expectedCommandText)
        {
            // Arrange
            using var connection = new DbConnection();
            using var sut = connection.CreateCommand();
            var builder = new StringBuilder();

            // Act
            sut.AppendQueryCondition(builder, 0, new QueryCondition("Field", queryOperator, "test"));

            // Assert
            builder.ToString().Should().Be(expectedCommandText);
            sut.Parameters.OfType<IDbDataParameter>().Should().HaveCount(1);
            sut.Parameters.OfType<IDbDataParameter>().First().ParameterName.Should().Be("p0");
            sut.Parameters.OfType<IDbDataParameter>().First().Value.Should().Be("test");
        }

        [ExcludeFromCodeCoverage]
        private class DynamicQueryMock : IDynamicQuery
        {
            public int? Limit { get; set; }
            public int? Offset { get; set; }
            public IReadOnlyCollection<IQueryCondition> Conditions { get; set; }
            public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }

            public ISingleEntityQuery Process() => new DynamicQueryMock { Limit = 10, Offset = 10 };

            public DynamicQueryMock()
            {
                Conditions = new List<IQueryCondition>();
                OrderByFields = new List<IQuerySortOrder>();
            }
        }

        [ExcludeFromCodeCoverage]
        private class ValidatableQueryMock : ISingleEntityQuery, IValidatableObject
        {
            public int? Limit { get; set; }

            public int? Offset { get; set; }

            public IReadOnlyCollection<IQueryCondition> Conditions { get; set; }

            public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Limit == null)
                {
                    yield return new ValidationResult("Limit is required", new[] { nameof(Limit) });
                }
                if (Offset == null)
                {
                    yield return new ValidationResult("Offset is required", new[] { nameof(Offset) });
                }
            }

            public ValidatableQueryMock()
            {
                Conditions = new List<IQueryCondition>();
                OrderByFields = new List<IQuerySortOrder>();
            }
        }

        [ExcludeFromCodeCoverage]
        private class ParameterizedQueryMock : IParameterizedQuery
        {
            public IReadOnlyCollection<IQueryParameter> Parameters { get; set; }

            public int? Limit { get; set; }

            public int? Offset { get; set; }

            public IReadOnlyCollection<IQueryCondition> Conditions { get; set; }

            public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }

            public ParameterizedQueryMock()
            {
                Conditions = new List<IQueryCondition>();
                OrderByFields = new List<IQuerySortOrder>();
                Parameters = new List<IQueryParameter>();
            }
        }
    }
}
