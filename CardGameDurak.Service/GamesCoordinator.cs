using CardGameDurak.Abstractions;
using CardGameDurak.Logic;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

public class GamesCoordinator : IGamesCoordinator
{
    private long _currentGameId = 1;
    private readonly Dictionary<GameSessionId,GameSession> _sessions = new();
    private readonly List<Player> _players = new();
    private readonly ILogger<GamesCoordinator> _logger;

    public GamesCoordinator(ILogger<GamesCoordinator> logger) =>
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public string Name => "I SINGLE COORDINATOR";

    private List<ICard> CreateDeck() => throw new NotImplementedException();

    private bool TryHostGames()
    {
        var playerGroupsToNewGame = _players
            .GroupBy(p => p.AwaitPlayersCount)
            .Where(g => g.Key <= g.Count())
            .Select(g => new
                {
                    PlayersCount = g.Key,
                    Players = g.Select(p => p)
                })
            .ToList();

        if (playerGroupsToNewGame.Count == 0)
            return false;

        playerGroupsToNewGame.ForEach(group =>
        {
            var sessionId = new GameSessionId(_currentGameId++);
            var session = new GameSession(
                sessionId, 
                CreateDeck(), 
                group.Players);

            _sessions.Add(sessionId, session);

            foreach (Player player in group.Players)
                _players.Remove(player);

            _logger.LogInformation("Start new session: {Session}", session);
        });

        return true;
    }

    public void AddToQueue(Player player)
    {
        _players.Add(player ?? throw new ArgumentNullException(nameof(player)));

        _logger.LogInformation("Added new player:{Player} to await start game", player);

        _logger.LogDebug("Try start new game");

        if (!TryHostGames())
            _logger.LogDebug("Failed to host any games");
    }

    public Task<long> JoinToGame(Player player)
    {
        var tcs = new TaskCompletionSource<long>();
        return tcs.Task;
    }

    public Task UpdateSession(GameSessionId id, Message message) => throw new NotImplementedException();
}
