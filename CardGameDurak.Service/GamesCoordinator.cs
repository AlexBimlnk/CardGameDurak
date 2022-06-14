using System.Collections.Concurrent;

using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Logic;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

internal class GamesCoordinator : IGamesCoordinator
{
    private long _currentGameId = 1;
    private readonly ConcurrentDictionary<GameSessionId, GameSession> _sessions = new();
    private readonly List<AwaitPlayer> _awaiterPlayers = new();
    private readonly ILogger<GamesCoordinator> _logger;

    public GamesCoordinator(ILogger<GamesCoordinator> logger) =>
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public string Name => "I SINGLE COORDINATOR";

    private List<ICard> CreateDeck() => throw new NotImplementedException();

    private bool TryHostGames()
    {
        var playerGroupsToNewGame = _awaiterPlayers
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
            var players = group.Players.Select(p => p.Player);

            var sessionId = new GameSessionId(_currentGameId++);
            var session = new GameSession(
                sessionId,
                CreateDeck(),
                players);

            if (_sessions.TryAdd(sessionId, session))
            {

                foreach (AwaitPlayer player in group.Players)
                {
                    _awaiterPlayers.Remove(player);
                    player.JoinTCS.SetResult(sessionId.Value);
                }

                _logger.LogInformation("Start new session: {@Session}", session);
            }
            else
                _logger.LogDebug("Fail add new session: {@Session}", session);
        });

        return true;
    }

    /// <inheritdoc/>
    public void AddToQueue(AwaitPlayer player)
    {
        _awaiterPlayers.Add(player ?? throw new ArgumentNullException(nameof(player)));

        _logger.LogInformation("Added new player:{@Player} to await start game", player);

        _logger.LogDebug("Try start new game");

        if (!TryHostGames())
            _logger.LogDebug("Failed to host any games");
    }

    /// <inheritdoc/>
    public Task<long> JoinToGame(AwaitPlayer player) => player.JoinTCS.Task;

    /// <inheritdoc/>
    public Task UpdateSession(IEventMessage message) => throw new NotImplementedException();
    
    /// <inheritdoc/>
    public IGameSession GetSession(GameSessionId sessionId)
    {
        ArgumentNullException.ThrowIfNull(sessionId, nameof(sessionId));

        return _sessions.TryGetValue(sessionId, out var session) switch
        {
            true => session,
            false => throw new KeyNotFoundException()
        };
    }
}
