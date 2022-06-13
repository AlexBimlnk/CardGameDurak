using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Service.Models.Messages;

public class EventMessage : IEventMessage
{
    public EventMessage(Player sender, GameEvent playerEvent, ICard card)
    {
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
    public IPlayer Sender { get; }

    /// <inheritdoc/>
    public ICard? Card { get; }

    /// <inheritdoc/>
    public GameEvent PlayerEvent { get; }
}
