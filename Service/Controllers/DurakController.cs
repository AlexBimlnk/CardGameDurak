using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Controlling;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.GameSession;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Network.Messages;
using CardGameDurak.Service.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace CardGameDurak.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DurakController : ControllerBase
{
    private readonly ILogger<DurakController> _logger;
    private readonly IGameCoordinator<CloudAwaitPlayer> _gamesCoordinator;
    private readonly ICloudSender _cloudSender;

    public DurakController(
        ILogger<DurakController> logger,
        IGameCoordinator<CloudAwaitPlayer> coordinator,
        ICloudSender cloudSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gamesCoordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
        _cloudSender = cloudSender ?? throw new ArgumentNullException(nameof(cloudSender));
    }

    private async Task<string> ReadRequestBodyAsync()
    {
        using (var reader = new StreamReader(Request.Body))
        {
            return await reader.ReadToEndAsync();
        }
    }

    [HttpGet]
    public string Get() => _gamesCoordinator.Name;

    /// <summary xml:lang = "ru">
    /// Добавляет игрока в очередь на подключение к новой игре.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Сообщение типа <see cref="IMessage{TValue, TSender}"/> 
    /// содержащее состояние игровой сессии, к которой присоединили игрока.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда сообщение равно или не может быть дессериализовано.
    /// </exception>
    [HttpGet("join")]
    public async Task<ActionResult<IMessage<ISessionState<IEnumerable<ICard>>, ISender>>> JoinToGameAsync()
    {
        var body = await ReadRequestBodyAsync();
        var message = JsonConvert.DeserializeObject<JoinMessage<CloudAwaitPlayer>>(body);


        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _logger.LogDebug("Receive join message");

        _gamesCoordinator.AddToQueue(message.Sender);

        var sessionState = await _gamesCoordinator.JoinToGame(message.Sender);

        _logger.LogDebug("Send registration message");

        return new SessionMessage<IEnumerable<ICard>, ISender>(sessionState, _cloudSender);
    }

    /// <summary xml:lang = "ru">
    /// Обновляет игровую сессию игрока.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Сообщение типа <see cref="IMessage{TValue, TSender}"/> 
    /// содержащее состояние запрашиваемой игровой сессии.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда сообщение равно или не может быть дессериализовано.
    /// </exception>
    [HttpGet("update")]
    public async Task<ActionResult<IMessage<ISessionState<IEnumerable<ICard>>, ISender>>> UpdateSessionAsync()
    {
        var body = await ReadRequestBodyAsync();
        var message = JsonConvert.DeserializeObject<UpdateMessage<int, Player>>(body);


        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _logger.LogDebug("Receive update request");

        var sessionState = await _gamesCoordinator.GetUpdateForSession(message.Key, message.Value, message.Sender);

        _logger.LogDebug("Get update session with id {Id} from {OldVersio} " +
            "version to {NewVersion}",
            message.Key.Value,
            message.Value,
            sessionState.Session.Version);

        return new SessionMessage<IEnumerable<ICard>, ISender>(sessionState, _cloudSender);
    }

    /// <summary xml:lang = "ru">
    /// Принимает сообщение о новом игровом событии.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Задачу, которая завершится после обновления игровой сессии.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда сообщение равно или не может быть дессериализовано.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Если данные о игровом событии выходят за рамки 
    /// допустимого перечисления <see cref="PlayerEvent"/>.
    /// </exception>
    [HttpPost("event")]
    public async Task PostEventAsync()
    {
        var body = await ReadRequestBodyAsync();
        var message = JsonConvert.DeserializeObject<UpdateMessage<GameEvent, Player>>(body);


        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _logger.LogDebug("Receive event message");

        await _gamesCoordinator.UpdateSession(
            message.Key,
            message.Value,
            message.Sender);
    }
}
