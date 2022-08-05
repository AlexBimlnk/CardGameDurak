using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Стратегия, используемая ботом на средней сложности.
/// </summary>
public class MediumStrategy: IBotStrategy
{
    /// <inheritdoc/>
    public bool TryAttack(
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        int deckSize,
        out ICard? resultCard)
    {
        resultCard = null!;
        var simpleCards = handCards.Where(x => !x.IsTrump);
        if (desktopCards.Count == 0)
            resultCard = simpleCards.MinBy(x => x.Rank) ?? handCards.MinBy(x => x.Rank);
        resultCard ??= deckSize switch
        {
            > 11 => resultCard ??= simpleCards.Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                                              .MinBy(x => x.Rank),
            > 4 => resultCard ??= simpleCards.Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                                             .MinBy(x => x.Rank)
                              ?? handCards.Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                                          .Where(x => x.Rank < 10)
                                          .MinBy(x => x.Rank),
            _ => resultCard ??= simpleCards.Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                                           .MinBy(x => x.Rank)
                            ?? handCards.Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                                        .MinBy(x => x.Rank)
        };

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
        if (desktopCards is null || desktopCards.Count == 0)
            throw new ArgumentNullException(nameof(desktopCards));

        var needClosed = desktopCards.Where(x => x.Owner!.Id != ownerId)
                                     .First(x => !x.IsCloseOnDesktop);
        closedCard = needClosed;
        resultCard = handCards.Where(x => !x.IsTrump)
                              .Where(x => needClosed.Rank < x.Rank)
                              .Where(x => needClosed.Suit == x.Suit)
                              .MinBy(x => x.Rank);
        resultCard ??= deckSize switch
        {
            > 11 => handCards.Where(x => x.IsTrump)
                                    .Where(needClosed => !needClosed.IsTrump)
                                    .Where(x => x.Rank <= 10)
                                    .MinBy(x => x.Rank),
            > 4 => resultCard ??= handCards.Where(x => x.IsTrump)
                                    .Where(x => !needClosed.IsTrump)
                                    .MinBy(x => x.Rank)
                       ?? handCards.Where(x => x.IsTrump)
                                    .Where(x => needClosed.IsTrump)
                                    .Where(x => x.Rank > needClosed.Rank)
                                    .Where(x => needClosed.Rank <= 10)
                                    .MinBy(x => x.Rank),
            _ => resultCard ??= handCards.Where(x => x.IsTrump)
                                    .Where(x => !needClosed.IsTrump)
                                    .MinBy(x => x.Rank)
                       ?? handCards.Where(x => x.IsTrump)
                                    .Where(x => needClosed.IsTrump)
                                    .Where(x => x.Rank > needClosed.Rank)
                                    .MinBy(x => x.Rank)
        };
        return resultCard is not null;
    }
}
