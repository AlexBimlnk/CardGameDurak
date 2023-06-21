namespace Logic;

/// <summary>
/// Стратегия, используемая ботом на средней сложности.
/// </summary>
public sealed class NormalStrategy : IBotStrategy
{
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

        if (deckSize < 0)
            throw new ArgumentException("Кол-во карт в колоде не может быть отрицательным");

        resultCard = null!;
        var simpleCards = handCards.Where(x => !x.IsTrump);

        if (desktopCards.Count == 0)
            resultCard = simpleCards.MinBy(x => x.Type) ?? handCards.MinBy(x => x.Type);

        resultCard ??= simpleCards.Where(x => desktopCards.Any(y => x.Type == y.Type))
                                  .MinBy(x => x.Type);

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
              //                       .First(x => !x.IsCloseOnDesktop);
        closedCard = needClosed;
        if (!needClosed.IsTrump)
        {
            resultCard = handCards.Where(x => needClosed.Type < x.Type)
                                  .Where(x => needClosed.Suit == x.Suit)
                                  .MinBy(x => x.Type);
            resultCard ??= handCards.Where(x => x.IsTrump).MinBy(x => x.Type);
        }
        else resultCard = null!;

        return resultCard is not null;
    }
}