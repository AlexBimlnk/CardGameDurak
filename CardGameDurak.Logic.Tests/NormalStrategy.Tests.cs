using System;
using System.Collections.Generic;

using CardGameDurak.Abstractions;

using FluentAssertions;

using Moq;

using Xunit;

namespace CardGameDurak.Logic.Tests;
public class NormalStrategyTests
{
    #region Методы
    [Theory(DisplayName = "Works attack strategy.")]
    [MemberData(nameof(NormalStrategyTestsData.TryAttackData), MemberType = typeof(NormalStrategyTestsData))]
    [Trait("Category", "Properties")]
    public void TryAttackWorks(
        ICard[] botCards,
        IReadOnlyCollection<ICard> desktopCards,
        ICard expectedCard,
        bool expectedResult)
    {
        // Arrange
        var deckSize = 24;
        var strategy = new NormalStrategy();

        // Act
        var result = strategy.TryAttack(botCards, desktopCards, deckSize, out var resultCard);

        // Assert
        result.Should().Be(expectedResult);
        resultCard.Should().BeEquivalentTo(expectedCard);
    }

    [Theory(DisplayName = "Works defence strategy.")]
    [MemberData(nameof(NormalStrategyTestsData.TryDefenceData), MemberType = typeof(NormalStrategyTestsData))]
    [Trait("Category", "Properties")]
    public void CanDefence(
        ICard[] botCards,
        IReadOnlyCollection<ICard> desktopCards,
        ICard closedCard,
        ICard expectedCard,
        bool expectedResult)
    {
        // Arrange
        var ownerId = 1;
        var deckSize = 24;
        var strategy = new NormalStrategy();

        // Act
        var result = strategy.TryDefence(ownerId, botCards, desktopCards, deckSize, out var resultCard, out var resultClosedCard);

        // Assert
        result.Should().Be(expectedResult);
        resultCard.Should().BeEquivalentTo(expectedCard);
        resultClosedCard.Should().BeEquivalentTo(closedCard);
    }

    [Fact(DisplayName = "Bot can not defence cause desktop is null.")]
    [Trait("Category", "Properties")]
    public void CanNotDefenceWhenDesktopIsNull()
    {
        // Arrange
        var ownerId = 1;
        var botCards = Array.Empty<ICard>();
        ICard[] desktopCards = null!;
        var deckSize = 20;
        var strategy = new NormalStrategy();

        // Act
        var exception = Record.Exception(() => strategy.TryDefence(ownerId, botCards, desktopCards, deckSize, out var resultCard, out var resultClosedCard));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Bot can not defence cause count of desktopCards is 0")]
    [Trait("Category", "Properties")]
    public void CanNotDefenceWhenDesktopCountIsZero()
    {
        // Arrange
        var ownerId = 1;
        var botCards = Array.Empty<ICard>();
        var desktopCards = Array.Empty<ICard>();
        var deckSize = 20;
        var strategy = new NormalStrategy();

        // Act
        var exception = Record.Exception(() => strategy.TryDefence(ownerId, botCards, desktopCards, deckSize, out var resultCard, out var resultClosedCard));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = "Bot can not defence cause there are not hand cards")]
    [Trait("Category", "Properties")]
    public void CanNotDefenceWhenHandIsNull()
    {
        // Arrange
        var ownerId = 1;
        ICard[] botCards = null!;
        var desktopCards = Array.Empty<ICard>();
        var deckSize = 20;
        var strategy = new NormalStrategy();

        // Act
        var exception = Record.Exception(() => strategy.TryDefence(ownerId, botCards, desktopCards, deckSize, out var resultCard, out var resultClosedCard));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Bot can not defence cause count of handCards is 0")]
    [Trait("Category", "Properties")]
    public void CanNotDefenceWhenHandCountIsZero()
    {
        // Arrange
        var ownerId = 1;
        var botCards = Array.Empty<ICard>();
        var desktopCards = Array.Empty<ICard>();
        var deckSize = 20;
        var strategy = new NormalStrategy();

        // Act
        var exception = Record.Exception(() => strategy.TryDefence(ownerId, botCards, desktopCards, deckSize, out var resultCard, out var resultClosedCard));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = "Bot can not do anything cause count of deck cards is less 0")]
    [Trait("Category", "Properties")]
    public void CanNotDefenceWhenDeckIsLessZero()
    {
        // Arrange
        var ownerId = 1;
        var botCards = Array.Empty<ICard>();
        var desktopCards = Array.Empty<ICard>();
        var deckSize = -1;
        var strategy = new NormalStrategy();

        // Act
        var exception1 = Record.Exception(() => strategy.TryAttack(botCards, desktopCards, deckSize, out var resultCard));
        var exception2 = Record.Exception(() => strategy.TryDefence(ownerId, botCards, desktopCards, deckSize, out var resultCard, out var resultClosedCard));

        // Assert
        new[] { exception1, exception2 }.Should().AllBeOfType<ArgumentException>();
    }
    #endregion
}
