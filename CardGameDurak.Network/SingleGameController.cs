using CardGameDurak.Abstractions;

namespace CardGameDurak.Network;
internal class SingleGameController : IGameController
{
    private readonly IBot _bot;
    public SingleGameController(IBot bot) => 
        _bot = bot ?? throw new ArgumentNullException(nameof(bot));
}