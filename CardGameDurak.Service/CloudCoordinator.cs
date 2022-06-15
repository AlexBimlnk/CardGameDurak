using System.Collections.Concurrent;

using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Logic;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

internal class CloudCoordinator : IGameCoordinator<AwaitPlayer>
{
    private readonly ConcurrentDictionary<GameSessionId, GameSession> _sessions = new();
    private readonly ConcurrentDictionary<ValueTuple<GameSessionId, int>, TaskCompletionSource<IGameSession>> _tcsOnUpdateSessions = new();
    
    private readonly List<AwaitPlayer> _awaiterPlayers = new();

    private readonly object _hostGuard = new();
    private const int STORE_UPDATED_IN_SECONDS = 30;

    private readonly ILogger<CloudCoordinator> _logger;
    
    public CloudCoordinator(ILogger<CloudCoordinator> logger) => 
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));


    public string Name => "I SINGLE COORDINATOR";

    private List<ICard> CreateDeck() => throw new NotImplementedException();

    private Task<IGameSession> CreateTaskOnUpdate(
        GameSessionId sessionId, 
        int sessionVersion)
    {
        var key = (sessionId, sessionVersion);
        if (_tcsOnUpdateSessions.TryGetValue(key, out var updateTCS))
            return updateTCS.Task;
        else
        {
            var tcs = new TaskCompletionSource<IGameSession>();

            _tcsOnUpdateSessions.TryAdd(key, tcs);

            _ = Task.Delay(TimeSpan.FromSeconds(STORE_UPDATED_IN_SECONDS))
                .ContinueWith(_ =>
                {
                    if (_tcsOnUpdateSessions.TryRemove(key, out tcs))
                        _logger.LogDebug("Removed updated version {Version} " +
                            "of session with {@Id}", key.sessionVersion, key.sessionId);

                });

            return tcs.Task;
        }
    }
    
    private bool TryHostGames()
    {
        lock (_hostGuard)
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

                var sessionId = new GameSessionId(_sessions.Count + 1);
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
    }

    /// <inheritdoc/>
    public void AddToQueue(AwaitPlayer player)
    {
        lock (_hostGuard)
        {
            _awaiterPlayers.Add(player ?? throw new ArgumentNullException(nameof(player)));
        }

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
                var oldVersion = session.Version;

                //Todo: update session state

                if (_tcsOnUpdateSessions.TryGetValue((session.Id, oldVersion), out var tcs))
                {
                    tcs.SetResult(session);
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
                true => CreateTaskOnUpdate(serviceSession.Id, serviceSession.Version)
            };
        }
        else
            throw new KeyNotFoundException();
    }
}
