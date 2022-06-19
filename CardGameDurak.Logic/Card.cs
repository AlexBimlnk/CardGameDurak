using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Игральная карта.
/// </summary>
public class Card : ICard, IComparable<Card>
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр класса <see cref="Card"/>.
    /// </summary>
    /// <param name="suit" xml:lang = "ru">
    /// Масть карты.
    /// </param>
    /// <param name="rank" xml:lang = "ru">
    /// Сила карты.
    /// </param>
    public Card(Suit suit, int rank)
    {
        Suit = suit;
        Rank = rank;
    }

    /// <inheritdoc/>
    public int Rank { get; }

    /// <inheritdoc/>
    public Suit Suit { get; }

    /// <inheritdoc/>
    public IPlayer? Owner { get; set; }
    /// <inheritdoc/>
    public bool IsTrump { get; set; }
    /// <inheritdoc/>
    public bool IsCloseOnDesktop { get; set; }
    /// <inheritdoc/>
    public int CompareTo(Card? card)
    {
        if (card is null)
            throw new ArgumentNullException("Карты нет!");
        if (!card.IsTrump) return Rank - card.Rank;
        else return Rank - card.Rank + 100;
    }
}
