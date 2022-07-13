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
public class CardTests
{
    #region Методы

    [Fact(DisplayName = "Cards are equivalent")]
    [Trait("Category", "Properties")]
    public void CardsEquals()
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

    [Fact(DisplayName = "Cards are not equivalent when argument is invalid")]
    [Trait("Category", "Properties")]
    public void SessionsDoesNotEqualsWhenArgumentisInvalid()
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
        Assert.True(false);
    }
    #endregion
}
