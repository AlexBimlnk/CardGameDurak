using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Service.Models.Messages;

internal class EventMessage : IEventMessage
{
    public EventMessage(AwaitPlayer sender, GameEvent playerEvent, ICard card)
    {
        Sender = sender?.Player ?? throw new ArgumentNullException(nameof(sender));
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
    public IPlayer Sender { get; }

    /// <inheritdoc/>
    public ICard? Card { get; }

    /// <inheritdoc/>
    public GameEvent PlayerEvent { get; }
}
