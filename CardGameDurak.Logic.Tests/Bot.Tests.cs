using System;
using System.Collections.Generic;
using System.Linq;
using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using FluentAssertions;
using Moq;
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

        // Act
        var exception = Record.Exception(() => new Bot(name));

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
        // Act
        var exception = Record.Exception(() => new Bot(name));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
    #endregion

    #region Методы
    [Theory(DisplayName = "Bot can attacking player.")]
    [MemberData(nameof(BotTestsData.CanAttackData), MemberType = typeof(BotTestsData))]
    [Trait("Category", "Properties")]
    public void CanAttack(IReadOnlyCollection<ICard> desktopCards, ICard expectedCard)
    {
        // Arrange
        var name = "BotName";
        var bot = new Bot(name);

        // Act
        var result = bot.Attaсk(desktopCards);

        // Assert
        result.Should().Be(expectedCard);
    }

    [Theory(DisplayName = "Bot can defending from outCard")]
    [MemberData(nameof(BotTestsData.CanDefenceData), MemberType = typeof(BotTestsData))]
    [Trait("Category", "Properties")]
    public void CanDefence(IReadOnlyCollection<ICard> desktopCards, ICard closedCard, ICard expectedCard)
    {
        // Arrange
        var name = "BotName";
        var bot = new Bot(name);
        // Act
        var result = bot.Defence(desktopCards, out var outCard);

        // Assert
        result.Should().Be(expectedCard);
        outCard.Should().Be(closedCard);
    }
    #endregion 
}
