namespace Logic;

/// <summary>
/// Базовый класс для всех игроков.
/// </summary>
public abstract class PlayerBase : IPlayer
{
    private const int DEFAULT_COUNT_CARDS_IN_HAND = 6;

    /// <summary>
    /// Карты, находящиеся в руке у игрока.
    /// </summary>
    protected readonly List<ICard> _cards = new(DEFAULT_COUNT_CARDS_IN_HAND);

    /// <summary>
    /// Создает новый экземпляр типа <see cref="PlayerBase"/>.
    /// </summary>
    /// <param name="id">
    /// Идентификатор игрока.
    /// </param>
    /// <param name="name">
    /// Имя игрока.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="name"/> оказался <see langword="null"/>.
    /// </exception>
    protected PlayerBase(PlayerId id, Name name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <inheritdoc/>
    public PlayerId Id { get; }

    /// <inheritdoc/>
    public Name Name { get; }
}
