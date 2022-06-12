using CardGameDurak.Abstractions;

namespace CardGameDurak.Network;
internal class SingleGameController : IGameController
{
    private readonly IPlayer _bot;
    public SingleGameController(IPlayer bot) => 
        _bot = bot ?? throw new ArgumentNullException(nameof(bot));
}