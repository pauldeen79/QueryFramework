﻿namespace QueryFramework.Core.Tests.Extensions;

public class SingleEntityQueryExtensionsTests
{
    //[Fact]
    //public void Too_Many_CloseBrackets_Leads_To_ValidationError()
    //{
    //    // Arrange
    //    var action = new Action(() => _ = new SingleEntityQueryBuilder().Where("Field".DoesContain("Value").WithCloseBracket()).Build());

    //    // Act
    //    var validationException = action.Should().Throw<ValidationException>().Which;

    //    // Assert
    //    validationException.ValidationResult.ErrorMessage.Should().Be("Too many brackets closed at condition: Field");
    //}

    //[Fact]
    //public void Missing_CloseBrackets_Leads_To_ValidationError()
    //{
    //    // Arrange
    //    var action = new Action(() => _ = new SingleEntityQueryBuilder().Where("Field".DoesContain("Value").WithOpenBracket()).Build());

    //    // Act
    //    var validationException = action.Should().Throw<ValidationException>().Which;

    //    // Assert
    //    validationException.ValidationResult.ErrorMessage.Should().Be("Missing close brackets, braket count should be 0 but remaining 1");
    //}
}
