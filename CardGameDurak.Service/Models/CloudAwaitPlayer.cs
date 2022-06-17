using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;

using Newtonsoft.Json;

namespace CardGameDurak.Service.Models;

/// <summary xml:lang = "ru">
/// Игрок ожидающий игру на сервисе.
/// </summary>
public sealed class CloudAwaitPlayer : AwaitPlayer, ISender
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CloudAwaitPlayer"/>.
    /// </summary>
    /// <param name="awaitPlayersCount" xml:lang = "ru">
    /// Количество ожидаемых в игре игроков.
    /// </param>
    /// <param name="player" xml:lang = "ru">
    /// Игрок.
    /// </param>
    public CloudAwaitPlayer(int awaitPlayersCount, Player player) : base(awaitPlayersCount, player) { }

    /// <summary xml:lang = "ru">
    /// TCS на присоединение к игре.
    /// </summary>
    [JsonIgnore]
    public TaskCompletionSource<IGameSession> JoinTCS { get; } = new();
}
