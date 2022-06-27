using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Players;

namespace CardGameDurak.Logic;

internal class Bot : PlayerBase, IBot
{
    public Bot(string name) : base(name) { }

    public ICard Attak(IReadOnlyCollection<ICard> desktopCards) => throw new NotImplementedException();
    public ICard Defence(IReadOnlyCollection<ICard> desktopCards) => throw new NotImplementedException();
}
