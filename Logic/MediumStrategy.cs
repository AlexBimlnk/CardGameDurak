namespace Logic;

/// <summary>
/// Стратегия, используемая ботом на средней сложности.
/// </summary>
public sealed class MediumStrategy : IBotStrategy
{
    private const CardType MAX_POSSIBLE_TYPE_TRUMP_CARD = CardType.Ten;

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Когда список карт находящихся в руке или столе равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Когда <paramref name="deckSize"/> меньше нуля.
    /// </exception>
    public bool TryAttack(
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        int deckSize,
        out ICard? resultCard)
    {
        ArgumentNullException.ThrowIfNull(handCards, nameof(handCards));
        ArgumentNullException.ThrowIfNull(desktopCards);

        if (deckSize < 0)
            throw new ArgumentException("Кол-во карт в колоде не может быть отрицательным");

        resultCard = null!;
        var simpleCards = handCards.Where(x => !x.IsTrump);
        if (desktopCards.Count == 0)
            resultCard = simpleCards.MinBy(x => x.Type) ?? handCards.MinBy(x => x.Type);
        resultCard ??= deckSize switch
        {
            > 11 => resultCard ??= simpleCards
                    .Where(x => desktopCards.Any(y => x.Type == y.Type))
                    .MinBy(x => x.Type),
            > 4 => resultCard ??= simpleCards
                    .Where(x => desktopCards.Any(y => x.Type == y.Type))
                    .MinBy(x => x.Type)
                        ?? handCards
                            .Where(x => desktopCards.Any(y => x.Type == y.Type))
                            .Where(x => x.Type <= MAX_POSSIBLE_TYPE_TRUMP_CARD)
                            .MinBy(x => x.Type),
            _ => resultCard ??= simpleCards
                    .Where(x => desktopCards.Any(y => x.Type == y.Type))
                    .MinBy(x => x.Type)
                        ?? handCards
                            .Where(x => desktopCards.Any(y => x.Type == y.Type))
                            .MinBy(x => x.Type)
        };
        return resultCard is not null;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Когда список карт находящихся в руке или столе равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Когда <paramref name="deckSize"/> меньше нуля или любой список карт пустой.
    /// </exception>
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


        var needClosed = desktopCards.First();
                        //.Where(x => x.Owner!.Id != ownerId)
                        //.First(x => !x.IsCloseOnDesktop);
        closedCard = needClosed;
        resultCard = handCards
                    .Where(x => !x.IsTrump)
                    .Where(x => needClosed.Type < x.Type)
                    .Where(x => needClosed.Suit == x.Suit)
                    .MinBy(x => x.Type);
        resultCard ??= deckSize switch
        {
            > 11 => handCards
                    .Where(x => x.IsTrump)
                    .Where(needClosed => !needClosed.IsTrump)
                    .Where(x => x.Type <= MAX_POSSIBLE_TYPE_TRUMP_CARD)
                    .MinBy(x => x.Type),
            > 4 => resultCard ??= handCards
                    .Where(x => x.IsTrump)
                    .Where(x => !needClosed.IsTrump)
                    .MinBy(x => x.Type)
                        ?? handCards
                            .Where(x => x.IsTrump)
                            .Where(x => needClosed.IsTrump)
                            .Where(x => x.Type > needClosed.Type)
                            .Where(x => needClosed.Type <= MAX_POSSIBLE_TYPE_TRUMP_CARD)
                            .MinBy(x => x.Type),
            _ => resultCard ??= handCards
                    .Where(x => x.IsTrump)
                    .Where(x => !needClosed.IsTrump)
                    .MinBy(x => x.Type)
                        ?? handCards
                            .Where(x => x.IsTrump)
                            .Where(x => needClosed.IsTrump)
                            .Where(x => x.Type > needClosed.Type)
                            .MinBy(x => x.Type)
        };
        return resultCard is not null;
    }
}