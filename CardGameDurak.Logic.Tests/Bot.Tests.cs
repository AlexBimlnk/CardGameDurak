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
        var name = new String("Имя бота");

        // Act
        var exception = Record.Exception(() => new Bot(name));

        // Assert
        exception.Should().BeNull();
    }
    #endregion

    #region Методы
    [Theory(DisplayName = "Bot is attacking player")]
    [MemberData(nameof(GiveCardsData), MemberType = typeof(BotTests))]
    [Trait("Category", "Properties")]
    public void Attack(IReadOnlyCollection<ICard> desktopCards)
    {
        // Arrange
        var name = new String("Имя бота");

        var bot = new Bot(name);
        var suit = new Suit();
        var rank = 0;
        var expectedCard = new Card(suit, rank);
        // Act
        var result = bot.Attaсk(desktopCards);
        // Assert
        result.Should().Be(expectedCard);
    }

    [Theory(DisplayName = "Bot is defending from outCard")]
    [MemberData(nameof(GiveCardsData), MemberType = typeof(BotTests))]
    [Trait("Category", "Properties")]
    public void Defence(IReadOnlyCollection<ICard> desktopCards, out ICard outCard)
    {
        // Arrange
        var name = new String("Имя бота");

        var bot = new Bot(name);
        var suit = new Suit();
        //var suit = new Mock.Of<Suit>(MockBehavior.Strict);
        var rank = 0;
        var expectedCard = new Card(suit, rank);
        // Act
        outCard = new Card(suit, rank);
        var result = bot.Defence(desktopCards, out outCard);
        // Assert
        result.Should().Be(expectedCard);
    }
    #endregion 
}
