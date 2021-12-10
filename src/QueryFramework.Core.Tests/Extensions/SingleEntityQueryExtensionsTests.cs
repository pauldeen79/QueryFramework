﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using Xunit;

namespace QueryFramework.Core.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class SingleEntityQueryExtensionsTests
    {
        [Fact]
        public void Too_Many_CloseBrackets_Leads_To_ValidationError()
        {
            // Arrange
            var action = new Action(() => _ = new SingleEntityQueryBuilder().Where("Field".DoesContain("Value", closeBracket: true)).Build());

            // Act
            var validationException = action.Should().Throw<ValidationException>().Which;

            // Assert
            validationException.ValidationResult.ErrorMessage.Should().Be("Too many brackets closed at condition: Field");
        }

        [Fact]
        public void Missing_CloseBrackets_Leads_To_ValidationError()
        {
            // Arrange
            var action = new Action(() => _ = new SingleEntityQueryBuilder().Where("Field".DoesContain("Value", openBracket: true)).Build());

            // Act
            var validationException = action.Should().Throw<ValidationException>().Which;

            // Assert
            validationException.ValidationResult.ErrorMessage.Should().Be("Missing close brackets, braket count should be 0 but remaining 1");
        }
    }
}
