using CardGameDurak.Abstractions.Controlling;
using CardGameDurak.Logic;
using CardGameDurak.Service.Models;

using Microsoft.Extensions.Options;

namespace CardGameDurak.Service;

/// <summary xml:lang = "ru">
/// Содержит методы расширения для DI.
/// </summary>
internal static class Registrations
{

    /// <summary xml:lang = "ru">
    /// Регистрирует нужные зависимости в DI.
    /// </summary>
    /// <param name="services" xml:lang = "ru">
    /// Коллекция служб.
    /// </param>
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGameCoorditanor();
        services.AddConfigurations(configuration);
    }

    private static void AddGameCoorditanor(this IServiceCollection services) =>
        services.AddSingleton<ICloudSender, CloudSender>()
                .AddSingleton<IGameCoordinator<CloudAwaitPlayer>, CloudCoordinator>();

    private static void AddConfigurations(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<GameSessionConfiguration>(configuration.GetSection(nameof(GameSessionConfiguration)))
                .AddSingleton<IValidateOptions<GameSessionConfiguration>, GameSessionConfigurationValidator>();

}
