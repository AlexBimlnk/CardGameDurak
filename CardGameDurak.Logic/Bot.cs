using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;
/// <summary xml:lang = "ru">
/// Класс бота.
/// </summary>
public class Bot : PlayerBase, IBot
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр класса <see cref="Bot"/>.
    /// </summary>
    /// <param name="name" xml:lang = "ru">
    /// Имя бота.
    /// </param>
    public Bot(string name) : base(name) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="desktopCards"></param>
    /// <returns></returns>
    public ICard Attaсk(IReadOnlyCollection<ICard> desktopCards)
    {
        if (desktopCards.Count == 0)
            return _cards.Min();

        return _cards.Where(owner => owner.Owner.Id == Id)
                     .Where(card => desktopCards.Any(x => x.Rank == card.Rank))
                     .MinBy(card => card.Rank);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="desktopCards"></param>
    /// <param name="outCard"></param>
    /// <returns></returns>
    public ICard Defence(IReadOnlyCollection<ICard> desktopCards, out ICard outCard)
    {
        if (desktopCards is null || desktopCards.Count == 0)
            throw new ArgumentNullException(nameof(desktopCards));


        ICard myCard = _cards.Where(card =>
        {
            return desktopCards.Where(owner => owner.Owner.Id != Id)
                               .Select(tempCard => new { Rank = card.Rank, Suit = card.Suit, IsTrump = card.IsTrump, tempCard })
                               .Any(x => x.Rank < card.Rank && x.Suit == card.Suit || !x.IsTrump && card.IsTrump);
        }).Min();

        outCard = desktopCards.Where(card => desktopCards.Where(owner => owner.Owner.Id != Id)
                              .Any(card => myCard.Rank > card.Rank || card.IsTrump))
                              .Min(); // это по приколу написано
        return myCard;
    }
}
