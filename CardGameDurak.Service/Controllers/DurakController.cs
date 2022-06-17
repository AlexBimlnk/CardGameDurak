using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Logic;
using CardGameDurak.Network.Messages;
using CardGameDurak.Service.Models;
using CardGameDurak.Service.Models.Messages;

using Microsoft.AspNetCore.Mvc;

namespace CardGameDurak.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DurakController : ControllerBase
{
    private readonly ILogger<DurakController> _logger;
    private readonly IGameCoordinator<AwaitPlayer> _gamesCoordinator;

    public DurakController(
        ILogger<DurakController> logger,
        IGameCoordinator<AwaitPlayer> coordinator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gamesCoordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
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
    /// Сообщение типа <see cref="IRegistrationMessage"/> о регистрации в игре.
    /// </returns>
    [HttpGet("join")]
    public async Task<ActionResult<IRegistrationMessage>> JoinToGameAsync([FromBody] JoinMessage<Player> message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _logger.LogDebug("Receive join message");

        var awaitablePlayer = new AwaitPlayer(message);

        _gamesCoordinator.AddToQueue(awaitablePlayer);

        var sessionId = await _gamesCoordinator.JoinToGame(awaitablePlayer);

        _logger.LogDebug("Send registration message");

        return new RegistrationMessage(
            new GameSessionId(sessionId),
            awaitablePlayer.Player.Id);
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
