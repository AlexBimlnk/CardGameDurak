using CardGameDurak.Abstractions;
using CardGameDurak.Logic;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

public class GamesCoordinator : IGamesCoordinator
{
    private long _currentGameId = 1;
    private readonly List<GameSession> _sessions = new List<GameSession>();
    private readonly List<Player> _players = new List<Player>();

    public string Name => "I SINGLE COORDINATOR";

    private List<ICard> CreateDeck() => new List<ICard>();

    private bool TryHostGame()
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
            _sessions.Add(new GameSession(
                new GameSessionId(_currentGameId),
                CreateDeck(),
                group.Players));

            foreach (Player player in group.Players)
                _players.Remove(player);
        });
        _currentGameId++;

        return true;
    }

    public void AddToQueue(Player player)
    {
        _players.Add(player ?? throw new ArgumentNullException(nameof(player)));
        TryHostGame();
    }

    public Task<long> JoinToGame(Player player)
    {
        var tcs = new TaskCompletionSource<long>();
        return tcs.Task;
    }

    public Task UpdateSession(GameSessionId id, Message message) => throw new NotImplementedException();
}
