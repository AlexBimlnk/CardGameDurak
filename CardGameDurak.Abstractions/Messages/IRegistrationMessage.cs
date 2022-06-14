namespace CardGameDurak.Abstractions.Messages;

/// <summary xml:lang = "ru">
/// Контракт, описывающий сообщение, сообщающее о регистрации пользователя в игре.
/// </summary>
public interface IRegistrationMessage
{
    /// <summary xml:lang = "ru">
    /// Идентификатор игры.
    /// </summary>
    public GameSessionId SessionId { get; }

    /// <summary xml:lang = "ru">
    /// Идентификатор игрока.
    /// </summary>
    public int PlayerId { get; }
}
