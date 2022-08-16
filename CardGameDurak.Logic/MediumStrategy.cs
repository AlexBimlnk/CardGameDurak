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
    private const int MAX_POSSIBLE_RANK_TRUMP_CARD = 10;

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
        resultCard ??= deckSize switch
        {
            > 11 => resultCard ??= simpleCards
                    .Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                    .MinBy(x => x.Rank),
            > 4 => resultCard ??= simpleCards
                    .Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                    .MinBy(x => x.Rank)
                        ?? handCards
                            .Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                            .Where(x => x.Rank <= MAX_POSSIBLE_RANK_TRUMP_CARD)
                            .MinBy(x => x.Rank),
            _ => resultCard ??= simpleCards
                    .Where(x => desktopCards.Any(y => x.Rank == y.Rank))
                    .MinBy(x => x.Rank)
                        ?? handCards
                            .Where(x => desktopCards.Any(y => x.Rank == y.Rank))
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
        ArgumentNullException.ThrowIfNull(handCards, nameof(handCards));
        ArgumentNullException.ThrowIfNull(desktopCards, nameof(desktopCards));
        if (deckSize < 0)
            throw new ArgumentException("Кол-во карт в колоде не может быть отрицательным");
        if (handCards.Count == 0)
            throw new ArgumentException("Отсутствуют карты у бота, нечем защищаться!");
        if (desktopCards.Count == 0)
            throw new ArgumentException("Карт нет на столе");
        var needClosed = desktopCards
                        .Where(x => x.Owner!.Id != ownerId)
                        .First(x => !x.IsCloseOnDesktop);
        closedCard = needClosed;
        resultCard = handCards
                    .Where(x => !x.IsTrump)
                    .Where(x => needClosed.Rank < x.Rank)
                    .Where(x => needClosed.Suit == x.Suit)
                    .MinBy(x => x.Rank);
        resultCard ??= deckSize switch
        {
            > 11 => handCards
                    .Where(x => x.IsTrump)
                    .Where(needClosed => !needClosed.IsTrump)
                    .Where(x => x.Rank <= MAX_POSSIBLE_RANK_TRUMP_CARD)
                    .MinBy(x => x.Rank),
            > 4 => resultCard ??= handCards
                    .Where(x => x.IsTrump)
                    .Where(x => !needClosed.IsTrump)
                    .MinBy(x => x.Rank)
                        ?? handCards
                            .Where(x => x.IsTrump)
                            .Where(x => needClosed.IsTrump)
                            .Where(x => x.Rank > needClosed.Rank)
                            .Where(x => needClosed.Rank <= MAX_POSSIBLE_RANK_TRUMP_CARD)
                            .MinBy(x => x.Rank),
            _ => resultCard ??= handCards
                    .Where(x => x.IsTrump)
                    .Where(x => !needClosed.IsTrump)
                    .MinBy(x => x.Rank)
                        ?? handCards
                            .Where(x => x.IsTrump)
                            .Where(x => needClosed.IsTrump)
                            .Where(x => x.Rank > needClosed.Rank)
                            .MinBy(x => x.Rank)
        };
        return resultCard is not null;
    }
}
