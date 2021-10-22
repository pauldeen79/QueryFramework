using System;
using System.Data.Stub;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentAssertions;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using QueryFramework.SqlServer.Extensions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class StringBuilderExtensionsTests
    {
        [Fact]
        public void AppendSelectFields_Skips_SkipFields()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            var actual = builder.AppendSelectFields(query, new[] { "Field2" }, countOnly: false);

            // Assert
            actual.ToString().Should().Be("Field1, Field3");
        }

        [Fact]
        public void AppendSelectFields_Uses_CountOnly_When_Set_To_True()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            var actual = builder.AppendSelectFields(query, new[] { "Field2" }, countOnly: true);

            // Assert
            actual.ToString().Should().Be("COUNT(*)");
        }

        [Fact]
        public void AppendSelectFields_Uses_Fields_When_Filled_And_GetAllFieldsDelegate_Is_Null_And_SelectAll_Is_True()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();

            // Act
            var actual = builder.AppendSelectFields(query, Array.Empty<string>(), countOnly: false, fields: "Field4, Field5, Field6");

            // Assert
            actual.ToString().Should().Be("Field4, Field5, Field6");
        }

        [Fact]
        public void AppendSelectFields_Uses_Star_When_Fields_Is_Empty_And_GetAllFieldsDelegate_Is_Null_And_SelectAll_Is_True()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();

            // Act
            var actual = builder.AppendSelectFields(query, Array.Empty<string>(), countOnly: false, fields: null);

            // Assert
            actual.ToString().Should().Be("*");
        }

        [Fact]
        public void AppendSelectFields_Uses_GetAllFieldsDelegate_When_Provided()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();

            // Act
            var actual = builder.AppendSelectFields(query, Array.Empty<string>(), countOnly: false, fields: null, getAllFieldsDelegate: () => new[] { "Field1", "Field2", "Field3" });

            // Assert
            actual.ToString().Should().Be("Field1, Field2, Field3");
        }

        [Fact]
        public void AppendSelectFields_Uses_Fields_From_Query_When_GetAllFields_Is_False()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            var actual = builder.AppendSelectFields(query, Array.Empty<string>(), countOnly: false);

            // Assert
            actual.ToString().Should().Be("Field1, Field2, Field3");
        }

        [Fact]
        public void AppendSelectFields_Uses_GetFieldDelegate_When_Provided()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            var actual = builder.AppendSelectFields(query, Array.Empty<string>(), countOnly: false, getFieldNameDelegate: x => x + "A");

            // Assert
            actual.ToString().Should().Be("Field1A, Field2A, Field3A");
        }

        [Fact]
        public void AppendSelectFields_Throws_When_GetFieldDelegate_Returns_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            builder.Invoking(x => x.AppendSelectFields(query, Array.Empty<string>(), countOnly: false, getFieldNameDelegate: _ => null))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query fields contains unknown field in expression [Field1]");
        }

        [Fact]
        public void AppendSelectFields_Throws_When_ExpressionValidationDelegate_Returns_False()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            builder.Invoking(x => x.AppendSelectFields(query, Array.Empty<string>(), countOnly: false, expressionValidationDelegate: _ => false))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query fields contains invalid expression [Field1]");
        }

        [Fact]
        public void AppendWhereClause_Does_Not_Append_Anything_When_Conditions_And_DefaultWhere_Are_Both_Empty()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Build();
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();

            // Act
            var actual = builder.AppendWhereClause(query.Conditions, out int paramCounter, command);

            // Assert
            actual.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public void AppendWhereClause_Adds_Single_Condition_Without_Default_Where_Clause()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();

            // Act
            var actual = builder.AppendWhereClause(query.Conditions, out int paramCounter, command);

            // Assert
            actual.ToString().Should().Be(" WHERE Field = @p0");
        }

        [Fact]
        public void AppendWhereClause_Adds_Single_Condition_With_Default_Where_Clause()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();

            // Act
            var actual = builder.AppendWhereClause(query.Conditions, out int paramCounter, command, defaultWhere: "Field IS NOT NULL");

            // Assert
            actual.ToString().Should().Be(" WHERE Field IS NOT NULL AND Field = @p0");
        }

        [Fact]
        public void AppendWhereClause_Adds_Multiple_Conditions_Without_Default_Where_Clause()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).And("Field2".IsNotNull()).Build();
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();

            // Act
            var actual = builder.AppendWhereClause(query.Conditions, out int paramCounter, command);

            // Assert
            actual.ToString().Should().Be(" WHERE Field = @p0 AND Field2 IS NOT NULL");
        }

        [Fact]
        public void AppendWhereClause_Adds_Multiple_Conditions_With_Default_Where_Clause()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).And("Field2".IsNotNull()).Build();
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();

            // Act
            var actual = builder.AppendWhereClause(query.Conditions, out int paramCounter, command, defaultWhere: "Field IS NOT NULL");

            // Assert
            actual.ToString().Should().Be(" WHERE Field IS NOT NULL AND Field = @p0 AND Field2 IS NOT NULL");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_Anything_When_Limit_And_Offset_Are_Both_Filled()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Limit(10).Offset(20).Build();

            // Act
            var actual = builder.AppendOrderByClause(query.OrderByFields, defaultOrderBy: "Ignored", query.Offset, countOnly: false);

            // Assert
            actual.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_Anything_When_CountOnly_Is_Set_To_True()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendOrderByClause(query.OrderByFields, defaultOrderBy: "Ignored", query.Offset, countOnly: true);

            // Assert
            actual.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_Anything_When_Query_OrderByFields_Is_Empty()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Build();

            // Act
            var actual = builder.AppendOrderByClause(query.OrderByFields, defaultOrderBy: null, query.Offset, countOnly: false);

            // Assert
            actual.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public void AppendOrderBy_Appends_Single_OrderBy_Clause()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendOrderByClause(query.OrderByFields, defaultOrderBy: null, query.Offset, countOnly: false);

            // Assert
            actual.ToString().Should().Be(" ORDER BY Field ASC");
        }

        [Fact]
        public void AppendOrderBy_Appends_Multiple_OrderBy_Clauses()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field1").ThenByDescending("Field2").Build();

            // Act
            var actual = builder.AppendOrderByClause(query.OrderByFields, defaultOrderBy: null, query.Offset, countOnly: false);

            // Assert
            actual.ToString().Should().Be(" ORDER BY Field1 ASC, Field2 DESC");
        }

        [Fact]
        public void AppendOrderBy_Appends_Default_OrderBy_Clause()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().Build();

            // Act
            var actual = builder.AppendOrderByClause(query.OrderByFields, defaultOrderBy: "Field ASC", query.Offset, countOnly: false);

            // Assert
            actual.ToString().Should().Be(" ORDER BY Field ASC");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_DefaultOrderBy_When_OrderBy_Clause_Is_Present_On_Query()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendOrderByClause(query.OrderByFields, defaultOrderBy: "ignored", query.Offset, countOnly: false);

            // Assert
            actual.ToString().Should().Be(" ORDER BY Field ASC");
        }

        [Fact]
        public void AppendOrderBy_Appends_Single_OrderBy_Clause_With_GetFieldNameDelegate()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendOrderByClause(query.OrderByFields, defaultOrderBy: null, query.Offset, countOnly: false, getFieldNameDelegate: x => x + "A");

            // Assert
            actual.ToString().Should().Be(" ORDER BY FieldA ASC");
        }

        [Fact]
        public void AppendOrderBy_Throws_When_GetFieldNameDelegate_Returns_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendOrderByClause(query.OrderByFields, defaultOrderBy: null, query.Offset, countOnly: false, getFieldNameDelegate: _ => null))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query order by fields contains unknown field [Field]");
        }

        [Fact]
        public void AppendOrderBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldName_Is_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendOrderByClause(query.OrderByFields, defaultOrderBy: null, query.Offset, countOnly: false, expressionValidationDelegate: _ => false, getFieldNameDelegate: null))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query order by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendOrderBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldName_Is_Not_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendOrderByClause(query.OrderByFields, defaultOrderBy: null, query.Offset, countOnly: false,  expressionValidationDelegate: _ => false, getFieldNameDelegate: x => x))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query order by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendGroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Null()
        {
            // Arrange
            var builder = new StringBuilder();

            // Act
            var actual = builder.AppendGroupByClause(null);

            // Assert
            actual.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public void AppendGroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Empty()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().Build();

            // Act
            var actual = builder.AppendGroupByClause(query.GroupByFields);

            // Assert
            actual.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public void AppendGroupBy_Appends_Single_GroupBy_Clause()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act
            var actual = builder.AppendGroupByClause(query.GroupByFields);

            // Assert
            actual.ToString().Should().Be(" GROUP BY Field");
        }

        [Fact]
        public void AppendGroupBy_Appends_Multiple_GroupBy_Clauses()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field1", "Field2").Build();

            // Act
            var actual = builder.AppendGroupByClause(query.GroupByFields);

            // Assert
            actual.ToString().Should().Be(" GROUP BY Field1, Field2");
        }

        [Fact]
        public void AppendGroupBy_Appends_GroupBy_Clause_With_GetFieldNameDelegate()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act
            var actual = builder.AppendGroupByClause(query.GroupByFields, getFieldNameDelegate: x => x + "A");

            // Assert
            actual.ToString().Should().Be(" GROUP BY FieldA");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_GetFieldNameDelegate_Returns_Null_And_ExpressionValidationDelegate_Is_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendGroupByClause(query.GroupByFields, getFieldNameDelegate: x => null))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query group by fields contains unknown field [Field]");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_GetFieldNameDelegate_Returns_Null_And_ExpressionValidationDelegate_Is_Not_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendGroupByClause(query.GroupByFields, getFieldNameDelegate: x => null, expressionValidationDelegate: _ => true))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query group by fields contains unknown field [Field]");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldNameDelegate_Is_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendGroupByClause(query.GroupByFields, getFieldNameDelegate: null, expressionValidationDelegate: _ => false))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query group by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldNameDelegate_Is_Not_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendGroupByClause(query.GroupByFields, getFieldNameDelegate: x => x, expressionValidationDelegate: _ => false))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query group by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendHaving_Adds_Single_Condition()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().Having("Field".IsEqualTo("value")).Build();
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();
            int paramCounter = 0;

            // Act
            var actual = builder.AppendHavingClause(query.HavingFields, ref paramCounter, command);

            // Assert
            actual.ToString().Should().Be(" HAVING Field = @p0");
        }

        [Fact]
        public void AppendHaving_Adds_Multiple_Conditions()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().Having("Field1".IsEqualTo("value")).Having("Field2".IsEqualTo("value")).Build();
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();
            int paramCounter = 0;

            // Act
            var actual = builder.AppendHavingClause(query.HavingFields, ref paramCounter, command);

            // Assert
            actual.ToString().Should().Be(" HAVING Field1 = @p0 AND Field2 = @p1");
        }

        [Fact]
        public void AppendHaving_Does_Not_Append_Anything_When_HavingFields_Is_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            int paramCounter = 0;
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();

            // Act
            var actual = builder.AppendHavingClause(null, ref paramCounter, command);

            // Assert
            actual.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public void AppendHaving_Does_Not_Append_Anything_When_HavingFields_Is_Empty()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().Build();
            int paramCounter = 0;
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();

            // Act
            var actual = builder.AppendHavingClause(query.HavingFields, ref paramCounter, command);

            // Assert
            actual.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public void AppendPagingOuterQuery_Throws_On_Null_Query()
        {
            // Arrange
            var builder = new StringBuilder();

            // Act & Assert
            builder.Invoking(x => x.AppendPagingOuterQuery(null, "fields", Array.Empty<string>(), false))
                   .Should().Throw<ArgumentNullException>()
                   .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void AppendTopClause_Throws_On_Null_Query()
        {
            // Arrange
            var builder = new StringBuilder();

            // Act & Assert
            builder.Invoking(x => x.AppendTopClause(null, null, false))
                   .Should().Throw<ArgumentNullException>()
                   .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void AppendPagingPrefix_Throws_On_Null_Query()
        {
            // Arrange
            var builder = new StringBuilder();

            // Act & Assert
            builder.Invoking(x => x.AppendPagingPrefix(null, false))
                   .Should().Throw<ArgumentNullException>()
                   .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void AppendPagingPrefix_Returns_Correct_Result_On_Empty_OrderByFields()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().Offset(10).Build();

            // Act
            var actual = builder.AppendPagingPrefix(query, false);

            // Assert
            actual.ToString().Should().Be(", ROW_NUMBER() OVER (ORDER BY (SELECT 0)) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Returns_Correct_Result_On_Single_OrderByFields()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();

            // Act
            var actual = builder.AppendPagingPrefix(query, false);

            // Assert
            actual.ToString().Should().Be(", ROW_NUMBER() OVER (ORDER BY Field ASC) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Returns_Correct_Result_On_Multiple_OrderByFields()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field1").ThenBy("Field2").Offset(10).Build();

            // Act
            var actual = builder.AppendPagingPrefix(query, false);

            // Assert
            actual.ToString().Should().Be(", ROW_NUMBER() OVER (ORDER BY Field1 ASC, Field2 ASC) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Uses_GetFieldNameDelegate_When_Provided()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();

            // Act
            var actual = builder.AppendPagingPrefix(query, false, getFieldNameDelegate: x => x + "A");

            // Assert
            actual.ToString().Should().Be(", ROW_NUMBER() OVER (ORDER BY FieldA ASC) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Throws_When_GetFieldNameDelegate_Returns_Null()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();

            // Act & Assert
            builder.Invoking(x => x.AppendPagingPrefix(query, false, getFieldNameDelegate: _ => null))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query OrderByFields contains unknown field [Field]");
        }

        [Fact]
        public void AppendPagingPrefix_Throws_When_ExpressionValidationDelegate_Returns_False()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();

            // Act & Assert
            builder.Invoking(x => x.AppendPagingPrefix(query, false, expressionValidationDelegate: _ => false))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.Message.Should().StartWith("Query OrderByFields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendPagingSuffix_Throws_On_Null_Query()
        {
            // Arrange
            var builder = new StringBuilder();

            // Act & Assert
            builder.Invoking(x => x.AppendPagingSuffix(null, null, false))
                   .Should().Throw<ArgumentNullException>()
                   .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void AppendPagingSuffix_Does_Not_Append_Anything_When_CountOnly_Is_Set_To_True()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendPagingSuffix(query, null, true);

            // Assert
            actual.ToString().Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public void AppendPagingSuffix_Does_Not_Append_Anything_When_Offset_Is(int? offset)
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(offset).Build();

            // Act
            var actual = builder.AppendPagingSuffix(query, null, false);

            // Assert
            actual.ToString().Should().BeEmpty();
        }

        [Fact]
        public void AppendPagingSuffix_Returns_Correct_Result_When_Limit_Is_Greater_Than_Zero()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Limit(20).Build();

            // Act
            var actual = builder.AppendPagingSuffix(query, null, false);

            // Assert
            actual.ToString().Should().Be(") sq WHERE sq.sq_row_number BETWEEN 11 and 30;");
        }

        [Fact]
        public void AppendPagingSuffix_Returns_Correct_Result_When_Limit_Is_Zero()
        {
            // Arrange
            var builder = new StringBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Limit(0).Build();

            // Act
            var actual = builder.AppendPagingSuffix(query, null, false);

            // Assert
            actual.ToString().Should().Be(") sq WHERE sq.sq_row_number > 10;");
        }
    }
}
