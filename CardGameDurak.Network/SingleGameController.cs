using CardGameDurak.Abstractions;

namespace CardGameDurak.Network;
internal class SingleGameController : IGameController
{
    private readonly IGameCoordinator<IAwaitPlayer> _gameCoordinator;
    private readonly IBot _bot;
    public SingleGameController(
        IGameCoordinator<IAwaitPlayer> coordinator,
        IBot bot)
    {
        _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        _gameCoordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
    }
}