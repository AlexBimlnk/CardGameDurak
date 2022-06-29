namespace CardGameDurak.Abstractions.Players;

/// <summary xml:lang = "ru">
/// Контракт, описывающий компьютерного игрока.
/// </summary>
public interface IBot : IPlayer
{
    /// <summary xml:lang = "ru">
    /// Подкидывает карты.
    /// </summary>
    /// <param name="desktopCards" xml:lang = "ru">
    /// Список карт, которые сейчас на столе.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Карту типа <see cref="ICard"/>, которую он решил подкинуть.
    /// </returns>
    public ICard Attak(IReadOnlyCollection<ICard> desktopCards);
    /// <summary xml:lang = "ru">
    /// Закрывает карту карту на столе.
    /// </summary>
    /// <param name="desktopCards" xml:lang = "ru">.
    /// Список карт, которые сейчас на столе.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Карту типа <see cref="ICard"/>, которой он отбивается.
    /// </returns>
    public ICard Defence(IReadOnlyCollection<ICard> desktopCards);
}
