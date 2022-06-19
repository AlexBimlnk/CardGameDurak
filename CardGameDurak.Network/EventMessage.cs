using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Network;

internal class EventMessage : IEventMessage
{
    public EventMessage(
        GameSessionId sessionId,
        IPlayer sender, 
        GameEvent playerEvent, 
        ICard card)
    {
        SessionId = sessionId;
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
        PlayerEvent = playerEvent switch
        {
            var pEnvet =>
            pEnvet == GameEvent.DropOnDesktop
                ? card is null
                    ? throw new ArgumentNullException(nameof(card))
                    : pEnvet
                : pEnvet,
        };
        Card = card;
    }

    /// <inheritdoc/>
    public GameSessionId SessionId { get; }

    /// <inheritdoc/>
    public IPlayer Sender { get; }

    /// <inheritdoc/>
    public ICard? Card { get; }

    /// <inheritdoc/>
    public GameEvent PlayerEvent { get; }
}
