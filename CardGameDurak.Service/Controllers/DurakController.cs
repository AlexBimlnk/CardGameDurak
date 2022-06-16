using CardGameDurak.Abstractions;
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
    public async Task<IGameSession> UpdateSessionAsync([FromBody] GameSession session)
    {
        ArgumentNullException.ThrowIfNull(session, nameof(session));

        _logger.LogDebug("Receive update request");

        var updateSession = await _gamesCoordinator.GetUpdateForSession(session);

        _logger.LogDebug("Get update session from {@OldSession} to {@UpdateSession}",
            session,
            updateSession);

        return session;
    }

    [HttpPost("event")]
    public async Task PostEventAsync(IEventMessage message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _logger.LogDebug("Receive event message");

        await _gamesCoordinator.UpdateSession(message);

        throw new NotImplementedException();
    }
}
