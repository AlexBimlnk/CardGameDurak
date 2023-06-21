namespace Logic.Events;

/// <summary>
/// Событие создания игры.
/// </summary>
public sealed class CreateGame<TGame> : GameEventBase, IGameEvent<TGame>
    where TGame : IGameType
{
    /// <summary>
    /// Создает новый объект типа <see cref="CreateGame{TGame}"/>.
    /// </summary>
    /// <param name="id">
    /// 
    /// </param>
    public CreateGame(GameId id)
        : base(id, GameEventType.CreateGame)
    {
    }
}
