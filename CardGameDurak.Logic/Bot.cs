using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;
/// <summary xml:lang = "ru">
/// Класс бота.
/// </summary>
public class Bot : PlayerBase, IBot
{
    private readonly IBotStrategy _strategy;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр класса <see cref="Bot"/>.
    /// </summary>
    /// <param name="name" xml:lang = "ru">
    /// Имя бота.
    /// </param>
    /// <param name="strategy" xml:lang = "ru">
    /// Стратегия, которой будет пользоваться бот во время игры.
    /// </param>
    public Bot(string name, IBotStrategy strategy) : base(name) => 
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
}
