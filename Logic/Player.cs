using Logic;

namespace CardGameDurak.Logic;

internal class Player : PlayerBase
{
    public Player(PlayerId playerId, Name name) 
        : base(playerId, name) { }
}
