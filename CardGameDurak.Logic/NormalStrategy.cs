using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Стратегия, используемая ботом на средней сложности.
/// </summary>
public sealed class NormalStrategy : IBotStrategy
{
    /// <inheritdoc/>
    public bool TryAttack(
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        out ICard? resultCard)
    {
        resultCard = handCards.Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                              .Where(x => !x.IsTrump)
                              .MinBy(x => x.Rank);

        if (desktopCards.Count == 0)
            resultCard ??= handCards.MinBy(x => x.Rank);

        return resultCard is not null;
    }

    /// <inheritdoc/>
    public bool TryDefence(
        int ownerId,
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        out ICard? resultCard,
        out ICard closedCard)
    {
        if (desktopCards is null || desktopCards.Count == 0)
            throw new ArgumentNullException(nameof(desktopCards));

        var needClosed = desktopCards.Where(x => x.Owner!.Id != ownerId)
                                     .Single(x => !x.IsCloseOnDesktop);
        closedCard = needClosed;

        resultCard = handCards.Where(x => needClosed.Rank < x.Rank)
                              .Where(x => needClosed.Suit == x.Suit)
                              .FirstOrDefault();

        return resultCard is not null;
    }
}
