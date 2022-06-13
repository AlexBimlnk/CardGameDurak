namespace CardGameDurak.Service;

public static class Registrations
{
    public static void AddGameCoorditanor(this IServiceCollection services) =>
        services.AddSingleton<IGamesCoordinator, GamesCoordinator>();
}
