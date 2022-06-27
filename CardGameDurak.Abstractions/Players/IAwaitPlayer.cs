namespace CardGameDurak.Abstractions.Players;

/// <summary xml:lang = "ru">
/// Контракт, описывающий игрока, ожидающего игру.
/// </summary>
public interface IAwaitPlayer
{
    /// <summary xml:lang = "ru">
    /// Количество игроков, ожидаемых в игре.
    /// </summary>
    public int AwaitPlayersCount { get; }

    /// <summary xml:lang = "ru">
    /// Игрок.
    /// </summary>
    public IPlayer Player { get; }
}
