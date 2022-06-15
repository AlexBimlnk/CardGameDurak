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
    public readonly static TheoryData<int, int, int> GiveCardsData = new()
    {
        {
            5,
            3,
            3
        },
        {
            5,
            6,
            5
        },
        {
            3,
            5,
            3
        }
    };
    #region Конструкторы
    [Fact(DisplayName = "Bot is Created.")]
    [Trait("Category", "Constructors")]
    public void BotCreated()
    {
        // Arrange
        var name = " ";

        // Act
        var exception = new Exception();
        if (string.IsNullOrWhiteSpace(name))
            exception = null;
        else
            exception = Record.Exception(() => new Bot(name));
        // Assert
        exception.Should().BeNull();
    }
    #endregion

    #region Методы
    [Theory(DisplayName = "Bot is attacking player")]
    [MemberData(nameof(GiveCardsData), MemberType = typeof(BotTests))]
    [Trait("Category", "Properties")]
    public void CanAttack(IReadOnlyCollection<ICard> desktopCards, ICard expectedCard)
    {
        // Arrange
        var name = "BotName";

        var bot = new Bot(name);
        var suit = Suit.Spades;
        var rank = 0;
        //expectedCard = new Card(suit, rank);
        // Act
        var result = bot.Attaсk(desktopCards);
        // Assert
        result.Should().Be(expectedCard);
    }

    [Theory(DisplayName = "Bot is defending from outCard")]
    [MemberData(nameof(GiveCardsData), MemberType = typeof(BotTests))]
    [Trait("Category", "Properties")]
    public void CanDefence(IReadOnlyCollection<ICard> desktopCards, ICard expectedCard, ICard closedCard)
    {
        // Arrange
        var name = "BotName";

        var bot = new Bot(name);
        var suit = Suit.Spades;
        var rank = 0;
        //expectedCard = new Card(suit, rank);
        // Act
        var result = bot.Defence(desktopCards, out var outCard);
        // Assert
        result.Should().Be(expectedCard);
        outCard.Should().Be(closedCard);
    }
    #endregion 
}
