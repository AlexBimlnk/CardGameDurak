namespace Logic.Events;

/// <summary>
/// Описывает игровое событие.
/// </summary>
public interface IGameEvent
{
    public GameId GameId { get; }

    public GameEventType Type { get; }
}
