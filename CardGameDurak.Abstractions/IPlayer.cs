namespace CardGameDurak.Abstractions;

/// <summary xml:lang = "ru">
/// Контракт, описывающий поведение всех игроков в игре.
/// </summary>
public interface IPlayer
{
    /// <summary xml:lang = "ru">
    /// Имя игрока.
    /// </summary>
    public string Name { get; }
    /// <summary xml:lang = "ru">
    /// Количество карт на руке.
    /// </summary>
    public int CountCards { get; }
    /// <summary xml:lang = "ru">
    /// Принимает карты, которые ему выдают.
    /// </summary>
    /// <param name="cards" xml:lang = "ru">
    /// Список карт, который нужно добавить в руку.
    /// </param>
    public void ReceiveCards(params ICard[] cards);
}
