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
    /// Задачу типа <see cref="Task{TResult}"/>, которая выполнится,
    /// когда игрока подключат к игре. Результатом выполнения
    /// является идентификатор игровой сессии.
    /// </returns>
    public Task<long> JoinToGame(TAwaitPlayer player);

    /// <summary xml:lang = "ru">
    /// Обноаляет игровую сессию.
    /// </summary>
    /// <param name="message" xml:lang = "ru">
    /// Сообщение, содержащие новое событие в игре.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Задачу, которая завершится после обновления сессии.
    /// </returns>
    public Task UpdateSession(IEventMessage message);

    /// <summary xml:lang = "ru">
    /// Создает задачу на обновление игровой сессии, которая
    /// выполнится когда поступит обновление.
    /// </summary>
    /// <param name="session" xml:lang = "ru">
    /// Игровая сессия, которую нужно будет обновить.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Задачу типа <see cref="Task{TResult}"/>, результатом
    /// которой будет игровая сессия типа <see cref="IGameSession"/>.
    /// </returns>
    public Task<IGameSession> GetUpdateForSession(IGameSession session);
}
