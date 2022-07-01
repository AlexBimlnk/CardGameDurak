using System;
using System.Collections.Generic;

using CardGameDurak.Abstractions;

using FluentAssertions;

using Xunit;

namespace CardGameDurak.Logic.Tests;
public class BotTests
{
    #region Конструкторы
    [Fact(DisplayName = "Bot can be created.")]
    [Trait("Category", "Constructors")]
    public void BotCanBeCreated()
    {
        // Arrange
        var name = "BotName";
        var strategy = new NormalStrategy();

        // Act
        var exception = Record.Exception(() => new Bot(name, strategy));

        // Assert
        exception.Should().BeNull();
    }

    [InlineData(null!)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(" \t\r \n ")]
    [Theory(DisplayName = "Bot can't be created when name is null or empty.")]
    [Trait("Category", "Constructors")]
    public void BotCanNotBeCreatedWhenNameIsMissing(string name)
    {
        // Arrange
        var strategy = new NormalStrategy();

        // Act
        var exception = Record.Exception(() => new Bot(name, strategy));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Bot can't be created when strategy is null.")]
    [Trait("Category", "Constructors")]
    public void BotCanNotBeCreatedWhenStategyIsNull()
    {
        // Arrange
        var name = "BotName";

        // Act
        var exception = Record.Exception(() => new Bot(name, null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
    #endregion

    #region Методы

    #endregion 
}
