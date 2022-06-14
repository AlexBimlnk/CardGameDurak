using System.Collections.Concurrent;

using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Logic;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

internal class GamesCoordinator : IGamesCoordinator<AwaitPlayer>
{
    private long _currentGameId = 1;
    private readonly ConcurrentDictionary<GameSessionId, GameSession> _sessions = new();
    private readonly ConcurrentDictionary<GameSessionId, TaskCompletionSource<IGameSession>> _updateSessionsTCS = new();
    private readonly List<AwaitPlayer> _awaiterPlayers = new();
    private readonly ILogger<GamesCoordinator> _logger;

    public GamesCoordinator(ILogger<GamesCoordinator> logger) => 
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));


    public string Name => "I SINGLE COORDINATOR";

    private List<ICard> CreateDeck() => throw new NotImplementedException();

    private Task<IGameSession> CreateTaskOnUpdate(GameSessionId sessionId)
    {
        if (_updateSessionsTCS.TryGetValue(sessionId, out var updateTCS))
            return updateTCS.Task;

        var tcs = new TaskCompletionSource<IGameSession>();

        return _updateSessionsTCS.TryAdd(sessionId, tcs) switch
        {
            true => tcs.Task,
            false => _updateSessionsTCS[sessionId].Task
        };
    }
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

                _logger.LogInformation("Start new session {@Session}", session);
            }
            else
                _logger.LogDebug("Fail add new session {@Session}", session);
        });

        return true;
    }

    /// <inheritdoc/>
    public void AddToQueue(AwaitPlayer player)
    {
        _awaiterPlayers.Add(player ?? throw new ArgumentNullException(nameof(player)));

        _logger.LogInformation("Added new player {@Player} to await start game", player);

        _logger.LogDebug("Try start new game");

        if (!TryHostGames())
            _logger.LogDebug("Failed to host any games");
    }

    /// <inheritdoc/>
    public Task<long> JoinToGame(AwaitPlayer player) => player.JoinTCS.Task;

    /// <inheritdoc/>
    public Task UpdateSession(IEventMessage message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (_sessions.TryGetValue(message.SessionId, out var session))
        {
            return Task.Run(() => {
                //Todo: update session state
                if (_updateSessionsTCS.TryGetValue(message.SessionId, out var tcs))
                {
                    tcs.SetResult(session);
                    if (_updateSessionsTCS.TryRemove(message.SessionId, out tcs))
                        _logger.LogDebug("Remove tcs on update for session with id {@Id}",
                            message.SessionId);
                }
                _logger.LogDebug("Updated session with id {@Id}", message.SessionId);
            });
        }
        else
            throw new KeyNotFoundException();
    }
    
    /// <inheritdoc/>
    public Task<IGameSession> GetUpdateForSession(IGameSession session)
    {
        ArgumentNullException.ThrowIfNull(session, nameof(session));

        if (_sessions.TryGetValue(session.Id, out var serviceSession))
        {
            return serviceSession.Equals(session) switch
            {
                // Если сессия уже успела обновиться возвращаем результат.
                false => Task.FromResult<IGameSession>(serviceSession),
                true => CreateTaskOnUpdate(serviceSession.Id)
            };
        }
        else
            throw new KeyNotFoundException();
    }
}
