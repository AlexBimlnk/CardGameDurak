using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Network.Messages;

/// <summary xml:lang = "ru">
/// Сообщение о игровом событии.
/// </summary>
/// <typeparam name="TValue" xml:lang = "ru">
/// Тип значения сообщения прикрепленного к игровому событию.
/// </typeparam>
/// <typeparam name="TSender" xml:lang = "ru">
/// Тип отправителя сообщения.
/// </typeparam>
public sealed class EventMessage<TValue, TSender>
    : IKeyableMessage<GameSessionId, Tuple<GameEvent, TValue>, TSender>
    where TSender : IPlayer, ISender
    where TValue : class
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="EventMessage"/>.
    /// </summary>
    /// <param name="key" xml:lang = "ru">
    /// Идентификатор игровой сессии.
    /// </param>
    /// <param name="value" xml:lang = "ru">
    /// Тело сообщения.
    /// </param>
    /// <param name="sender" xml:lang = "ru">
    /// Отправитель сообщения.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если любой из входных параметров равен <see langword="null"/>.
    /// </exception>
    public EventMessage(GameSessionId key, Tuple<GameEvent, TValue> value, TSender sender)
    {
        Key = key;
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
        ArgumentNullException.ThrowIfNull(Value.Item2, nameof(Value.Item2));
    }

    /// <inheritdoc/>
    public GameSessionId Key { get; }

    /// <summary xml:lang = "ru">
    /// Тело сообщение содержит кортеж из двух элементов - 
    /// игрового события <see cref="GameEvent"/> и
    /// приклепленное к нему значение типа <typeparamref name="TValue"/>.
    /// </summary>
    public Tuple<GameEvent, TValue> Value { get; }

    /// <summary xml:lang = "ru">
    /// Отправитель сообщения типа <typeparamref name="TSender"/>.
    /// </summary>
    public TSender Sender { get; }
}
