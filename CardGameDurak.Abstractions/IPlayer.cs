namespace CardGameDurak.Abstractions;

/// <summary xml:lang = "ru">
/// Контракт, описывающий поведение всех игроков в игре.
/// </summary>
public interface IPlayer
{
    /// <summary xml:lang = "ru">
    /// Идентификатор игрока в игре.
    /// </summary>
    public int? Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Имя игрока.
    /// </summary>
    public string Name { get; }

    /// <summary xml:lang = "ru">
    /// Количество карт на руке.
    /// </summary>
    public int CountCards { get; }
}
