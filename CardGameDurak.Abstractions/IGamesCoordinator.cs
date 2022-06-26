using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Abstractions;

/// <summary xml:lang = "ru">
/// Контракт, описывающий игровой координатор, 
/// который хранит и обновляет игровые сессии.
/// </summary>
/// <typeparam name="TAwaitPlayer" xml:lang = "ru">
/// Объект типа <see cref="IAwaitPlayer"/>, который будет выступать
/// в качестве ожидаемого игрока.
/// </typeparam>
public interface IGameCoordinator<TAwaitPlayer> where TAwaitPlayer : IAwaitPlayer
{
    public string Name { get; }

    /// <summary xml:lang = "ru">
    /// Добавляет в очередь игрока на ожидание игры.
    /// </summary>
    /// <param name="player" xml:lang = "ru">
    /// Игрок типа <typeparamref name="TAwaitPlayer"/>, ожидающий игру.
    /// </param>
    public void AddToQueue(TAwaitPlayer player);

    /// <summary xml:lang = "ru">
    /// Создает задачу на присоединение игрока к игре.
    /// </summary>
    /// <param name="player" xml:lang = "ru">
    /// Игрок типа <typeparamref name="TAwaitPlayer"/>, ожидающий игру.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Задачу типа <see cref="Task{TResult}"/>, результатом
    /// которой будет состояние игровой сессии типа <see cref="ISessionState{TLinkedValue}"/>.
    /// </returns>
    public Task<ISessionState<IEnumerable<ICard>>> JoinToGame(TAwaitPlayer player);

    /// <summary xml:lang = "ru">
    /// Обноаляет игровую сессию.
    /// </summary>
    /// <param name="sessionId" xml:lang = "ru">
    /// Идентификатор сессии.
    /// </param>
    /// <param name="event" xml:lang = "ru">
    /// Событие, произошедшее в игре.
    /// </param>
    /// <param name="player" xml:lang = "ru">
    /// Игрок, отправивший сообщение о событии.
    /// </param>
    /// <param name="card" xml:lang = "ru">
    /// Прикрепленная к событию карта.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Задачу, которая завершится после обновления сессии.
    /// </returns>
    public Task UpdateSession(GameSessionId sessionId, IGameEvent @event, IPlayer player);

    /// <summary xml:lang = "ru">
    /// Создает задачу на обновление игровой сессии, которая
    /// выполнится когда поступит обновление.
    /// </summary>
    /// <param name="sessionId" xml:lang = "ru">
    /// Идентификатор сессии.
    /// </param>
    /// <param name="version" xml:lang = "ru">
    /// Версия сессии.
    /// </param>
    /// <param name="player" xml:lang = "ru">
    /// Игрок, запросивший обновление.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Задачу типа <see cref="Task{TResult}"/>, результатом
    /// которой будет состояние игровой сессии типа <see cref="ISessionState{TLinkedValue}"/>.
    /// </returns>
    public Task<ISessionState<IEnumerable<ICard>>> GetUpdateForSession(GameSessionId sessionId, int version, IPlayer player);
}
