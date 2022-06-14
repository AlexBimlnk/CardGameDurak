using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

/// <summary xml:lang = "ru">
/// 
/// </summary>
public interface IGamesCoordinator
{
    public string Name { get; }

    /// <summary xml:lang = "ru">
    /// 
    /// </summary>
    /// <param name="player" xml:lang = "ru">
    /// 
    /// </param>
    public void AddToQueue(AwaitPlayer player);

    /// <summary xml:lang = "ru">
    /// 
    /// </summary>
    /// <param name="player" xml:lang = "ru">
    /// 
    /// </param>
    /// <returns xml:lang = "ru">
    /// 
    /// </returns>
    public Task<long> JoinToGame(AwaitPlayer player);

    /// <summary xml:lang = "ru">
    /// 
    /// </summary>
    /// <param name="message" xml:lang = "ru">
    /// 
    /// </param>
    /// <returns xml:lang = "ru">
    /// 
    /// </returns>
    public Task UpdateSession(IEventMessage message);

    /// <summary xml:lang = "ru">
    /// 
    /// </summary>
    /// <param name="session" xml:lang = "ru">
    /// 
    /// </param>
    /// <returns xml:lang = "ru">
    /// 
    /// </returns>
    public Task<IGameSession> GetUpdateForSession(IGameSession session);
}
