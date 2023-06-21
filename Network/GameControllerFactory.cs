using CardGameDurak.Abstractions.Controlling;
using CardGameDurak.Abstractions.Players;

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
    /// <param name="coordinator" xml:lang = "ru">
    /// Координатор игр.
    /// </param>
    /// <param name="bot" xml:lang = "ru">
    /// Компьютерный игрок.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Игровой контроллер типа <see cref="IGameController"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой аргумент равен <see langword="null"/>.
    /// </exception>
    public static IGameController CreateSingleGameController(
        IGameCoordinator<IAwaitPlayer> coordinator, 
        IBot bot)
    {
        ArgumentNullException.ThrowIfNull(coordinator, nameof(coordinator));
        ArgumentNullException.ThrowIfNull(bot, nameof(bot));

        return new SingleGameController(coordinator, bot);
    }
}
