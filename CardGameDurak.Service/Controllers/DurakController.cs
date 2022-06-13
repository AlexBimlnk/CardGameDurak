using Microsoft.AspNetCore.Mvc;

using CardGameDurak.Service.Models;
using CardGameDurak.Abstractions.Messages;
using CardGameDurak.Service.Models.Messages;

namespace CardGameDurak.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class DurakController : Controller
{
    private readonly ILogger<DurakController> _logger;
    private readonly IGamesCoordinator _gamesCoordinator;

    public DurakController(
        ILogger<DurakController> logger,
        IGamesCoordinator coordinator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gamesCoordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
    }

    [HttpGet]
    public string Get() => _gamesCoordinator.Name;

    [HttpGet("join")]
    public async Task<StartGameMessage> JoinToGameAsync(IJoinMessage message)
    {
        var tcs = new TaskCompletionSource();

        _gamesCoordinator.AddToQueue(
            new Player(
                message.Player.Name, 
                message.AwaitPlayersCount));

        throw new NotImplementedException();
    }
}
