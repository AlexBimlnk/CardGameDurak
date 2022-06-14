using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Service.Models.Messages;

/// <summary xml:lang = "ru">
/// Сообщение о регистрации игры.
/// </summary>
internal class RegistrationMessage : IRegistrationMessage
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="RegistrationMessage"/>.
    /// </summary>
    /// <param name="sessionId" xml:lang = "ru">
    /// Идентификатор игровой сессии.
    /// </param>
    /// <param name="playerId" xml:lang = "ru">
    /// Идентификатор, который присвоили игроку.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из параметров равен <see langword="null"/>.
    /// </exception>
    public RegistrationMessage(GameSessionId sessionId, int? playerId)
    {
        SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
        PlayerId = playerId ?? throw new ArgumentNullException(nameof(playerId));
    }

    /// <inheritdoc/>
    public GameSessionId SessionId { get; }

    /// <inheritdoc/>
    public int PlayerId { get; }
}