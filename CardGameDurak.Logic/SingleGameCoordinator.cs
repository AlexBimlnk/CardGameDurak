using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;

namespace CardGameDurak.Logic;
internal class SingleGameCoordinator : IGameCoordinator<AwaitPlayer>
{
    public string Name => throw new NotImplementedException();

    public void AddToQueue(AwaitPlayer player) => throw new NotImplementedException();
    public Task<ISessionState<IEnumerable<ICard>>> GetUpdateForSession(GameSessionId sessionId, int version, IPlayer player) => throw new NotImplementedException();
    public Task<ISessionState<IEnumerable<ICard>>> JoinToGame(AwaitPlayer player) => throw new NotImplementedException();
    public Task UpdateSession(GameSessionId sessionId, IGameEvent @event, IPlayer player) => throw new NotImplementedException();
}
