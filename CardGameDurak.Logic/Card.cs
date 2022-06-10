using CardGameDurak.Logic.Abstractions;
using CardGameDurak.Logic.Enums;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Игровальная карта.
/// </summary>
public class Card
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

    /// <summary xml:lang = "ru">
    /// Сила карты.
    /// </summary>
    public int Rank { get; }

    /// <summary xml:lang = "ru">
    /// Масть карты.
    /// </summary>
    public Suit Suit { get; }

    /// <summary xml:lang = "ru">
    /// Владелец катры.
    /// </summary>
    public IPlayer? Owner { get; set; }
}
