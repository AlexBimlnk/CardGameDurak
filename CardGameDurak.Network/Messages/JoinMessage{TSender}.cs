using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Network.Messages;

/// <summary xml:lang = "ru">
/// Сообщение о присоединении.
/// </summary>
/// <typeparam name="TSender" xml:lang = "ru">
/// Тип игрока, отправившего запрос на присоединение.
/// </typeparam>
public sealed class JoinMessage<TSender> : IMessage<int, TSender>
    where TSender : IAwaitPlayer, ISender
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="JoinMessage{TPlayer}"/>.
    /// </summary>
    /// <param name="awaitPlayersCount" xml:lang = "ru">
    /// Ожидаемое кол-во игроков в игре.
    /// </param>
    /// <param name="sender" xml:lang = "ru">
    /// Отправитель.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда отправитель равен <see langword="null"/>.
    /// </exception>
    public JoinMessage(TSender sender) => Sender = sender ?? throw new ArgumentNullException(nameof(sender));

    /// <summary xml:lang = "ru">
    /// Отправитель сообщения типа <typeparamref name="TSender"/>.
    /// </summary>
    public TSender Sender { get; }

    /// <inheritdoc/>
    public int Value => Sender.AwaitPlayersCount;
}
