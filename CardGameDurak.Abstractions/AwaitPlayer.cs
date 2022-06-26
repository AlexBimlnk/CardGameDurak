using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Abstractions;

/// <summary xml:lang = "ru">
/// Игрок ожидающий игру.
/// </summary>
public class AwaitPlayer : IAwaitPlayer
{
    private const int MIN_AWAIT_PLAYERS_COUNT = 2;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="AwaitPlayer"/>.
    /// </summary>
    /// <param name="awaitPlayersCount" xml:lang = "ru">
    /// Количество ожидаемых в игре игроков.
    /// </param>
    /// <param name="player" xml:lang = "ru">
    /// Игрок.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Если количество ожидаемых игроков выходит за пределы допустимого.
    /// </exception>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда игрок равен <see langword="null"/>.
    /// </exception>
    public AwaitPlayer(int awaitPlayersCount, IPlayer player)
    {
        if (awaitPlayersCount < MIN_AWAIT_PLAYERS_COUNT)
            throw new ArgumentOutOfRangeException(nameof(awaitPlayersCount));

        Player = player ?? throw new ArgumentNullException(nameof(player));
        AwaitPlayersCount = awaitPlayersCount;
    }

    /// <inheritdoc/>
    public int AwaitPlayersCount { get; }

    /// <inheritdoc/>
    public IPlayer Player { get; }
}
