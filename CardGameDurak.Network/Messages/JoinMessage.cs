using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Network.Messages;

/// <summary xml:lang = "ru">
/// Сообщение о присоединении.
/// </summary>
/// <typeparam name="TPlayer" xml:lang = "ru">
/// Тип игрока, отправившего запрос на присоединение.
/// </typeparam>
public sealed class JoinMessage<TPlayer> : IJoinMessage<TPlayer>
    where TPlayer : IPlayer
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
    public JoinMessage(int awaitPlayersCount, TPlayer sender)
    {
        AwaitPlayersCount = awaitPlayersCount;
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    /// <inheritdoc/>
    public int AwaitPlayersCount { get; set; }

    /// <inheritdoc/>
    public TPlayer Sender { get; set; }
}
