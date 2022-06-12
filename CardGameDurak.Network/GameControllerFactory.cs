using CardGameDurak.Abstractions;

namespace CardGameDurak.Network;

/// <summary xml:lang = "ru">
/// Фабрика создания игровых контроллеров.
/// </summary>
public static class GameControllerFactory
{
    /// <summary xml:lang = "ru">
    /// Создает новый игровой контролле для игры по локальной сети.
    /// </summary>
    /// <param name="addresses" xml:lang = "ru">
    /// Список адресов играющих игроков.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Игровой контроллер типа <see cref="IGameController"/>.
    /// </returns>
    public static IGameController CreateLocalController(IReadOnlyCollection<string> addresses) => new LocalGameController(addresses);

    /// <summary xml:lang = "ru">
    /// Создает новый игровой контроллер для игры по сети интернет.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Игровой контроллер типа <see cref="IGameController"/>.
    /// </returns>
    public static IGameController CreateCloudController() => new CloudGameController();

    /// <summary xml:lang = "ru">
    /// Создает новый игровой контроллер для одиночной игры.
    /// </summary>
    /// <param name="bot" xml:lang = "ru">
    /// Компьютерный игрок.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Игровой контроллер типа <see cref="IGameController"/>.
    /// </returns>
    public static IGameController CreateSingleGameController(IBot bot) => new SingleGameController(bot);
}
