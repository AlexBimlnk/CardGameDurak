using CardGameDurak.Abstractions;
using CardGameDurak.Service.Models;

namespace CardGameDurak.Service;

/// <summary xml:lang = "ru">
/// Содержит методы расширения для DI.
/// </summary>
internal static class Registrations
{
    /// <summary xml:lang = "ru">
    /// Регистрирует игрового координатора в DI.
    /// </summary>
    /// <param name="services" xml:lang = "ru">
    /// Коллекция служб.
    /// </param>
    public static void AddGameCoorditanor(this IServiceCollection services) =>
        services.AddSingleton<ICloudSender, CloudSender>()
                .AddSingleton<IGameCoordinator<CloudAwaitPlayer>, CloudCoordinator>();
}
