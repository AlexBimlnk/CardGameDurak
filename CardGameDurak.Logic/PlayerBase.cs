using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.Players;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Базовый класс для всех игроков.
/// </summary>
internal abstract class PlayerBase : IEquatable<IPlayer>
{
    protected readonly List<ICard> _cards = new List<ICard>(6);

    public PlayerBase(string name) => Name = name;

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public int CountCards => _cards.Count;

    /// <inheritdoc/>
    public int? Id { get; set; }

    /// <inheritdoc/>
    public bool Equals(IPlayer? curPlayer)
    {
        if (curPlayer is null)
            return false;
        if (Name != curPlayer.Name || Id != curPlayer.Id || CountCards != curPlayer.CountCards)
            return false;
        return true;
    }
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is IPlayer player)
            return Equals(player);
        else
            return false;
    }
    public override int GetHashCode() => HashCode.Combine(Id, Name);

    /// <summary xml:lang = "ru">
    /// Принимает карты, которые ему выдают.
    /// </summary>
    /// <param name="cards" xml:lang = "ru">
    /// Список карт, который нужно добавить в руку.
    /// </param>
    public void ReceiveCards(params ICard[] cards) =>
        _cards.AddRange(cards ?? throw new ArgumentNullException(nameof(cards)));
}
