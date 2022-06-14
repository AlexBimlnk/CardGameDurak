using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Игральная карта.
/// </summary>
public class Card : ICard
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
    public bool IsTrump { get; }
    public bool IsCloseOnDesktop { get; set; }
}
