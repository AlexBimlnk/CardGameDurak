using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

public class Message
{
    public Message(Player sender, PlayerEvent playerEvent, ICard card)
    {
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
        PlayerEvent = playerEvent switch
        {
            var pEnvet => 
            pEnvet == PlayerEvent.DropOnDesktop 
                ? card is null 
                    ? throw new ArgumentNullException(nameof(card))
                    : pEnvet
                : pEnvet,
        };
        Card = card;
    }

    public Player Sender { get; }

    public ICard? Card { get; }

    public PlayerEvent PlayerEvent { get; }
}
