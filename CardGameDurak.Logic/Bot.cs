using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;

internal class Bot : PlayerBase, IBot
{
    public Bot(string name) : base(name) { }

    public ICard? Attaсk(IReadOnlyCollection<ICard> desktopCards)
    {
        if (desktopCards.Count() == 0)
            return _cards.Min();
        return _cards.Where(owner => owner.Owner == this)
                       .Where(card => desktopCards.Any(x => x.Rank == card.Rank))
                       .Min();
    }
    public ICard? Defence(IReadOnlyCollection<ICard> desktopCards, out ICard enemyCard)
    {
        if (desktopCards.Count() == 0)
        {
            enemyCard = null;
            return null;
        }
        ICard myCard = _cards.Where(owner => owner.Owner == this)
                             .Where(card => desktopCards.Where(owner => owner.Owner != this)
                             .Any(x => x.Rank < card.Rank && x.Suit == card.Suit
                             || !x.IsTrump && card.IsTrump))
                             .Min();
        enemyCard = desktopCards.Where(card => desktopCards.Where(owner => owner.Owner != this)
                                .Any(card => myCard.Rank > card.Rank || card.IsTrump)).Min();
        return _cards.Where(owner => owner.Owner == this)
                             .Where(card => desktopCards.Where(owner => owner.Owner != this)
                             .Any(x => x.Rank < card.Rank && x.Suit == card.Suit 
                             || !x.IsTrump && card.IsTrump))
                             .Min();
    }
}
