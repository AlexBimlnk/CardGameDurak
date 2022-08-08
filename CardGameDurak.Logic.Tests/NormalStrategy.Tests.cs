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
        var botCards = Array.Empty<ICard>(); // как тут создать массив mockов?
        List<ICard> desktopCards = null!;
        ICard closedCard = Mock.Of<ICard>(MockBehavior.Strict);
        ICard expectedCard = Mock.Of<ICard>(MockBehavior.Strict);
        var expectedResult = false;

        // Act
        var exception = Record.Exception(() => CanDefence(botCards, desktopCards, closedCard, expectedCard, expectedResult));

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
        ICard closedCard = Mock.Of<ICard>(MockBehavior.Strict);
        ICard expectedCard = Mock.Of<ICard>(MockBehavior.Strict);
        var expectedResult = false;

        // Act
        var exception = Record.Exception(() => CanDefence(botCards, desktopCards, closedCard, expectedCard, expectedResult));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        #endregion
    }
}
