namespace CardGameDurak.Abstractions.GameSession;

/// <summary xml:lang = "ru">
/// Контракт, описывающий состояние игровой сессии.
/// </summary>
/// <typeparam name="TLinkedValue" xml:lang = "ru">
/// Тип прикрепленного к состоянию сессии значения.
/// </typeparam>
public interface ISessionState<TLinkedValue>
{
    /// <summary xml:lang = "ru">
    /// Игровая сесссия.
    /// </summary>
    public IGameSession Session { get; }

    /// <summary xml:lang = "ru">
    /// Прикрепленное значение типа <typeparamref name="TLinkedValue"/>.
    /// </summary>
    public TLinkedValue LinkedValue { get; }
}
