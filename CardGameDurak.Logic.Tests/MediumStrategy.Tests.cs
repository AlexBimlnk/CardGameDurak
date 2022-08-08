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
public class MediumStrategyTests
{

    #region Методы
    [Theory(DisplayName = "Works attack additional strategy.")]
    [MemberData(nameof(MediumStrategyTestsData.TryAttackData), MemberType = typeof(MediumStrategyTestsData))]
    [Trait("Category", "Properties")]
    public void TryAttackWorks(
        ICard[] botCards,
        IReadOnlyCollection<ICard> desktopCards,
        int deckSize,
        ICard expectedCard,
        bool expectedResult)
    {
        // Arrange
        var strategy = new MediumStrategy();

        // Act
        var result = strategy.TryAttack(botCards, desktopCards, deckSize, out var resultCard);

        // Assert
        result.Should().Be(expectedResult);
        resultCard.Should().BeEquivalentTo(expectedCard);
    }

    [Theory(DisplayName = "Works defence additional strategy.")]
    [MemberData(nameof(MediumStrategyTestsData.TryDefenceData), MemberType = typeof(MediumStrategyTestsData))]
    [Trait("Category", "Properties")]
    public void CanDefence(
        ICard[] botCards,
        IReadOnlyCollection<ICard> desktopCards,
        int deckSize,
        ICard closedCard,
        ICard expectedCard,
        bool expectedResult)
    {
        // Arrange
        var ownerId = 1;
        var strategy = new MediumStrategy();

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
        var botCards = Array.Empty<ICard>(); // как тут создать массив mockов?
        List<ICard> desktopCards = null!;
        var decksize = 0;
        ICard closedCard = Mock.Of<ICard>(MockBehavior.Strict);
        ICard expectedCard = Mock.Of<ICard>(MockBehavior.Strict);
        var expectedResult = false;

        // Act
        var exception = Record.Exception(() => CanDefence(botCards, desktopCards, decksize, closedCard, expectedCard, expectedResult));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Bot can not defence cause count of desktopCards is 0")]
    [Trait("Category", "Properties")]
    public void CanNotDefenceWhenDesktopCountIsZero()
    {
        // Arrange
        var botCards = Array.Empty<ICard>(); // Аналогично
        var desktopCards = new List<ICard>();
        var decksize = 0;
        ICard closedCard = Mock.Of<ICard>(MockBehavior.Strict);
        ICard expectedCard = Mock.Of<ICard>(MockBehavior.Strict);
        var expectedResult = false;

        // Act
        var exception = Record.Exception(() => CanDefence(botCards, desktopCards, decksize, closedCard, expectedCard, expectedResult));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
    }
    #endregion 
}