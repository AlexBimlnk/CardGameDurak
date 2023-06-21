using System.Collections.Concurrent;

using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Controlling;
using CardGameDurak.Abstractions.GameSession;
using CardGameDurak.Abstractions.Players;
using CardGameDurak.Logic;
using CardGameDurak.Service.Models;

using Logic;

using Microsoft.Extensions.Options;

namespace CardGameDurak.Service;

internal sealed class CloudCoordinator : IGameCoordinator<CloudAwaitPlayer>
{
    private readonly ConcurrentDictionary<GameSessionId, GameSession> _sessions = new();
    private readonly ConcurrentDictionary<ValueTuple<GameSessionId, int>, TaskCompletionSource<IGameSession>> _tcsOnUpdateSessions = new();

    private readonly List<CloudAwaitPlayer> _awaiterPlayers = new();

    private readonly object _hostGuard = new();
    private const int STORE_UPDATED_IN_SECONDS = 30;

    private readonly ILogger<CloudCoordinator> _logger;
    private readonly GameSessionConfiguration _configuration;

    public CloudCoordinator(ILogger<CloudCoordinator> logger, IOptions<GameSessionConfiguration> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = options?.Value ?? throw new ArgumentNullException(nameof(options));

        _logger.LogDebug($"{nameof(CloudCoordinator)} started");
    }

    public string Name => "I SINGLE COORDINATOR";

    private List<ICard> CreateDeck() => throw new NotImplementedException();

    private ISessionState<IEnumerable<ICard>> GetSessionState(IGameSession session, IPlayer player) =>
        new SessionState((GameSession)session, (Card[])session.GetPlayerCards(player));

    private async Task<ISessionState<IEnumerable<ICard>>> CreateTaskOnUpdateAsync(
        GameSessionId sessionId,
        int sessionVersion,
        IPlayer player)
    {
        var key = (sessionId, sessionVersion);

        if (!_tcsOnUpdateSessions.TryGetValue(key, out var updateTCS))
        {
            var tcs = new TaskCompletionSource<IGameSession>();

            _tcsOnUpdateSessions.TryAdd(key, tcs);

            _ = Task.Delay(TimeSpan.FromSeconds(STORE_UPDATED_IN_SECONDS))
                .ContinueWith(_ =>
                {
                    if (_tcsOnUpdateSessions.TryRemove(key, out tcs))
                        _logger.LogDebug("Removed updated version {Version} " +
                            "of session with {Id}",
                            key.sessionVersion,
                            key.sessionId.Value);

                });

            updateTCS = tcs;
        }

        return GetSessionState(await updateTCS.Task, player);
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
                    _configuration,
                    CreateDeck(),
                    players);

                if (_sessions.TryAdd(sessionId, session))
                {

                    foreach (CloudAwaitPlayer player in group.Players)
                    {
                        _awaiterPlayers.Remove(player);

                        var sessionState = new SessionState(
                            session,
                            (Card[])session.GetPlayerCards(player.Player));

                        player.JoinTCS.SetResult(sessionState);
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
    public void AddToQueue(CloudAwaitPlayer player)
    {
        lock (_hostGuard)
        {
            _awaiterPlayers.Add(player ?? throw new ArgumentNullException(nameof(player)));
        }

        _logger.LogInformation("Added new player {Player} to await start game", player);

        _logger.LogDebug("Try start new game");

        if (!TryHostGames())
            _logger.LogDebug("Failed to host any games");
    }

    /// <inheritdoc/>
    public Task<ISessionState<IEnumerable<ICard>>> JoinToGame(CloudAwaitPlayer player) => player.JoinTCS.Task;

    /// <inheritdoc/>
    public Task UpdateSession(GameSessionId sessionId, IGameEvent @event, IPlayer player)
    {
        ArgumentNullException.ThrowIfNull(player, nameof(player));

        if (_sessions.TryGetValue(sessionId, out var session))
        {
            return Task.Run(() =>
            {
                var oldVersion = session.Version;

                //Todo: update session state

                if (_tcsOnUpdateSessions.TryGetValue((sessionId, oldVersion), out var tcs))
                {
                    tcs.SetResult(session);
                }
                _logger.LogDebug("Updated session with id {Id}", sessionId.Value);
            });
        }
        else
            throw new KeyNotFoundException();
    }

    /// <inheritdoc/>
    public Task<ISessionState<IEnumerable<ICard>>> GetUpdateForSession(GameSessionId sessionId, int version, IPlayer player)
    {
        ArgumentNullException.ThrowIfNull(player, nameof(player));

        if (_sessions.TryGetValue(sessionId, out var serviceSession))
        {
            return serviceSession.Version.Equals(version) switch
            {
                // Если сессия уже успела обновиться возвращаем результат.
                false => Task.FromResult(GetSessionState(serviceSession, player)),
                true => CreateTaskOnUpdateAsync(sessionId, version, player)
            };
        }

        throw new KeyNotFoundException();
    }
}
