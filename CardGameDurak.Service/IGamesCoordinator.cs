using CardGameDurak.Service.Models;
using CardGameDurak.Abstractions;
using CardGameDurak.Service.Models.Messages;

namespace CardGameDurak.Service;

public interface IGamesCoordinator
{
    public string Name { get; }

    public void AddToQueue(Player player);

    public Task<long> JoinToGame(Player player);

    public Task UpdateSession(GameSessionId id, EventMessage message);
}
