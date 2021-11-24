using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Data.Core.Builders;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;
using QueryFramework.SqlServer.Tests.TestHelpers;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DatabaseCommandBuilderExtensionsTests
    {
        private DatabaseCommandBuilder Builder { get; }

        public DatabaseCommandBuilderExtensionsTests()
        {
            Builder = new DatabaseCommandBuilder();
        }

        [Fact]
        public void AppendSelectFields_Skips_SkipFields()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input.Where(x => x != "Field2"));
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendSelectFields(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("Field1, Field3");
        }

        [Fact]
        public void AppendSelectFields_Uses_CountOnly_When_Set_To_True()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input.Where(x => x != "Field2"));

            // Act
            var actual = Builder.AppendSelectFields(query, settingsMock.Object, fieldProviderMock.Object, countOnly: true);

            // Assert
            actual.Build().CommandText.Should().Be("COUNT(*)");
        }

        [Fact]
        public void AppendSelectFields_Uses_Star_When_Fields_Is_Empty_And_GetAllFieldsDelegate_Returns_Null_And_SelectAll_Is_True()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetAllFields())
                             .Returns(Enumerable.Empty<string>());

            // Act
            var actual = Builder.AppendSelectFields(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("*");
        }

        [Fact]
        public void AppendSelectFields_Uses_GetAllFieldsDelegate_When_Provided()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.Fields)
                                      .Returns(string.Empty);
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetAllFields())
                             .Returns(new[] { "Field1", "Field2", "Field3" });
            // Act
            var actual = Builder.AppendSelectFields(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("Field1, Field2, Field3");
        }

        [Fact]
        public void AppendSelectFields_Uses_Fields_From_Query_When_GetAllFields_Is_False()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendSelectFields(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("Field1, Field2, Field3");
        }

        [Fact]
        public void AppendSelectFields_Uses_GetFieldDelegate_When_Result_Is_Not_Null()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x + "A");
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendSelectFields(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("Field1A, Field2A, Field3A");
        }

        [Fact]
        public void AppendSelectFields_Throws_When_GetFieldDelegate_Returns_Null()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.ValidateFieldNames)
                        .Returns(true);
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns(default(string));
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            Builder.Invoking(x => x.AppendSelectFields(query,
                                                       settingsMock.Object,
                                                       fieldProviderMock.Object,
                                                       countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query fields contains unknown field in expression [Field1]");
        }

        [Fact]
        public void AppendSelectFields_Throws_When_ExpressionValidationDelegate_Returns_False()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(false);

            // Act
            Builder.Invoking(x => x.AppendSelectFields(query,
                                                       settingsMock.Object,
                                                       fieldProviderMock.Object,
                                                       countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query fields contains invalid expression [Field1]");
        }

        [Fact]
        public void AppendWhereClause_Does_Not_Append_Anything_When_Conditions_And_DefaultWhere_Are_Both_Empty()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act
            var actual = Builder.Append("SELECT...").AppendWhereClause(query, settingsMock.Object, fieldProviderMock.Object, out _);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Fact]
        public void AppendWhereClause_Adds_Single_Condition_Without_Default_Where_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendWhereClause(query, settingsMock.Object, fieldProviderMock.Object, out _);

            // Assert
            actual.Build().CommandText.Should().Be(" WHERE Field = @p0");
        }

        [Fact]
        public void AppendWhereClause_Adds_Single_Condition_With_Default_Where_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.DefaultWhere)
                        .Returns("Field IS NOT NULL");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendWhereClause(query, settingsMock.Object, fieldProviderMock.Object, out _);

            // Assert
            actual.Build().CommandText.Should().Be(" WHERE Field IS NOT NULL AND Field = @p0");
        }

        [Fact]
        public void AppendWhereClause_Adds_Multiple_Conditions_Without_Default_Where_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value"))
                                                        .And("Field2".IsNotNull())
                                                        .Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendWhereClause(query, settingsMock.Object, fieldProviderMock.Object, out _);

            // Assert
            actual.Build().CommandText.Should().Be(" WHERE Field = @p0 AND Field2 IS NOT NULL");
        }

        [Fact]
        public void AppendWhereClause_Adds_Multiple_Conditions_With_Default_Where_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).And("Field2".IsNotNull()).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.DefaultWhere)
                        .Returns("Field IS NOT NULL");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendWhereClause(query, settingsMock.Object, fieldProviderMock.Object, out _);

            // Assert
            actual.Build().CommandText.Should().Be(" WHERE Field IS NOT NULL AND Field = @p0 AND Field2 IS NOT NULL");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_Anything_When_Limit_And_Offset_Are_Both_Filled()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Limit(10).Offset(20).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.DefaultOrderBy)
                        .Returns("Ignored");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act
            var actual = Builder.Append("SELECT...").AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_Anything_When_CountOnly_Is_Set_To_True()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.DefaultOrderBy)
                        .Returns("Ignored");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act
            var actual = Builder.Append("SELECT...").AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: true);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_Anything_When_Query_OrderByFields_Is_Empty()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act
            var actual = Builder.Append("SELECT...").AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Fact]
        public void AppendOrderBy_Appends_Single_OrderBy_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY Field ASC");
        }

        [Fact]
        public void AppendOrderBy_Appends_Multiple_OrderBy_Clauses()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field1").ThenByDescending("Field2").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY Field1 ASC, Field2 DESC");
        }

        [Fact]
        public void AppendOrderBy_Appends_Default_OrderBy_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.DefaultOrderBy)
                        .Returns("Field ASC");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act
            var actual = Builder.AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY Field ASC");
        }

        [Fact]
        public void AppendOrderBy_Does_Not_Append_DefaultOrderBy_When_OrderBy_Clause_Is_Present_On_Query()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMoc = new Mock<IQueryProcessorSettings>();
            settingsMoc.SetupGet(x => x.DefaultOrderBy).Returns("Ignored");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendOrderByClause(query, settingsMoc.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY Field ASC");
        }

        [Fact]
        public void AppendOrderBy_Appends_Single_OrderBy_Clause_With_GetFieldNameDelegate()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x + "A");
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be(" ORDER BY FieldA ASC");
        }

        [Fact]
        public void AppendOrderBy_Throws_When_GetFieldNameDelegate_Returns_Null()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.ValidateFieldNames)
                        .Returns(true);
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act & Assert
            Builder.Invoking(x => x.AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query order by fields contains unknown field [Field]");
        }

        [Fact]
        public void AppendOrderBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldName_Returns_Null()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns(default(string));
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(false);

            // Act & Assert
            Builder.Invoking(x => x.AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query order by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendOrderBy_Throws_When_ExpressionValidationDelegate_Returns_False_And_GetFieldName_Returns_NonNullValue()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(false);

            // Act & Assert
            Builder.Invoking(x => x.AppendOrderByClause(query, settingsMock.Object, fieldProviderMock.Object, countOnly: false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query order by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendGroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Null()
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act
            var actual = Builder.Append("SELECT...").AppendGroupByClause(null, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Fact]
        public void AppendGroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Empty()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act
            var actual = Builder.Append("SELECT...").AppendGroupByClause(queryMock.Object, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Fact]
        public void AppendGroupBy_Appends_Single_GroupBy_Clause()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field") }));
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendGroupByClause(queryMock.Object, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().CommandText.Should().Be(" GROUP BY Field");
        }

        [Fact]
        public void AppendGroupBy_Appends_Multiple_GroupBy_Clauses()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field1"), new QueryExpression("Field2") }));
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendGroupByClause(queryMock.Object, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().CommandText.Should().Be(" GROUP BY Field1, Field2");
        }

        [Fact]
        public void AppendGroupBy_Appends_GroupBy_Clause_With_GetFieldNameDelegate()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field") }));
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x + "A");
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendGroupByClause(queryMock.Object, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().CommandText.Should().Be(" GROUP BY FieldA");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_GetFieldNameDelegate_Returns_Null()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field") }));
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.ValidateFieldNames)
                                      .Returns(true);
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns(default(string));
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act & Assert
            Builder.Invoking(x => x.AppendGroupByClause(queryMock.Object, settingsMock.Object, fieldProviderMock.Object))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query group by fields contains unknown field [Field]");
        }

        [Fact]
        public void AppendGroupBy_Throws_When_ExpressionValidationDelegate_Returns_False()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field") }));
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(false);

            // Act & Assert
            Builder.Invoking(x => x.AppendGroupByClause(queryMock.Object, settingsMock.Object, fieldProviderMock.Object))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query group by fields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendHaving_Adds_Single_Condition()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.HavingFields)
                     .Returns(new ValueCollection<IQueryCondition>(new[] { new QueryCondition("Field", QueryOperator.Equal, "value") }));
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);
            int paramCounter = 0;

            // Act
            var actual = Builder.AppendHavingClause(queryMock.Object, settingsMock.Object, fieldProviderMock.Object, ref paramCounter);

            // Assert
            actual.Build().CommandText.Should().Be(" HAVING Field = @p0");
        }

        [Fact]
        public void AppendHaving_Adds_Multiple_Conditions()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.HavingFields)
                     .Returns(new ValueCollection<IQueryCondition>(new[]
                     {
                         new QueryCondition("Field1", QueryOperator.Equal, "value1"),
                         new QueryCondition("Field2", QueryOperator.Equal, "value2")
                     }));
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);
            int paramCounter = 0;

            // Act
            var actual = Builder.AppendHavingClause(queryMock.Object, settingsMock.Object, fieldProviderMock.Object, ref paramCounter);

            // Assert
            actual.Build().CommandText.Should().Be(" HAVING Field1 = @p0 AND Field2 = @p1");
        }

        [Fact]
        public void AppendHaving_Does_Not_Append_Anything_When_HavingFields_Is_Null()
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            int paramCounter = 0;

            // Act
            var actual = Builder.Append("SELECT...").AppendHavingClause(null, settingsMock.Object, fieldProviderMock.Object, ref paramCounter);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Fact]
        public void AppendHaving_Does_Not_Append_Anything_When_HavingFields_Is_Empty()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            int paramCounter = 0;

            // Act
            var actual = Builder.Append("SELECT...").AppendHavingClause(queryMock.Object, settingsMock.Object, fieldProviderMock.Object, ref paramCounter);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Fact]
        public void AppendPagingPrefix_Returns_Correct_Result_On_Empty_OrderByFields()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().Offset(10).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act
            var actual = Builder.AppendPagingPrefix(query, settingsMock.Object, fieldProviderMock.Object, false);

            // Assert
            actual.Build().CommandText.Should().Be(", ROW_NUMBER() OVER (ORDER BY (SELECT 0)) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Returns_Correct_Result_On_Single_OrderByFields()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendPagingPrefix(query, settingsMock.Object, fieldProviderMock.Object, false);

            // Assert
            actual.Build().CommandText.Should().Be(", ROW_NUMBER() OVER (ORDER BY Field ASC) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Returns_Correct_Result_On_Multiple_OrderByFields()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().OrderBy("Field1").ThenBy("Field2").Offset(10).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendPagingPrefix(query, settingsMock.Object, fieldProviderMock.Object, false);

            // Assert
            actual.Build().CommandText.Should().Be(", ROW_NUMBER() OVER (ORDER BY Field1 ASC, Field2 ASC) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Uses_GetFieldNameDelegate_When_Provided()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x + "A");
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendPagingPrefix(query, settingsMock.Object, fieldProviderMock.Object, false);

            // Assert
            actual.Build().CommandText.Should().Be(", ROW_NUMBER() OVER (ORDER BY FieldA ASC) as sq_row_number");
        }

        [Fact]
        public void AppendPagingPrefix_Throws_When_GetFieldNameDelegate_Returns_Null()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.ValidateFieldNames)
                        .Returns(true);
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act & Assert
            Builder.Invoking(x => x.AppendPagingPrefix(query, settingsMock.Object, fieldProviderMock.Object, false))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query OrderByFields contains unknown field [Field]");
        }

        [Fact]
        public void AppendPagingPrefix_Throws_When_ExpressionValidationDelegate_Returns_False()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(false);

            // Act & Assert
            Builder.Invoking(x => x.AppendPagingPrefix(query, settingsMock.Object, fieldProviderMock.Object, false))
                          .Should().Throw<InvalidOperationException>()
                          .And.Message.Should().StartWith("Query OrderByFields contains invalid expression [Field]");
        }

        [Fact]
        public void AppendPagingSuffix_Does_Not_Append_Anything_When_CountOnly_Is_Set_To_True()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Build();

            // Act
            var actual = Builder.Append("SELECT...").AppendPagingSuffix(query, new Mock<IQueryProcessorSettings>().Object, true);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public void AppendPagingSuffix_Does_Not_Append_Anything_When_Offset_Is(int? offset)
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(offset).Build();

            // Act
            var actual = Builder.Append("SELECT...").AppendPagingSuffix(query, new Mock<IQueryProcessorSettings>().Object, false);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT...");
        }

        [Fact]
        public void AppendPagingSuffix_Returns_Correct_Result_When_Limit_Is_Greater_Than_Zero()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Limit(20).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();

            // Act
            var actual = Builder.AppendPagingSuffix(query, settingsMock.Object, false);

            // Assert
            actual.Build().CommandText.Should().Be(") sq WHERE sq.sq_row_number BETWEEN 11 and 30;");
        }

        [Fact]
        public void AppendPagingSuffix_Returns_Correct_Result_When_Limit_Is_Zero()
        {
            // Arrange
            var query = new SingleEntityQueryBuilder().OrderBy("Field").Offset(10).Limit(0).Build();
            var settingsMock = new Mock<IQueryProcessorSettings>();

            // Act
            var actual = Builder.AppendPagingSuffix(query, settingsMock.Object, false);

            // Assert
            actual.Build().CommandText.Should().Be(") sq WHERE sq.sq_row_number = 0;");
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(99, true)]
        public void AppendQueryCondition_Adds_Combination_Conditionally_But_Always_Increases_ParamCountner_When_ParamCounter_Is(int paramCounter, bool shouldAddCombination)
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.AppendQueryCondition(paramCounter,
                                                      new QueryCondition("Field", QueryOperator.Greater, "value"),
                                                      settingsMock.Object,
                                                      fieldProviderMock.Object);

            // Assert
            if (shouldAddCombination)
            {
                Builder.Build().CommandText.Should().Be($" AND Field > @p{paramCounter}");
            }
            else
            {
                Builder.Build().CommandText.Should().Be($"Field > @p{paramCounter}");
            }
            actual.Should().Be(paramCounter + 1);
        }

        [Fact]
        public void AppendQueryCondition_Adds_Brackets_When_Necessary()
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            Builder.AppendQueryCondition(0,
                                         new QueryCondition("Field", QueryOperator.Greater, "value", true, true),
                                         settingsMock.Object,
                                         fieldProviderMock.Object);

            // Assert
            Builder.Build().CommandText.Should().Be("(Field > @p0)");
        }

        [Fact]
        public void AppendQueryCondition_Gets_CustomFieldName_When_Possible()
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x == "Field" ? "CustomField" : x);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            Builder.AppendQueryCondition(0,
                                         new QueryCondition("Field", QueryOperator.Greater, "value"),
                                         settingsMock.Object,
                                         fieldProviderMock.Object);

            // Assert
            Builder.Build().CommandText.Should().Be("CustomField > @p0");
        }

        [Fact]
        public void AppendQueryCondition_Throws_On_Invalid_CustomFieldName_When_ValidateFieldNames_Is_Set_To_True()
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.ValidateFieldNames)
                        .Returns(true);
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x == "Field" ? null : x);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            Builder.Invoking(x => x.AppendQueryCondition(0,
                                                         new QueryCondition("Field", QueryOperator.Greater, "value"),
                                                         settingsMock.Object,
                                                         fieldProviderMock.Object))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query conditions contains unknown field [Field]");
        }

        [Fact]
        public void AppendQueryCondition_Throws_On_Invalid_Expression_When_ExpressionValidationDelegate_Returns_False()
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(false);

            // Act
            Builder.Invoking(x => x.AppendQueryCondition(0,
                                                         new QueryCondition(new QueryExpression("Field", "SUM({0})"), QueryOperator.Greater, "value"),
                                                         settingsMock.Object,
                                                         fieldProviderMock.Object))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query conditions contains invalid expression [SUM(Field)]");
        }

        [Theory]
        [InlineData(QueryOperator.IsNullOrEmpty, "COALESCE(Field,'') = ''")]
        [InlineData(QueryOperator.IsNotNullOrEmpty, "COALESCE(Field,'') <> ''")]
        [InlineData(QueryOperator.IsNull, "Field IS NULL")]
        [InlineData(QueryOperator.IsNotNull, "Field IS NOT NULL")]
        public void AppendQueryCondition_Fills_CommandText_Correctly_For_QueryOperator_Without_Value(QueryOperator queryOperator, string expectedCommandText)
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            Builder.AppendQueryCondition(0,
                                         new QueryCondition("Field", queryOperator),
                                         settingsMock.Object,
                                         fieldProviderMock.Object);

            // Assert
            Builder.Build().CommandText.Should().Be(expectedCommandText);
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
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            Builder.AppendQueryCondition(0,
                                         new QueryCondition("Field", queryOperator, "test"),
                                         settingsMock.Object,
                                         fieldProviderMock.Object);
            var actual = Builder.Build();

            // Assert
            actual.CommandText.Should().Be(expectedCommandText);
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            parameters.Should().HaveCount(1);
            if (parameters?.Count == 1)
            {
                parameters.First().Key.Should().Be("p0");
                parameters.First().Value.Should().Be("test");
            }
        }

        [Fact]
        public void AppendPagingOuterQuery_Adds_Prefix_To_CommandText_When_Query_Has_Offset()
        {
            // Arrange
            var queryMock = new Mock<IFieldSelectionQuery>();
            queryMock.SetupGet(x => x.GetAllFields).Returns(true);
            queryMock.SetupGet(x => x.Offset).Returns(10);
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetAllFields())
                             .Returns(Enumerable.Empty<string>());

            // Act
            var actual = Builder.AppendPagingOuterQuery(queryMock.Object, settingsMock.Object, fieldProviderMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT * FROM (");
        }

        [Fact]
        public void AppendSelectAndDistinctClause_Adds_Select_When_Distinct_Is_False()
        {
            // Arrange
            var queryMock = new Mock<IFieldSelectionQuery>();
            queryMock.SetupGet(x => x.GetAllFields).Returns(true);

            // Act
            var actual = Builder.AppendSelectAndDistinctClause(queryMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT ");
        }

        [Fact]
        public void AppendSelectAndDistinctClause_Adds_Select_Distinct_When_Distinct_Is_True()
        {
            // Arrange
            var queryMock = new Mock<IFieldSelectionQuery>();
            queryMock.SetupGet(x => x.GetAllFields).Returns(true);
            queryMock.SetupGet(x => x.Distinct).Returns(true);

            // Act
            var actual = Builder.AppendSelectAndDistinctClause(queryMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("SELECT DISTINCT ");
        }

        [Fact]
        public void AppendTopClause_Appends_Top_Clause_When_Offset_Is_Empty_And_Limit_Is_Filled_On_Query()
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            var query = new SingleEntityQueryBuilder().Take(10).Build();

            // Act
            var actual = Builder.AppendTopClause(query, settingsMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("TOP 10 ");
        }

        [Fact]
        public void AppendTopClause_Appends_Top_Clause_When_Offset_Is_Empty_And_Limit_Is_Filled_On_Settings()
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.OverrideLimit).Returns(10);
            var query = new SingleEntityQueryBuilder().Build();

            // Act
            var actual = Builder.AppendTopClause(query, settingsMock.Object, countOnly: false);

            // Assert
            actual.Build().CommandText.Should().Be("TOP 10 ");
        }

        [Fact]
        public void AddQueryParameters_Adds_QueryParameters_When_Found()
        {
            // Arrange
            var query = new ParameterizedQueryMock(new[] { new QueryParameter("name", "Value") });

            // Act
            var actual = Builder.AddQueryParameters(query);

            // Assert
            actual.CommandParameters.Should().HaveCount(1);
            actual.CommandParameters.First().Key.Should().Be("name");
            actual.CommandParameters.First().Value.Should().Be("Value");
        }
    }
}
