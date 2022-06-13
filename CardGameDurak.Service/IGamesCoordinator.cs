using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

public interface IGamesCoordinator
{
    public string Name { get; }

    public void AddToQueue(AwaitPlayer player);

    public Task<long> JoinToGame(AwaitPlayer player);

    public Task UpdateSession(GameSessionId id, IEventMessage message);
}
