using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;

namespace CardGameDurak.Service.Models;

internal class GameEvent : IGameEvent
{
    public GameEvent(PlayerEvent playerEvent, ICard card = default!)
    {
        Value = card;
        Event = playerEvent switch
        {
            PlayerEvent.DropOnDesktop => card is not null
                ? playerEvent
                : throw new ArgumentNullException(nameof(card), $"Card can't be null when events is {PlayerEvent.DropOnDesktop}"),
            PlayerEvent.Take => playerEvent,
            PlayerEvent.MoveTurn => playerEvent,
            _ => throw new ArgumentOutOfRangeException(nameof(playerEvent))
        };
    }

    public PlayerEvent Event { get; }

    public ICard? Value { get; }
}
