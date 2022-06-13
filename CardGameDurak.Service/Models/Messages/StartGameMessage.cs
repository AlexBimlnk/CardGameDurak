using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Service.Models.Messages;

public class StartGameMessage : IRegistrationMessage
{
    public StartGameMessage(GameSessionId sessionId, int playerId)
    {
        SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
        PlayerId = playerId;
    }

    /// <inheritdoc/>
    public GameSessionId SessionId { get; }

    /// <inheritdoc/>
    public int PlayerId { get; }
}