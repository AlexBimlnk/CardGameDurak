using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Service.Models;

/// <summary xml:lang = "ru">
/// Игрок ожидающий игру.
/// </summary>
public class AwaitPlayer : IAwaitPlayer
{
    private const int MIN_AWAIT_PLAYERS_COUNT = 2;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="AwaitPlayer"/>.
    /// </summary>
    /// <param name="message" xml:lang = "ru">
    /// Сообщение о присоединении к игре.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Если количество ожидаемых игроков выходит за пределы допустимого.
    /// </exception>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда сообщение или игрок <see langword="null"/>.
    /// </exception>
    public AwaitPlayer(IJoinMessage message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (message.AwaitPlayersCount < MIN_AWAIT_PLAYERS_COUNT)
            throw new ArgumentOutOfRangeException(nameof(message.AwaitPlayersCount));

        Player = message.Sender ?? throw new ArgumentNullException(nameof(message.Sender));
        AwaitPlayersCount = message.AwaitPlayersCount;
    }

    /// <inheritdoc/>
    public int AwaitPlayersCount { get; }

    /// <summary xml:lang = "ru">
    /// TCS на присоединение к игре.
    /// </summary>
    public TaskCompletionSource<long> JoinTCS { get; } = new();

    /// <inheritdoc/>
    public IPlayer Player { get; }
}
