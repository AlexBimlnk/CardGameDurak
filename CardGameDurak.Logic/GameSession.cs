﻿using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Игровая сессия.
/// </summary>
public class GameSession
{
    private const int DEFAULT_SIZE_DECK = 36;
    private const int DEFAULT_AMOUNT_PLAYERS = 2;
    private const int MAX_AMOUNT_CARD_ON_DESKTOP = 12;
    private const int MAX_AMOUNT_CARD_TO_GIVE= 6;

    private readonly List<ICard> _deck = new(DEFAULT_SIZE_DECK);
    private readonly List<IPlayer> _players = new(DEFAULT_AMOUNT_PLAYERS);
    private readonly List<ICard> _desktop = new(MAX_AMOUNT_CARD_ON_DESKTOP);

    private readonly GameSessionId _id;
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
    public GameSession(
        GameSessionId id,
        IEnumerable<ICard> deck, 
        IEnumerable<IPlayer> players)
    {
        _id = id ?? throw new ArgumentNullException(nameof(id));
        _deck.AddRange(deck ?? throw new ArgumentNullException(nameof(deck)));
        _players.AddRange(
            (players?.Count() ?? throw new ArgumentNullException(nameof(players))) < 2
            ? throw new ArgumentException("Count of players can't be less 2.", nameof(players))
            : players);
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор игровой сессии.
    /// </summary>
    public long Id => _id.Value;

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
        if (count < 1 || count > MAX_AMOUNT_CARD_TO_GIVE)
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
}