using CardGameDurak.Abstractions.Enums;

namespace CardGameDurak.Abstractions;

/// <summary xml:lang = "ru">
/// Контракт, описывающий игральную карт.
/// </summary>
public interface ICard
{
    /// <summary xml:lang = "ru">
    /// Масть карты.
    /// </summary>
    public Suit Suit { get; }

    /// <summary xml:lang = "ru">
    /// Сила карты.
    /// </summary>
    public int Rank { get; }

    /// <summary xml:lang = "ru">
    /// Владелец катры.
    /// </summary>
    public IPlayer? Owner { get; set; }
}
