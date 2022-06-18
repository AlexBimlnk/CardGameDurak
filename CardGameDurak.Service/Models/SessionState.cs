using CardGameDurak.Abstractions;
using CardGameDurak.Logic;

namespace CardGameDurak.Service.Models;

internal class SessionState : ISessionState<IEnumerable<ICard>>
{
    public SessionState(GameSession session, Card[] cards)
    {
        Session = session ?? throw new ArgumentNullException(nameof(session));
        LinkedValue = cards ?? throw new ArgumentNullException(nameof(cards));
    }

    public IGameSession Session { get; }

    public IEnumerable<ICard> LinkedValue { get; }
}
