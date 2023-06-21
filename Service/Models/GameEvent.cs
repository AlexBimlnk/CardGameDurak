using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Controlling;
using CardGameDurak.Abstractions.Enums;

using Logic;

namespace CardGameDurak.Service.Models;

internal class GameEvent : IGameEvent
{
    public GameEvent(PlayerEvent playerEvent, Card card = default!)
    {
        Card = card;
        PlayerEvent = playerEvent switch
        {
            PlayerEvent.DropOnDesktop => card is not null
                ? playerEvent
                : throw new ArgumentNullException(nameof(card), $"Card can't be null when events is {PlayerEvent.DropOnDesktop}"),
            PlayerEvent.Take => playerEvent,
            PlayerEvent.MoveTurn => playerEvent,
            _ => throw new ArgumentOutOfRangeException(nameof(playerEvent))
        };
    }

    public PlayerEvent PlayerEvent { get; }

    public ICard? Card { get; }
}
