using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Core.Builders;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using QueryFramework.SqlServer.Extensions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DatabaseCommandBuilderExtensionsTests
    {
        [Fact]
        public void AppendSelectFields_Skips_SkipFields()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            var actual = builder.AppendSelectFields(query, new QueryProcessorSettings(skipFields: new[] { "Field2" }), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("Field1, Field3");
        }

        [Fact]
        public void AppendSelectFields_Uses_CountOnly_When_Set_To_True()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            var actual = builder.AppendSelectFields(query, new QueryProcessorSettings(skipFields: new[] { "Field2" }), countOnly: true);

            // Assert
            actual.Build().CommandText.Should().Be("COUNT(*)");
        }

        [Fact]
        public void AppendSelectFields_Uses_Fields_When_Filled_And_GetAllFieldsDelegate_Is_Null_And_SelectAll_Is_True()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();

            // Act
            var actual = builder.AppendSelectFields(query, new QueryProcessorSettings(skipFields: Array.Empty<string>(), fields: "Field4, Field5, Field6"), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("Field4, Field5, Field6");
        }

        [Fact]
        public void AppendSelectFields_Uses_Star_When_Fields_Is_Empty_And_GetAllFieldsDelegate_Is_Null_And_SelectAll_Is_True()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();

            // Act
            var actual = builder.AppendSelectFields(query, new QueryProcessorSettings(skipFields: Array.Empty<string>(), fields: null), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("*");
        }

        [Fact]
        public void AppendSelectFields_Uses_GetAllFieldsDelegate_When_Provided()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();

            // Act
            var actual = builder.AppendSelectFields(query, new QueryProcessorSettings(skipFields: Array.Empty<string>(), fields: null, getAllFieldsDelegate: () => new[] { "Field1", "Field2", "Field3" }), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("Field1, Field2, Field3");
        }

        [Fact]
        public void AppendSelectFields_Uses_Fields_From_Query_When_GetAllFields_Is_False()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            var actual = builder.AppendSelectFields(query, new QueryProcessorSettings(skipFields: Array.Empty<string>()), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("Field1, Field2, Field3");
        }

        [Fact]
        public void AppendSelectFields_Uses_GetFieldDelegate_When_Provided()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            var actual = builder.AppendSelectFields(query, new QueryProcessorSettings(skipFields: Array.Empty<string>(), getFieldNameDelegate: x => x + "A"), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("Field1A, Field2A, Field3A");
        }

        [Fact]
        public void AppendSelectFields_Throws_When_GetFieldDelegate_Returns_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            builder.Invoking(x => x.AppendSelectFields(query, new QueryProcessorSettings(skipFields: Array.Empty<string>(), getFieldNameDelegate: _ => null), countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query fields contains unknown field in expression [Field1]");
        }

        [Fact]
        public void AppendSelectFields_Throws_When_ExpressionValidationDelegate_Returns_False()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();

            // Act
            builder.Invoking(x => x.AppendSelectFields(query, new QueryProcessorSettings(skipFields: Array.Empty<string>(), expressionValidationDelegate: _ => false), countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query fields contains invalid expression [Field1]");
        }

        [Fact]
        public void AppendWhereClause_Does_Not_Append_Anything_When_Conditions_And_DefaultWhere_Are_Both_Empty()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed
            var query = new FieldSelectionQueryBuilder().Build();

            // Act
            var actual = builder.AppendWhereClause(query, new QueryProcessorSettings(),  out _);

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Fact]
        public void AppendWhereClause_Adds_Single_Condition_Without_Default_Where_Clause()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();

            // Act
            var actual = builder.AppendWhereClause(query, new QueryProcessorSettings(), out _);

            // Assert
            actual.Build().CommandText.Should().Be(" WHERE Field = @p0");
        }

        [Fact]
        public void AppendWhereClause_Adds_Single_Condition_With_Default_Where_Clause()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();

            // Act
            var actual = builder.AppendWhereClause(query, new QueryProcessorSettings(defaultWhere: "Field IS NOT NULL"), out _);

            // Assert
            actual.Build().CommandText.Should().Be(" WHERE Field IS NOT NULL AND Field = @p0");
        }

        [Fact]
        public void AppendWhereClause_Adds_Multiple_Conditions_Without_Default_Where_Clause()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).And("Field2".IsNotNull()).Build();

            // Act
            var actual = builder.AppendWhereClause(query, new QueryProcessorSettings(), out _);

            // Assert
            actual.Build().CommandText.Should().Be(" WHERE Field = @p0 AND Field2 IS NOT NULL");
        }

        [Fact]
        public void AppendWhereClause_Adds_Multiple_Conditions_With_Default_Where_Clause()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).And("Field2".IsNotNull()).Build();

            // Act
            var actual = builder.AppendWhereClause(query, new QueryProcessorSettings(defaultWhere: "Field IS NOT NULL"), out _);

            // Assert
            actual.Build().CommandText.Should().Be(" WHERE Field IS NOT NULL AND Field = @p0 AND Field2 IS NOT NULL");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_Anything_When_Limit_And_Offset_Are_Both_Filled()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Limit(10).Offset(20).Build();

            // Act
            var actual = builder.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: "Ignored"), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_Anything_When_CountOnly_Is_Set_To_True()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: "Ignored"), countOnly: true);

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_Anything_When_Query_OrderByFields_Is_Empty()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed
            var query = new FieldSelectionQueryBuilder().Build();

            // Act
            var actual = builder.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: null), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Fact]
        public void AppendOrderBy_Appends_Single_OrderBy_Clause()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: null), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY Field ASC");
        }

        [Fact]
        public void AppendOrderBy_Appends_Multiple_OrderBy_Clauses()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field1").ThenByDescending("Field2").Build();

            // Act
            var actual = builder.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: null), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY Field1 ASC, Field2 DESC");
        }

        [Fact]
        public void AppendOrderBy_Appends_Default_OrderBy_Clause()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().Build();

            // Act
            var actual = builder.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: "Field ASC"), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY Field ASC");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_DefaultOrderBy_When_OrderBy_Clause_Is_Present_On_Query()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: "ignored"), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY Field ASC");
        }

        [Fact]
        public void AppendOrderBy_Appends_Single_OrderBy_Clause_With_GetFieldNameDelegate()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: null, getFieldNameDelegate: x => x + "A"), countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY FieldA ASC");
        }

        [Fact]
        public void AppendOrderBy_Throws_When_GetFieldNameDelegate_Returns_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: null, getFieldNameDelegate: _ => null), countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query order by fields contains unknown field [Field]");
        }

        [Fact]
        public void AppendOrderBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldName_Is_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: null, expressionValidationDelegate: _ => false, getFieldNameDelegate: null), countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query order by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendOrderBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldName_Is_Not_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendOrderByClause(query, new QueryProcessorSettings(defaultOrderBy: null, expressionValidationDelegate: _ => false, getFieldNameDelegate: x => x), countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query order by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendGroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed

            // Act
            var actual = builder.AppendGroupByClause(null, new QueryProcessorSettings());

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Fact]
        public void AppendGroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Empty()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed
            var query = new AdvancedSingleDataObjectQueryBuilder().Build();

            // Act
            var actual = builder.AppendGroupByClause(query, new QueryProcessorSettings());

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Fact]
        public void AppendGroupBy_Appends_Single_GroupBy_Clause()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act
            var actual = builder.AppendGroupByClause(query, new QueryProcessorSettings());

            // Assert
            actual.Build().CommandText.Should().Be(" GROUP BY Field");
        }

        [Fact]
        public void AppendGroupBy_Appends_Multiple_GroupBy_Clauses()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field1", "Field2").Build();

            // Act
            var actual = builder.AppendGroupByClause(query, new QueryProcessorSettings());

            // Assert
            actual.Build().CommandText.Should().Be(" GROUP BY Field1, Field2");
        }

        [Fact]
        public void AppendGroupBy_Appends_GroupBy_Clause_With_GetFieldNameDelegate()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act
            var actual = builder.AppendGroupByClause(query, new QueryProcessorSettings(getFieldNameDelegate: x => x + "A"));

            // Assert
            actual.Build().CommandText.Should().Be(" GROUP BY FieldA");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_GetFieldNameDelegate_Returns_Null_And_ExpressionValidationDelegate_Is_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendGroupByClause(query, new QueryProcessorSettings(getFieldNameDelegate: x => null)))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query group by fields contains unknown field [Field]");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_GetFieldNameDelegate_Returns_Null_And_ExpressionValidationDelegate_Is_Not_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendGroupByClause(query, new QueryProcessorSettings(getFieldNameDelegate: x => null, expressionValidationDelegate: _ => true)))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query group by fields contains unknown field [Field]");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldNameDelegate_Is_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendGroupByClause(query, new QueryProcessorSettings(getFieldNameDelegate: null, expressionValidationDelegate: _ => false)))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query group by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldNameDelegate_Is_Not_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().GroupBy("Field").Build();

            // Act & Assert
            builder.Invoking(x => x.AppendGroupByClause(query, new QueryProcessorSettings(getFieldNameDelegate: x => x, expressionValidationDelegate: _ => false)))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query group by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendHaving_Adds_Single_Condition()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().Having("Field".IsEqualTo("value")).Build();
            int paramCounter = 0;

            // Act
            var actual = builder.AppendHavingClause(query, new QueryProcessorSettings(), ref paramCounter);

            // Assert
            actual.Build().CommandText.Should().Be(" HAVING Field = @p0");
        }

        [Fact]
        public void AppendHaving_Adds_Multiple_Conditions()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new AdvancedSingleDataObjectQueryBuilder().Having("Field1".IsEqualTo("value")).Having("Field2".IsEqualTo("value")).Build();
            int paramCounter = 0;

            // Act
            var actual = builder.AppendHavingClause(query, new QueryProcessorSettings(), ref paramCounter);

            // Assert
            actual.Build().CommandText.Should().Be(" HAVING Field1 = @p0 AND Field2 = @p1");
        }

        [Fact]
        public void AppendHaving_Does_Not_Append_Anything_When_HavingFields_Is_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed
            int paramCounter = 0;

            // Act
            var actual = builder.AppendHavingClause(null, new QueryProcessorSettings(), ref paramCounter);

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Fact]
        public void AppendHaving_Does_Not_Append_Anything_When_HavingFields_Is_Empty()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed
            var query = new AdvancedSingleDataObjectQueryBuilder().Build();
            int paramCounter = 0;

            // Act
            var actual = builder.AppendHavingClause(query, new QueryProcessorSettings(), ref paramCounter);

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Fact]
        public void AppendPagingPrefix_Returns_Correct_Result_On_Empty_OrderByFields()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new SingleEntityQueryBuilder().Offset(10).Build();

            // Act
            var actual = builder.AppendPagingPrefix(query, new QueryProcessorSettings(), false);

            // Assert
            actual.Build().CommandText.Should().Be(", ROW_NUMBER() OVER (ORDER BY (SELECT 0)) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Returns_Correct_Result_On_Single_OrderByFields()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();

            // Act
            var actual = builder.AppendPagingPrefix(query, new QueryProcessorSettings(), false);

            // Assert
            actual.Build().CommandText.Should().Be(", ROW_NUMBER() OVER (ORDER BY Field ASC) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Returns_Correct_Result_On_Multiple_OrderByFields()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field1").ThenBy("Field2").Offset(10).Build();

            // Act
            var actual = builder.AppendPagingPrefix(query, new QueryProcessorSettings(), false);

            // Assert
            actual.Build().CommandText.Should().Be(", ROW_NUMBER() OVER (ORDER BY Field1 ASC, Field2 ASC) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Uses_GetFieldNameDelegate_When_Provided()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();

            // Act
            var actual = builder.AppendPagingPrefix(query, new QueryProcessorSettings(getFieldNameDelegate: x => x + "A"), false);

            // Assert
            actual.Build().CommandText.Should().Be(", ROW_NUMBER() OVER (ORDER BY FieldA ASC) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Throws_When_GetFieldNameDelegate_Returns_Null()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();

            // Act & Assert
            builder.Invoking(x => x.AppendPagingPrefix(query, new QueryProcessorSettings(getFieldNameDelegate: _ => null), false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query OrderByFields contains unknown field [Field]");
        }

        [Fact]
        public void AppendPagingPrefix_Throws_When_ExpressionValidationDelegate_Returns_False()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();

            // Act & Assert
            builder.Invoking(x => x.AppendPagingPrefix(query, new QueryProcessorSettings(expressionValidationDelegate: _ => false), false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query OrderByFields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendPagingSuffix_Does_Not_Append_Anything_When_CountOnly_Is_Set_To_True()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = builder.AppendPagingSuffix(query, null, true);

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public void AppendPagingSuffix_Does_Not_Append_Anything_When_Offset_Is(int? offset)
        {
            // Arrange
            var builder = new DatabaseCommandBuilder().Append(" "); // important to add a space, because null or empty is not allowed
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(offset).Build();

            // Act
            var actual = builder.AppendPagingSuffix(query, null, false);

            // Assert
            actual.Build().CommandText.Should().Be(" ");
        }

        [Fact]
        public void AppendPagingSuffix_Returns_Correct_Result_When_Limit_Is_Greater_Than_Zero()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Limit(20).Build();

            // Act
            var actual = builder.AppendPagingSuffix(query, new QueryProcessorSettings(), false);

            // Assert
            actual.Build().CommandText.Should().Be(") sq WHERE sq.sq_row_number BETWEEN 11 and 30;");
        }

        [Fact]
        public void AppendPagingSuffix_Returns_Correct_Result_When_Limit_Is_Zero()
        {
            // Arrange
            var builder = new DatabaseCommandBuilder();
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Limit(0).Build();

            // Act
            var actual = builder.AppendPagingSuffix(query, new QueryProcessorSettings(), false);

            // Assert
            actual.Build().CommandText.Should().Be(") sq WHERE sq.sq_row_number > 10;");
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(99, true)]
        public void AppendQueryCondition_Adds_Combination_Conditionally_But_Always_Increases_ParamCountner_When_ParamCounter_Is(int paramCounter, bool shouldAddCombination)
        {
            // Arrange
            var sut = new DatabaseCommandBuilder();

            // Act
            var actual = sut.AppendQueryCondition(paramCounter, new QueryCondition("Field", QueryOperator.Greater, "value"), new QueryProcessorSettings());

            // Assert
            if (shouldAddCombination)
            {
                sut.Build().CommandText.Should().Be($" AND Field > @p{paramCounter}");
            }
            else
            {
                sut.Build().CommandText.Should().Be($"Field > @p{paramCounter}");
            }
            actual.Should().Be(paramCounter + 1);
        }

        [Fact]
        public void AppendQueryCondition_Adds_Brackets_When_Necessary()
        {
            // Arrange
            var sut = new DatabaseCommandBuilder();

            // Act
            sut.AppendQueryCondition(0, new QueryCondition("Field", QueryOperator.Greater, "value", true, true), new QueryProcessorSettings());

            // Assert
            sut.Build().CommandText.Should().Be("(Field > @p0)");
        }

        [Fact]
        public void AppendQueryCondition_Gets_CustomFieldName_When_Possible()
        {
            // Arrange
            var sut = new DatabaseCommandBuilder();

            // Act
            sut.AppendQueryCondition(0,
                                     new QueryCondition("Field", QueryOperator.Greater, "value"),
                                     new QueryProcessorSettings(getFieldNameDelegate: x => x == "Field" ? "CustomField" : x));

            // Assert
            sut.Build().CommandText.Should().Be("CustomField > @p0");
        }

        [Fact]
        public void AppendQueryCondition_Throws_On_Invalid_CustomFieldName_When_ValidateFieldNames_Is_Set_To_True()
        {
            // Arrange
            var sut = new DatabaseCommandBuilder();

            // Act
            sut.Invoking(x => x.AppendQueryCondition(0,
                                                     new QueryCondition("Field", QueryOperator.Greater, "value"),
                                                     new QueryProcessorSettings(getFieldNameDelegate: x => x == "Field" ? null : x)))
               .Should().Throw<InvalidOperationException>().And.Message.Should().StartWith("Query conditions contains unknown field [Field]");
        }

        [Fact]
        public void AppendQueryCondition_Throws_On_Invalid_Expression_When_ExpressionValidationDelegate_Is_Not_Null()
        {
            // Arrange
            var sut = new DatabaseCommandBuilder();

            // Act
            sut.Invoking(x => x.AppendQueryCondition(0,
                                                     new QueryCondition(new QueryExpression("Field", "SUM({0})"), QueryOperator.Greater, "value"),
                                                     new QueryProcessorSettings(expressionValidationDelegate: _ => false)))
               .Should().Throw<InvalidOperationException>().And.Message.Should().StartWith("Query conditions contains invalid expression [SUM(Field)]");
        }

        [Theory]
        [InlineData(QueryOperator.IsNullOrEmpty, "COALESCE(Field,'') = ''")]
        [InlineData(QueryOperator.IsNotNullOrEmpty, "COALESCE(Field,'') <> ''")]
        [InlineData(QueryOperator.IsNull, "Field IS NULL")]
        [InlineData(QueryOperator.IsNotNull, "Field IS NOT NULL")]
        public void AppendQueryCondition_Fills_CommandText_Correctly_For_QueryOperator_Without_Value(QueryOperator queryOperator, string expectedCommandText)
        {
            // Arrange
            var sut = new DatabaseCommandBuilder();

            // Act
            sut.AppendQueryCondition(0, new QueryCondition("Field", queryOperator), new QueryProcessorSettings());

            // Assert
            sut.Build().CommandText.Should().Be(expectedCommandText);
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
            var sut = new DatabaseCommandBuilder();

            // Act
            sut.AppendQueryCondition(0, new QueryCondition("Field", queryOperator, "test"), new QueryProcessorSettings());
            var cmd = sut.Build();

            // Assert
            cmd.CommandText.Should().Be(expectedCommandText);
            cmd.CommandParameters.Should().BeOfType<Dictionary<string, object>>();
            var parameters = cmd.CommandParameters as Dictionary<string, object>;
            if (parameters != null)
            {
                parameters.Should().HaveCount(1);
                parameters.First().Key.Should().Be("p0");
                parameters.First().Value.Should().Be("test");
            }
        }
    }
}
