using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.GameSession;

namespace CardGameDurak.Logic;
internal class GameSessionCondition: IEquatable<IGameSession>
{
    public GameSessionId SessionId { get; set; }
    public int CountPlayers { get; }
    public int[]? CountCards { get;  }
    public int CountDesktopCards { get; }
    public ICard[]? DesktopCards { get;  }
    bool IEquatable<IGameSession>.Equals(IGameSession? oldSessionCondition)
    {
        var newCondition = new GameSessionCondition();
        if (newCondition is null)
            return false;
        else
            return (base.Equals(oldSessionCondition) && CountPlayers == newCondition.CountPlayers) 
                   && (base.Equals(oldSessionCondition) && CountDesktopCards == newCondition.CountDesktopCards);  
    }
}
