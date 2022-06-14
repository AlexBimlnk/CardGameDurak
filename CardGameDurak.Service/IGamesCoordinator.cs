using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

/// <summary xml:lang = "ru">
/// Контракт, описывающий игровой координатор, 
/// который хранит и обновляет игровые сессии.
/// </summary>
public interface IGamesCoordinator
{
    public string Name { get; }

    /// <summary xml:lang = "ru">
    /// Добавляет в очередь игрока на ожидание игры.
    /// </summary>
    /// <param name="player" xml:lang = "ru">
    /// Игрок, ожидающий игру.
    /// </param>
    public void AddToQueue(AwaitPlayer player);

    /// <summary xml:lang = "ru">
    /// Создает обещание на присоединение игрока к игре.
    /// </summary>
    /// <param name="player" xml:lang = "ru">
    /// Игрок, ожидающий игру.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Задачу типа <see cref="Task{TResult}"/>, 
    /// где рузультатом является идентификатор игровой сессии.
    /// </returns>
    public Task<long> PromisJoinToGame(AwaitPlayer player);

    /// <summary xml:lang = "ru">
    /// Обноаляет игровую сессию.
    /// </summary>
    /// <param name="message" xml:lang = "ru">
    /// Сообщение, содержащие новое событие в игре.
    /// </param>
    /// <returns xml:lang = "ru">
    /// TODO: Сигнатура может поменяться.
    /// </returns>
    public Task UpdateSession(IEventMessage message);

    /// <summary xml:lang = "ru">
    /// Обещает обновить сессию как только поступит обновление.
    /// </summary>
    /// <param name="session" xml:lang = "ru">
    /// Игровая сессия, которую нужно будет обновить.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Обновленную игровую сессию.
    /// </returns>
    public Task<IGameSession> PromisUpdateSession(IGameSession session);
}
