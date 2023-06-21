using CardGameDurak.Abstractions.GameSession;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Network.Messages;

/// <summary xml:lang = "ru">
/// Сообщение о состоянии игры.
/// </summary>
/// <typeparam name="TLinkedValue" xml:lang = "ru">
/// Тип значения сообщения прикрепленного к игровой сессии.
/// </typeparam>
/// <typeparam name="TSender" xml:lang = "ru">
/// Тип отправителя сообщения.
/// </typeparam>
public sealed class SessionMessage<TLinkedValue, TSender>
    : IMessage<ISessionState<TLinkedValue>, TSender>
    where TLinkedValue : class
    where TSender : ISender
{
    /// <summary>
    /// Создает экземпляр типа <see cref="SessionMessage{TValue, TSender}"/>.
    /// </summary>
    /// <param name="value" xml:lang = "ru"> 
    /// Тело сообщения. 
    /// </param>
    /// <param name="sender" xml:lang = "ru"> 
    /// Отправитель сообщения. 
    /// </param>
    /// <exception cref="ArgumentNullException"> 
    /// Если один из входных параметров равен <see langword="null"/>. 
    /// </exception>
    public SessionMessage(ISessionState<TLinkedValue> value, TSender sender)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }


    /// <summary xml:lang = "ru">
    /// Состояние игровой сессией с прикре приклепленному
    /// к ней значение типа <typeparamref name="TLinkedValue"/>.
    /// </summary>
    public ISessionState<TLinkedValue> Value { get; }

    /// <inheritdoc/>
    public TSender Sender { get; }
}
