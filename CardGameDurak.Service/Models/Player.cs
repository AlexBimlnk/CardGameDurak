using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Service.Models;

public sealed class Player : IPlayer, ISender
{
    public int? Id { get; set ; }

    public string Name { get; set; }

    public int CountCards { get; set; }
}
