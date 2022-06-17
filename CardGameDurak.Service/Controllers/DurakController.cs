using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Logic;
using CardGameDurak.Network.Messages;
using CardGameDurak.Service.Models;

using Microsoft.AspNetCore.Mvc;

namespace CardGameDurak.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DurakController : ControllerBase
{
    private readonly ILogger<DurakController> _logger;
    private readonly IGameCoordinator<AwaitPlayer> _gamesCoordinator;
    private readonly ICloudSender _cloudSender;

    public DurakController(
        ILogger<DurakController> logger,
        IGameCoordinator<AwaitPlayer> coordinator,
        ICloudSender cloudSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gamesCoordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
        _cloudSender = cloudSender ?? throw new ArgumentNullException(nameof(cloudSender));
    }

    [HttpGet]
    public string Get() => _gamesCoordinator.Name;

    /// <summary xml:lang = "ru">
    /// Добавляет игрока в очередь и ждёт его регистрации в игре.
    /// </summary>
    /// <param name="message" xml:lang = "ru">
    /// Сообщение на присоединение к игре.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Сообщение типа <see cref="RegistrationMessage{TValue, TSender}"/> о регистрации в игре.
    /// </returns>
    [HttpGet("join")]
    public async Task<ActionResult<IMessage<IGameSession, ISender>>> JoinToGameAsync([FromBody] JoinMessage<CloudAwaitPlayer> message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _logger.LogDebug("Receive join message");

        _gamesCoordinator.AddToQueue(message.Sender);

        var session = await _gamesCoordinator.JoinToGame(message.Sender);

        _logger.LogDebug("Send registration message");

        return new RegistrationMessage<IGameSession, ISender>(session, _cloudSender);
    }

    [HttpGet("update")]
    public async Task<SessionMessage<IEnumerable<ICard>, ISender>> UpdateSessionAsync([FromBody] UpdateMessage<int, Player> message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _logger.LogDebug("Receive update request");

        var updateSession = await _gamesCoordinator.GetUpdateForSession(message.Key, message.Value, message.Sender);

        _logger.LogDebug("Get update session with id {Id} from {OldVersio} " +
            "version to {NewVersion}",
            message.Key.Value,
            message.Value,
            updateSession.Version);

        return new SessionMessage<IEnumerable<ICard>, ISender>(
            new Tuple<IGameSession, IEnumerable<ICard>>(updateSession, null!), 
            _cloudSender);
    }

    //[HttpPost("event")]
    //public async Task PostEventAsync([FromBody] UpdateMessage<GameEvent, Player> message)
    //{
    //    ArgumentNullException.ThrowIfNull(message, nameof(message));

    //    _logger.LogDebug("Receive event message");

    //    await _gamesCoordinator.UpdateSession(
    //        message.Key, 
    //        message.Value, 
    //        message.Sender);
    //}

    [HttpPost("event")]
    public async Task PostEventAsync([FromBody] UpdateMessage<Tuple<GameEvent, Card>, Player> message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _logger.LogDebug("Receive event message");

        await _gamesCoordinator.UpdateSession(
            message.Key, 
            message.Value.Item1,
            message.Sender,
            message.Value.Item2);
    }
}
