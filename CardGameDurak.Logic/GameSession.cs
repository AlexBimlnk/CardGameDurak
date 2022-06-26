using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Игровая сессия.
/// </summary>
public class GameSession : IGameSession
{
    private const int DEFAULT_SIZE_DECK = 36;
    private const int MAX_SIZE_DECK = 52;
    private const int MIN_AMOUNT_PLAYERS = 2;
    private const int MAX_AMOUNT_PLAYERS = 6;
    private const int MAX_AMOUNT_CARD_ON_DESKTOP = 12;
    private const int MAX_AMOUNT_CARD_TO_GIVE = 6;
    private const int MIN_AMOUNT_CARD_TO_GIVE = 1;

    private readonly List<ICard> _deck = new(DEFAULT_SIZE_DECK);
    private readonly List<IPlayer> _players = new(MIN_AMOUNT_PLAYERS);
    private readonly List<ICard> _desktop = new(MAX_AMOUNT_CARD_ON_DESKTOP);

    private readonly Random _random = new();

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="GameSession"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор игровой сессии.
    /// </param>
    /// <param name="deck" xml:lang = "ru">
    /// Коллекция карт, представляющая колоду.
    /// </param>
    /// <param name="players" xml:lang = "ru">
    /// Коллекция игроков, которые будут присутствовать в игре.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException" xml:lnag = "ru">
    /// Когда кол-во игроков меньше двух.
    /// </exception>
    public GameSession(
        GameSessionId id,
        IEnumerable<ICard> deck, 
        IEnumerable<IPlayer> players)
    {
        Id = id;
        _deck.AddRange(deck ?? throw new ArgumentNullException(nameof(deck)));
        AddPlayers(players ?? throw new ArgumentNullException(nameof(players)));
    }

    /// <inheritdoc/>
    public GameSessionId Id { get; }

    /// <inheritdoc/>
    public int Version { get; }

    /// <inheritdoc/>
    public IReadOnlyCollection<IPlayer> Players => _players;

    /// <inheritdoc/>
    public IReadOnlyCollection<ICard> Desktop => _desktop;

    private void AddPlayers(IEnumerable<IPlayer> players)
    {
        _players.AddRange(players);

        if (_players.Count < MIN_AMOUNT_PLAYERS)
            throw new ArgumentException($"Count of players can't be less {MIN_AMOUNT_PLAYERS}", nameof(players));

        foreach (var i in Enumerable.Range(0, _players.Count))
            _players[i].Id = i + 1;
    }

    /// <summary xml:lang = "ru">
    /// Выдает карты по требованию.
    /// </summary>
    /// <param name="count" xml:lang = "ru">
    /// Количество карт, которое нужно выдать.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Перечисляемую последовательность типа <see cref="IEnumerable{T}"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Если кол-во карт меньше единицы или больше максимально допустимого значения.
    /// </exception>
    public IEnumerable<ICard> GiveCards(int count)
    {
        if (count < MIN_AMOUNT_CARD_TO_GIVE || count > MAX_AMOUNT_CARD_TO_GIVE)
            throw new ArgumentOutOfRangeException(nameof(count));

        IEnumerable<ICard> _()
        {
            while (_deck.Count > 0 && count > 0)
            {
                ICard card = _deck[_random.Next(0, _deck.Count)];
                _deck.Remove(card);
                count--;
                yield return card;
            }
        };

        return _();
    }

    /// <summary xml:lang = "ru">
    /// Устанавливает козырную масть карт
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Возвращает масть типа Suit что является перечислением <see cref="Suit"/>.
    /// </returns>
    public Suit SetTrumpCard() => _deck[_random.Next(0, _deck.Count)].Suit;
}
