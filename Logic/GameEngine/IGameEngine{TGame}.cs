using Logic.Events.GameEvents;
using Logic.GameTypes;

namespace Logic.GameEngine;

/// <summary>
/// Описывает движок карточной игры.
/// </summary>
/// <typeparam name="TGame">
/// Тип карточной игры.
/// </typeparam>
public interface IGameEngine<TGame>
    where TGame : IGameType
{
    /// <summary>
    /// Обрабатывает игровое событие.
    /// </summary>
    /// <param name="gameEvent">
    /// Игровое событие.
    /// </param>
    /// <param name="token">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// <see cref="Task"/>.
    /// </returns>
    public Task ProcessEvent(IGameEvent<TGame> gameEvent, CancellationToken token);
}
