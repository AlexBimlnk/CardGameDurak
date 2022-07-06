using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.GameSession;
using CardGameDurak.Abstractions.Players;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Игральная карта.
/// </summary>
public class Card : IEquatable<ICard>
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр класса <see cref="Card"/>.
    /// </summary>
    /// <param name="suit" xml:lang = "ru">
    /// Масть карты.
    /// </param>
    /// <param name="rank" xml:lang = "ru">
    /// Сила карты.
    /// </param>
    public Card(Suit suit, int rank)
    {
        Suit = suit;
        Rank = rank;
    }

    /// <inheritdoc/>
    public int Rank { get; }

    /// <inheritdoc/>
    public Suit Suit { get; }

    /// <inheritdoc/>
    public IPlayer? Owner { get; set; }

    /// <inheritdoc/>
    public bool IsTrump { get; set; }

    /// <inheritdoc/>
    public bool IsCloseOnDesktop { get; set; }

    /// <inheritdoc/>
    public bool Equals(ICard? curCard)
    {
        if (curCard is null) 
            return false;
        if (Rank != curCard.Rank || Suit != curCard.Suit || Owner != curCard.Owner)
            return false;
        return true;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is ICard card)
            return Equals(card);
        else
            return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Rank, Suit);
}
