using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLogic.Players
{
    public interface IPlayer
    {
        event EventHandler<Card> OnPlayerCardDropped;
        int CountCards { get; }
    }
}
