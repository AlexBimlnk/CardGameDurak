using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Базовый класс для всех игроков.
/// </summary>
public abstract class PlayerBase : IPlayer
{
    private const int DEFAULT_COUNT_CARDS_IN_HAND = 6;

    /// <summary xml:lang = "ru">
    /// Карты, находящиеся в руке у игрока.
    /// </summary>
    protected readonly List<ICard> _cards = new(DEFAULT_COUNT_CARDS_IN_HAND);

    public PlayerBase(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Имя игрока не может быть пустым.");

        Name = name;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public int CountCards => _cards.Count;

    /// <inheritdoc/>
    public int? Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Принимает карты, которые ему выдают.
    /// </summary>
    /// <param name="cards" xml:lang = "ru">
    /// Список карт, который нужно добавить в руку.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда массив выданных карт равен <see langword="null"/>.  
    /// </exception>
    public void ReceiveCards(IEnumerable<ICard> cards) =>
        _cards.AddRange(cards ?? throw new ArgumentNullException(nameof(cards)));
}
