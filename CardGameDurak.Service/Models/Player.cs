using CardGameDurak.Abstractions;

namespace CardGameDurak.Service.Models;

public class Player : IPlayer
{
    public int? Id { get; set ; }

    public string Name { get; set; }

    public int CountCards { get; set; }
}
