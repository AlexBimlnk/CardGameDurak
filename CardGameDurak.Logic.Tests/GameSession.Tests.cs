using System;
using System.Collections.Generic;
using System.Linq;

using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.GameSession;
using CardGameDurak.Abstractions.Players;

using FluentAssertions;

using Moq;

using Xunit;

namespace CardGameDurak.Logic.Tests;

public class GameSessionTests
{
    public static readonly TheoryData<int, int, int> GiveCardsData = new()
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

    private static List<ICard> CreateMockDeck(int size = 1)
    {
        var cards = new List<ICard>();

        foreach (var i in Enumerable.Range(0, size))
            cards.Add(Mock.Of<ICard>(MockBehavior.Strict));

        return cards;
    }
    private static List<IPlayer> CreateMockPlayers()
    {
        var playerStub1 = new Mock<IPlayer>(MockBehavior.Strict);
        playerStub1.SetupSet(x => x.Id = It.Is((int id) => 1 <= id));
        var playerStub2 = new Mock<IPlayer>(MockBehavior.Strict);
        playerStub2.SetupSet(x => x.Id = It.Is((int id) => 1 <= id));

        return new List<IPlayer>()
        {
            playerStub1.Object,
            playerStub2.Object
        };
    }

    #region Конструкторы

    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Constructors")]
    public void CanBeCreated()
    {
        // Arrange
        var id = new GameSessionId(1);
        var configuration = new GameSessionConfiguration
        {
            MinPlayersCount = 2,
            MaxPlayersCount = 2,
            DeckSize = 1
        };
        var deck = CreateMockDeck();
        var players = CreateMockPlayers();

        // Act
        var exception = Record.Exception(() => new GameSession(id, configuration, deck, players));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Can't be created when any argument is null.")]
    [Trait("Category", "Constructors")]
    public void CanNotBeCreatedWhenArgumnetsIsNull()
    {
        // Arrange
        var id = new GameSessionId(1);
        var configuration = new GameSessionConfiguration
        {
            MinPlayersCount = 2,
            MaxPlayersCount = 2,
            DeckSize = 1
        };
        var deck = CreateMockDeck();
        var players = CreateMockPlayers();

        // Act
        var exception1 = Record.Exception(() => new GameSession(id, configuration, null!, players));
        var exception2 = Record.Exception(() => new GameSession(id, configuration, deck, null!));

        // Assert
        new[] { exception1, exception2 }.Should().AllBeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can't be created without configuration.")]
    [Trait("Category", "Constructors")]
    public void CanNotBeCreatedWithoutConfiguration()
    {
        // Arrange
        var id = new GameSessionId(1);
        var configuration = new GameSessionConfiguration
        {
            MinPlayersCount = 2,
            MaxPlayersCount = 2,
            DeckSize = 1
        };
        var deck = CreateMockDeck();
        var players = CreateMockPlayers();

        // Act
        var exception = Record.Exception(() => new GameSession(id, null!, deck, players));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
    #endregion

    #region Свойства

    [Fact(DisplayName = "Can get id.")]
    [Trait("Category", "Properties")]
    public void CanGetId()
    {
        // Arrange
        var id = new GameSessionId(1);
        var configuration = new GameSessionConfiguration
        {
            MinPlayersCount = 2,
            MaxPlayersCount = 2,
            DeckSize = 1
        };
        var deck = CreateMockDeck();
        var players = CreateMockPlayers();
        var session = new GameSession(id, configuration, deck, players);

        // Act
        var result = session.Id;

        // Assert
        result.Should().Be(id);
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
        var configuration = new GameSessionConfiguration
        {
            MinPlayersCount = 2,
            MaxPlayersCount = 2,
            DeckSize = deckSize
        };
        var deck = CreateMockDeck(deckSize);
        var players = CreateMockPlayers();

        var session = new GameSession(id, configuration, deck, players);

        // Act
        var result = session.GiveCards(countCards);

        // Assert
        result.Count().Should().Be(expectedCountCards);
    }

    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    [Theory(DisplayName = "Can't give cards when argunment is invalid.")]
    [Trait("Category", "Properties")]
    public void CanNotGiveCardsWhenArgumentIsInvalid(int countCards)
    {
        // Arrange
        var id = new GameSessionId(1);
        var configuration = new GameSessionConfiguration
        {
            MinPlayersCount = 2,
            MaxPlayersCount = 2,
            DeckSize = 1
        };
        var players = CreateMockPlayers();
        var deck = CreateMockDeck();
        var session = new GameSession(id, configuration, deck, players);

        // Act
        var exception = Record.Exception(() => session.GiveCards(countCards));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentOutOfRangeException>();
    }
    #endregion
}
