using CardGameDurak.Abstractions;

namespace CardGameDurak.Network;
internal class SingleGameController : IGameController
{
    private readonly IGameCoordinator<IAwaitPlayer> _gameCoordinator;
    private readonly IBot _bot;

    internal SingleGameController(
        IGameCoordinator<IAwaitPlayer> coordinator,
        IBot bot)
    {
        _bot = bot!;
        _gameCoordinator = coordinator!;
    }
}