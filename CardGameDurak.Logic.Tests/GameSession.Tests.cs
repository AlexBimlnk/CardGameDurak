using System;
using System.Collections.Generic;
using System.Linq;

using CardGameDurak.Abstractions;

using FluentAssertions;

using Moq;

using Xunit;

namespace CardGameDurak.Logic.Tests;

public class GameSessionTests
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

    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Constructors")]
    public void CanBeCreated()
    {
        // Arrange
        var id = new GameSessionId(1);
        var deck = new List<ICard>() { Mock.Of<ICard>(MockBehavior.Strict) };
        var players = new List<IPlayer>() 
        { 
            Mock.Of<IPlayer>(MockBehavior.Strict),
            Mock.Of<IPlayer>(MockBehavior.Strict)
        };

        // Act
        var exception = Record.Exception(() => new GameSession(id, deck, players));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Can't be created when any argument is null.")]
    [Trait("Category", "Constructors")]
    public void CanNotBeCreatedWhenArgumnetsIsNull()
    {
        // Arrange

        // Act

        // Assert
        Assert.True(false);
    }

    [Fact(DisplayName = "Can't be created when players count less 2.")]
    [Trait("Category", "Constructors")]
    public void CanNotBeCreatedWhenCountPlayersLessTwo()
    {
        // Arrange

        // Act

        // Assert
        Assert.True(false);
    }
    #endregion

    #region Свойства

    [Fact(DisplayName = "Can get id.")]
    [Trait("Category", "Properties")]
    public void CanGetId()
    {
        // Arrange
        var id = new GameSessionId(1);
        var deck = new List<ICard>() { Mock.Of<ICard>(MockBehavior.Strict) };
        var players = new List<IPlayer>()
        {
            Mock.Of<IPlayer>(MockBehavior.Strict),
            Mock.Of<IPlayer>(MockBehavior.Strict)
        };
        var session = new GameSession(id, deck, players);
        var expectedResult = 1;

        // Act
        var result = session.Id;

        // Assert
        result.Should().Be(expectedResult);
    }
    #endregion

    #region Методы

    [Theory(DisplayName = "Can give cards.")]
    [MemberData(nameof(GiveCardsData), MemberType = typeof(GameSessionTests))]
    [Trait("Category", "Properties")]
    public void CanGiveCards(int deckSize, int countCards, int expectedCountCards)
    {
        // Arrange
        var id = new GameSessionId(1);
        var players = new List<IPlayer>()
        {
            Mock.Of<IPlayer>(MockBehavior.Strict),
            Mock.Of<IPlayer>(MockBehavior.Strict)
        };
        var deck = new List<ICard>(deckSize);
        foreach (var i in Enumerable.Range(0, deckSize))
            deck.Add(Mock.Of<ICard>(MockBehavior.Strict));

        var session = new GameSession(id, deck, players);

        // Act
        var result = session.GiveCards(countCards);

        // Assert
        result.Count().Should().Be(expectedCountCards);
    }

    [Fact(DisplayName = "Can't give cards when argunment is invalid.")]
    [Trait("Category", "Properties")]
    public void CanNotGiveCardsWhenArgumentIsInvalid()
    {
        // Arrange
        var id = new GameSessionId(1);
        var players = new List<IPlayer>()
        {
            Mock.Of<IPlayer>(MockBehavior.Strict),
            Mock.Of<IPlayer>(MockBehavior.Strict)
        };
        var deck = new List<ICard>();
        var session = new GameSession(id, deck, players);

        // Act

        // Assert

    }
    #endregion
}
