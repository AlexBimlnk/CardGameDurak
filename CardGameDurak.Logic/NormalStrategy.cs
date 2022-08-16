using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Стратегия, используемая ботом на средней сложности.
/// </summary>
public sealed class NormalStrategy : IBotStrategy
{
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lnag = "ru">
    /// Когда список карт в руке (и дополнительно при защите на столе) равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException" xml:lnag = "ru">
    /// Когда кол-во карт в колоде меньше нуля или когда при защите у бота отсутствуют карты в руке, а также на поле
    /// </exception>
    
    /// <inheritdoc/>
    public bool TryAttack(
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        int deckSize,
        out ICard? resultCard)
    {
        ArgumentNullException.ThrowIfNull(handCards, nameof(handCards));
        if (deckSize < 0)
            throw new ArgumentException("Кол-во карт в колоде не может быть отрицательным");
        resultCard = null!;
        var simpleCards = handCards.Where(x => !x.IsTrump);
        if (desktopCards.Count == 0)
            resultCard = simpleCards.MinBy(x => x.Rank) ?? handCards.MinBy(x => x.Rank);
        resultCard ??= simpleCards.Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                                  .MinBy(x => x.Rank);

        return resultCard is not null;
    }

    /// <inheritdoc/>
    public bool TryDefence(
        int ownerId,
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        int deckSize,
        out ICard? resultCard,
        out ICard closedCard)
    {
        ArgumentNullException.ThrowIfNull(handCards, nameof(handCards));
        ArgumentNullException.ThrowIfNull(desktopCards, nameof(desktopCards));
        if (deckSize < 0)
            throw new ArgumentException("Кол-во карт в колоде не может быть отрицательным");
        if (handCards.Count == 0)
            throw new ArgumentException("Отсутствуют карты у бота, нечем защищаться!");
        if (desktopCards.Count == 0)
            throw new ArgumentException("Карт нет на столе");

        var needClosed = desktopCards.Where(x => x.Owner!.Id != ownerId)
                                     .First(x => !x.IsCloseOnDesktop);
        closedCard = needClosed;
        if (!needClosed.IsTrump)
        {
            resultCard = handCards.Where(x => needClosed.Rank < x.Rank)
                                  .Where(x => needClosed.Suit == x.Suit)
                                  .MinBy(x => x.Rank);
            resultCard ??= handCards.Where(x => x.IsTrump).MinBy(x => x.Rank);
        }
        else resultCard = null!;

        return resultCard is not null;
    }
}
