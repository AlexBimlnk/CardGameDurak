namespace CardGameDurak.Abstractions;

/// <summary xml:lang = "ru">
/// Контракт, описывающий игровые сессии.
/// </summary>
public interface IGameSession
{
    /// <summary xml:lang = "ru">
    /// Идентификатор игровой сессии.
    /// </summary>
    public GameSessionId Id { get; }

    /// <summary xml:lang = "ru">
    /// Состояние игровой сессии.
    /// </summary>
    public int Version { get; }

    /// <summary xml:lang = "ru">
    /// Список игроков в игре.
    /// </summary>
    public IReadOnlyCollection<IPlayer> Players { get; }

    /// <summary xml:lang = "ru">
    /// Список карт, находящихся на столе.
    /// </summary>
    public IReadOnlyCollection<ICard> Desktop { get; }
}
