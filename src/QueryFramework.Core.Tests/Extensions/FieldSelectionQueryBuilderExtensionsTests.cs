namespace QueryFramework.Core.Tests.Extensions;

public class FieldSelectionQueryBuilderExtensionsTests
{
    [Fact]
    public void Can_Use_Select_With_QueryExpressionBuilder_To_Add_Field()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Select(new FieldExpressionBuilder().WithFieldName("FieldName"));

        // Assert
        actual.Fields.Should().HaveCount(1);
        var field = actual.Fields.First() as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field!.FieldNameExpression).Value.Should().Be("FieldName");
        actual.Distinct.Should().BeFalse();
    }

    [Fact]
    public void Can_Use_Select_With_FieldName_String_To_Add_Field()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Select("FieldName");

        // Assert
        actual.Fields.Should().HaveCount(1);
        var field = actual.Fields.First() as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field!.FieldNameExpression).Value.Should().Be("FieldName");
        actual.Distinct.Should().BeFalse();
    }

    [Fact]
    public void Can_Use_Select_With_FieldName_String_To_Add_Multiple_Fields()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Select("FieldName1", "FieldName2");

        // Assert
        actual.Fields.Should().HaveCount(2);
        var firstField = actual.Fields.First() as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)firstField!.FieldNameExpression).Value.Should().Be("FieldName1");
        var lastField = actual.Fields.Last() as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)lastField!.FieldNameExpression).Value.Should().Be("FieldName2");
        actual.Distinct.Should().BeFalse();
    }

    [Fact]
    public void Can_Use_SelectAll_To_Clear_Fields_And_Set_GetAllFields_To_True()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder().Select("FieldName1", "FieldName2");

        // Act
        var actual = sut.SelectAll();

        // Assert
        actual.Fields.Should().BeEmpty();
        actual.GetAllFields.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_SelectDistinct_With_QueryExpressionBuilder_To_Add_Field()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.SelectDistinct(new FieldExpressionBuilder().WithFieldName("FieldName"));

        // Assert
        actual.Fields.Should().HaveCount(1);
        var field = actual.Fields.First() as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field!.FieldNameExpression).Value.Should().Be("FieldName");
        actual.Distinct.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_SelectDistinct_With_FieldName_String_To_Add_Field()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.SelectDistinct("FieldName");

        // Assert
        actual.Fields.Should().HaveCount(1);
        var field = actual.Fields.First() as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field!.FieldNameExpression).Value.Should().Be("FieldName");
        actual.Distinct.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_SelectDistinct_With_FieldName_Strings_To_Add_Multiple_Fields()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.SelectDistinct("FieldName1", "FieldName2");

        // Assert
        actual.Fields.Should().HaveCount(2);
        var firstField = actual.Fields.First() as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)firstField!.FieldNameExpression).Value.Should().Be("FieldName1");
        var lastField = actual.Fields.Last() as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)lastField!.FieldNameExpression).Value.Should().Be("FieldName2");
        actual.Distinct.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_Distinct_To_Set_Distinct()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Distinct();

        // Assert
        actual.Distinct.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_GetAllFields_To_Set_GetAllFields()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.GetAllFields();

        // Assert
        actual.GetAllFields.Should().BeTrue();
    }
}
