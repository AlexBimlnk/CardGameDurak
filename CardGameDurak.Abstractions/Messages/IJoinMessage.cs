namespace CardGameDurak.Abstractions.Messages;

/// <summary xml:lang = "ru">
/// Контракт, описывающий сообщение на присоединение к игре.
/// </summary>
/// <typeparam name="TSender" xml:lang = "ru">
/// Тип игрока.
/// </typeparam>
public interface IJoinMessage<TSender>
{
    /// <summary xml:lang = "ru">
    /// Ожидаемое число игроков в игре.
    /// </summary>
    public int AwaitPlayersCount { get; }

    /// <summary xml:lang = "ru">
    /// Отправитель типа <typeparamref name="TSender"/>.
    /// </summary>
    public TSender Sender { get; }
}
