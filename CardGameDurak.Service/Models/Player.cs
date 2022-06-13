using CardGameDurak.Abstractions;

namespace CardGameDurak.Service.Models;

/// <summary xml:lang = "ru">
/// Игрок.
/// </summary>
public class Player : IPlayer
{
    private const int MIN_AWAIT_PLAYERS_COUNT = 2;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа .
    /// </summary>
    /// <param name="id">
    /// Идентификатор игрока.
    /// </param>
    /// <param name="name" xml:lang = "ru">
    /// Имя игрока.
    /// </param>
    /// <param name="awaitPlayersCount" xml:lang = "ru">
    /// Количество игроков, которых ожидает в игре.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Если количество ожидаемых игроков выходит за пределы допустимого.
    /// </exception>
    public Player(long id, string name, int awaitPlayersCount)
    {
        if (awaitPlayersCount < MIN_AWAIT_PLAYERS_COUNT)
            throw new ArgumentOutOfRangeException(nameof(awaitPlayersCount));

        Name = string.IsNullOrWhiteSpace(name) switch
        {
            true => throw new ArgumentNullException(nameof(name)),
            false => name
        };
        Id = id;
        AwaitPlayersCount = awaitPlayersCount;
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор игрока.
    /// </summary>
    public long Id { get; }

    /// <summary xml:lang = "ru">
    /// Количество игроков, ожидаемых в игре.
    /// </summary>
    public int AwaitPlayersCount { get; }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public int CountCards => throw new NotImplementedException();

    /// <inheritdoc/>
    public void ReceiveCards(params ICard[] cards) => throw new NotImplementedException();
}
