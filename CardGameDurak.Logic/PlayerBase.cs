using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Базовый класс для всех игроков.
/// </summary>
internal abstract class PlayerBase : IPlayer
{
    protected readonly List<ICard> _cards = new List<ICard>(6);

    public PlayerBase(string name) => Name = name;

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public int CountCards => _cards.Count;

    /// <inheritdoc/>
    public void ReceiveCards(params ICard[] cards) =>
        _cards.AddRange(cards ?? throw new ArgumentNullException(nameof(cards)));
}
