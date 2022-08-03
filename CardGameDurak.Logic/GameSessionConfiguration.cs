namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Конфигурация игровой сессии.
/// </summary>
public class GameSessionConfiguration
{
    private const int DEFAULT_DECK_SIZE = 36;

    /// <summary xml:lang = "ru">
    /// Минимальное кол-во игроков в игровой сессии.
    /// </summary>
    public int MinPlayersCount { get; init; }

    /// <summary xml:lang = "ru">
    /// Максимальное кол-во игроков в игровой сессии.
    /// </summary>
    public int MaxPlayersCount { get; init; }    

    /// <summary xml:lang = "ru">
    /// Размер колоды.
    /// </summary>
    public int DeckSize { get; init; } = DEFAULT_DECK_SIZE;
}
