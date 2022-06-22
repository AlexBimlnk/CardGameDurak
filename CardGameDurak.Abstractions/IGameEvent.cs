using CardGameDurak.Abstractions.Enums;

namespace CardGameDurak.Abstractions;

/// <summary xml:lang = "ru">
/// Контракт, описывающий игровое событие.
/// </summary>
public interface IGameEvent
{
    /// <summary xml:lang = "ru">
    /// Событие, которое совершил игрок.
    /// </summary>
    public PlayerEvent PlayerEvent { get; }

    /// <summary xml:lang = "ru">
    /// Значение, сопровождающее игровое событие.
    /// </summary>
    public ICard? Card { get; }
}
