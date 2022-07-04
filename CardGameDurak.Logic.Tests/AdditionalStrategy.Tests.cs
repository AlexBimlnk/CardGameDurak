﻿using System.Collections.Generic;

using CardGameDurak.Abstractions;

using FluentAssertions;

using Xunit;

namespace CardGameDurak.Logic.Tests;
public class AdditionalStrategyTests
{
    #region Методы
    [Theory(DisplayName = "Works attack additional strategy.")]
    [MemberData(nameof(AdditionalStrategyTestsData.TryAttackData), MemberType = typeof(AdditionalStrategyTestsData))]
    [Trait("Category", "Properties")]
    public void TryAttackWorks(
        ICard[] botCards,
        IReadOnlyCollection<ICard> desktopCards,
        int deckSize,
        ICard expectedCard,
        bool expectedResult)
    {
        // Arrange
        var strategy = new AdditionalStrategy();

        // Act
        var result = strategy.TryAttack(botCards, desktopCards, deckSize, out var resultCard);

        // Assert
        result.Should().Be(expectedResult);
        resultCard.Should().BeEquivalentTo(expectedCard);
    }

    [Theory(DisplayName = "Works defence additional strategy.")]
    [MemberData(nameof(AdditionalStrategyTestsData.TryDefenceData), MemberType = typeof(AdditionalStrategyTestsData))]
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
        var strategy = new AdditionalStrategy();

        // Act
        var result = strategy.TryDefence(ownerId, botCards, desktopCards, deckSize, out var resultCard, out var resultClosedCard);

        // Assert
        result.Should().Be(expectedResult);
        resultCard.Should().BeEquivalentTo(expectedCard);
        resultClosedCard.Should().BeEquivalentTo(closedCard);
    }
    #endregion 
}