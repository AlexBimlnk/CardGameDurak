using CardGameDurak.Abstractions;
using CardGameDurak.Service.Models;
using CardGameDurak.Service.Models.Messages;

namespace CardGameDurak.Service;

internal interface IGamesCoordinator
{
    public string Name { get; }

    public void AddToQueue(AwaitPlayer player);

    public Task<long> JoinToGame(AwaitPlayer player);

    public Task UpdateSession(GameSessionId id, EventMessage message);
}
