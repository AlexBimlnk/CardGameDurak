namespace CardGameDurak.Abstractions;

/// <summary xml:lang = "ru">
/// Контракт, описывающий стратегию, которой пользуется бот.
/// </summary>
public interface IBotStrategy
{
    /// <summary xml:lang = "ru">
    /// Пробует применить стратегию атаки.
    /// </summary>
    /// <param name="handCards" xml:lang = "ru">
    /// Карты, находящиеся в руке бота.
    /// </param>
    /// <param name="desktopCards" xml:lang = "ru">
    /// Карты, находящиеся на игровом столе.
    /// </param>
    /// <param name="resultCard" xml:lang = "ru">
    /// Карта, которую нужно подкинуть. Если карта для атаки не найдется,
    /// то этот параметр будет <see langword="null"/>.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see langword="true"/>, если нашлась карта для атаки, 
    /// иначе - <see langword="false"/>.
    /// </returns>
    public bool TryAttack(
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        out ICard? resultCard);

    /// <summary xml:lang = "ru">
    /// Пробует применить стратегию обороны.
    /// </summary>
    /// <param name="ownerId" xml:lang = "ru">
    /// Идентификатор бота, использующего стратегию.
    /// </param>
    /// <param name="handCards" xml:lang = "ru">
    /// Карты, находящиеся в руке бота.
    /// </param>
    /// <param name="desktopCards" xml:lang = "ru">
    /// Карты, находящиеся на игровом столе.
    /// </param>
    /// <param name="resultCard" xml:lang = "ru">
    /// Карта, которой бот отбивается. Если карта для защиты не найдется,
    /// то этот параметр будет <see langword="null"/>.
    /// </param>
    /// <param name="closedCard" xml:lang = "ru">
    /// Карта, которую бот должен закрыть на столе.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see langword="true"/>, если нашлась карта для защиты, 
    /// иначе - <see langword="false"/>.
    /// </returns>
    public bool TryDefence(
        int ownerId,
        IReadOnlyCollection<ICard> handCards,
        IReadOnlyCollection<ICard> desktopCards,
        out ICard? resultCard,
        out ICard closedCard);

}
