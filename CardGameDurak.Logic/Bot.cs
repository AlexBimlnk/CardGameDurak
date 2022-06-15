    using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;
public class Bot : PlayerBase, IBot
{
    public Bot(string name) : base(name) { }

    public ICard Attak(IReadOnlyCollection<ICard> desktopCards) => throw new NotImplementedException();
    public ICard Defence(IReadOnlyCollection<ICard> desktopCards) => throw new NotImplementedException();
    public ICard? Attaсk(IReadOnlyCollection<ICard> desktopCards)
    {
        if (desktopCards.Count() == 0)
            return _cards.Min();
        return _cards.Where(owner => owner.Owner == this)
                       .Where(card => desktopCards.Any(x => x.Rank == card.Rank))
                       .Min();
    }
    public ICard? Defence(IReadOnlyCollection<ICard> desktopCards, out ICard outCard)
    {
        if (desktopCards.Count() == 0)
        {
            outCard = null;
            return null;
        }
        ICard myCard = _cards.Where(owner => owner.Owner == this)
                             .Where(card => desktopCards.Where(owner => owner.Owner != this)
                             .Select(tempCard => new { Rank = card.Rank, Suit = card.Suit, IsTrump = card.IsTrump, tempCard})
                             .Any(x => x.Rank < card.Rank && x.Suit == card.Suit
                             || !x.IsTrump && card.IsTrump))
                             .Min();
        outCard = desktopCards.Where(card => desktopCards.Where(owner => owner.Owner != this)
                                .Any(card => myCard.Rank > card.Rank || card.IsTrump)).Min(); // это по приколу написано
        return myCard;
    }
}
