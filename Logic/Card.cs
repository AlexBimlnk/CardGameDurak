namespace Logic;

/// <summary>
/// Игральная карта.
/// </summary>
public class Card : ICard
{
    /// <summary>
    /// Создает новый экземпляр класса <see cref="Card"/>.
    /// </summary>
    /// <param name="suit">
    /// Масть карты.
    /// </param>
    /// <param name="type">
    /// Сила карты.
    /// </param>
    public Card(Suit suit, CardType type)
    {
        Suit = suit;
        Type = type;
    }

    /// <inheritdoc/>
    public CardType Type { get; }

    /// <inheritdoc/>
    public Suit Suit { get; }

    /// <inheritdoc/>
    public bool IsTrump => throw new NotSupportedException();
}
