namespace CardGameDurak.Abstractions.Enums;

/// <summary xml:lang = "ru">
/// События которые может произвести игрок.
/// </summary>
public enum PlayerEvent
{
    /// <summary xml:lang = "ru">
    /// Положил карту на стол.
    /// </summary>
    DropOnDesktop,
    /// <summary xml:lang = "ru">
    /// Забрал карты со стола.
    /// </summary>
    Take,
    /// <summary xml:lang = "ru">
    /// Передал ход.
    /// </summary>
    MoveTurn
}
