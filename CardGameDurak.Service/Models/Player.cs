using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Abstractions.Players;

namespace CardGameDurak.Service.Models;

public sealed class Player : IPlayer, ISender
{
    public int? Id { get; set ; }

    public string Name { get; set; }

    public int CountCards { get; set; }
}
