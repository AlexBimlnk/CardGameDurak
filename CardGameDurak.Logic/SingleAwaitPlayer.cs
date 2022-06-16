using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;
internal class SingleAwaitPlayer : IAwaitPlayer
{
    public SingleAwaitPlayer(int awaitPlayersCount, IPlayer player)
    {
        Player = player ?? throw new ArgumentNullException(nameof(player));
        AwaitPlayersCount = awaitPlayersCount;
    }

    public int AwaitPlayersCount { get; }

    public IPlayer Player { get; }
}
