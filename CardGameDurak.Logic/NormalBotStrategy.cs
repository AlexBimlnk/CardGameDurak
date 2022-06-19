using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Стратегия, используемая ботом на средней сложности.
/// </summary>
public sealed class NormalStrategy : IBotStrategy
{
    private ICard? FindOptimal(
        IReadOnlyCollection<ICard> desktopCards,
        IReadOnlyCollection<ICard> handCards,
        bool withTrump) =>
        handCards.Where(x =>
                        desktopCards.Count != 0
                        ? desktopCards.Any(y => x.Rank == y.Rank)
                        : true)
                 .Where(x => x.IsTrump == withTrump)
                 .MinBy(x => x.Rank);

    /// <inheritdoc/>
    public bool TryAttack(
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        out ICard? resultCard)
    {
        resultCard = FindOptimal(desktopCards, handCards, withTrump: false);

        if (desktopCards.Count == 0)
            resultCard ??= handCards.MinBy(x => x.Rank);

        return resultCard is not null;
    }

    /// <inheritdoc/>
    public bool TryDefence(
        int ownerId,
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        out ICard resultCard,
        out ICard closedCard)
    {
        if (desktopCards is null || desktopCards.Count == 0)
            throw new ArgumentNullException(nameof(desktopCards));

        throw new NotImplementedException();

        //ICard myCard = handCards.Where(card =>
        //{
        //    return desktopCards.Where(card => card.Owner!.Id != ownerId)
        //                       .Any(x => x.Rank < card.Rank && x.Suit == card.Suit
        //                       || !x.IsTrump && card.IsTrump);
        //})
        //                       .First();

        //closedCard = desktopCards.Where(card => desktopCards.Where(owner => owner.Owner.Id != Id)
        //                      .Any(card => myCard.Rank > card.Rank || card.IsTrump))
        //                      .First();
        //return myCard;
    }
}
