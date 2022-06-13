using CardGameDurak.Abstractions.Enums;

namespace CardGameDurak.Abstractions.Messages;

/// <summary xml:lang = "ru">
/// Контракт, описывающий сообщение содержащее игровое событие.
/// </summary>
public interface IEventMessage
{
    /// <summary xml:lang = "ru">
    /// Игрок, отправивший сообщение.
    /// </summary>
    public IPlayer Sender { get; }

    /// <summary xml:lang = "ru">
    /// Тип игрового события.
    /// </summary>
    public GameEvent PlayerEvent { get; }

    /// <summary xml:lang = "ru">
    /// Закрепленная за событием карта.
    /// </summary>
    public ICard? Card { get; }
}
