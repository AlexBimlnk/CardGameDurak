using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic;

public interface ICard
{
    public CardType Type { get; }

    public Suit Suit { get; } 

    public bool IsTrump { get; }
}
