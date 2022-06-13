using Microsoft.AspNetCore.Mvc;

using CardGameDurak.Service.Models;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Abstractions;
using CardGameDurak.Service.Models.Messages;

namespace CardGameDurak.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DurakController : ControllerBase
{
    private readonly ILogger<DurakController> _logger;
    private readonly IGamesCoordinator _gamesCoordinator;

    public DurakController(
        ILogger<DurakController> logger,
        IGamesCoordinator coordinator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gamesCoordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));

        _logger.LogDebug($"Created {nameof(DurakController)}");
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
    public async Task<IRegistrationMessage> JoinToGameAsync(IJoinMessage message)
    {
        _logger.LogDebug("Receive join message");

        var tcs = new TaskCompletionSource();

        var awaitablePlayer = new AwaitPlayer(message);

        _gamesCoordinator.AddToQueue(awaitablePlayer);

        var sessionId = await _gamesCoordinator.JoinToGame(awaitablePlayer);

        _logger.LogDebug("Send registration message");

        return new RegistrationMessage(
            new GameSessionId(sessionId),
            awaitablePlayer.Player.Id);
    }

    [HttpGet("update")]
    public void UpdateGame(IEventMessage message) => throw new NotImplementedException();
}
