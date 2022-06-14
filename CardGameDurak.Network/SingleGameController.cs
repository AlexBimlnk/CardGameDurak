using CardGameDurak.Abstractions;

namespace CardGameDurak.Network;
internal class SingleGameController : IGameController
{
    private readonly IGamesCoordinator<IAwaitPlayer> _gameCoordinator;
    private readonly IBot _bot;
    public SingleGameController(
        IGamesCoordinator<IAwaitPlayer> coordinator,
        IBot bot)
    {
        _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        _gameCoordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
    }
}