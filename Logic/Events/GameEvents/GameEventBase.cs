using System.ComponentModel;

namespace Logic.Events.GameEvents;

/// <summary>
/// Базовый класс для игровых событий.
/// </summary>
public abstract class GameEventBase : IGameEvent
{
    /// <summary>
    /// Создает новый объект типа <see cref="GameEventBase"/>.
    /// </summary>
    /// <param name="id">
    /// Идентификатор игры.
    /// </param>
    /// <param name="type">
    /// Тип игрового события.
    /// </param>
    /// <exception cref="InvalidEnumArgumentException">
    /// Если <paramref name="type"/> был неизвестного типа.
    /// </exception>
    protected GameEventBase(GameId id, GameEventType type)
    {
        GameId = id;

        if (!Enum.IsDefined(type))
            throw new InvalidEnumArgumentException();
        Type = type;
    }

    /// <inheritdoc/>
    public GameId GameId { get; }

    /// <inheritdoc/>
    public GameEventType Type { get; }
}
