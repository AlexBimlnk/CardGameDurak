using Microsoft.AspNetCore.Mvc;

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
}
