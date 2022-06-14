namespace CardGameDurak.Abstractions.Messages;

/// <summary xml:lang = "ru">
/// Контракт, описывающий сообщение на присоединение к игре.
/// </summary>
public interface IJoinMessage
{
    /// <summary xml:lang = "ru">
    /// Ожидаемое число игроков в игре.
    /// </summary>
    public int AwaitPlayersCount { get; }

    /// <summary xml:lang = "ru">
    /// Отправитель.
    /// </summary>
    public IPlayer Sender { get; }
}
