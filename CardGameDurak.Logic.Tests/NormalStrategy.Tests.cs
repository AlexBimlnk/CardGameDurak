using System.Collections.Generic;

using CardGameDurak.Abstractions;

using FluentAssertions;

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
        var strategy = new NormalStrategy();
        // Act
        var result = strategy.TryAttack(botCards, desktopCards, out var resultCard);

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
        var strategy = new NormalStrategy();

        // Act
        var result = strategy.TryDefence(ownerId, botCards, desktopCards, out var resultCard, out var resultClosedCard);

        // Assert
        result.Should().Be(expectedResult);
        resultCard.Should().BeEquivalentTo(expectedCard);
        resultClosedCard.Should().BeEquivalentTo(closedCard);
    }
    #endregion 
}
