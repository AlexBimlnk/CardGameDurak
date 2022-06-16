using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Network.Messages;

/// <summary xml:lang = "ru">
/// Сообщение о состоянии игры.
/// </summary>
/// <typeparam name="TValue" xml:lang = "ru">
/// Тип значения сообщения прикрепленного к игровой сессии.
/// </typeparam>
/// <typeparam name="TSender" xml:lang = "ru">
/// Тип отправителя сообщения.
/// </typeparam>
public sealed class SessionMessage<TValue, TSender> 
    : IMessage<Tuple<IGameSession, TValue>, TSender>
    where TValue : class
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
    public SessionMessage(Tuple<IGameSession, TValue> value, TSender sender)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
        ArgumentNullException.ThrowIfNull(Value.Item2, nameof(Value.Item2));
    }


    /// <summary xml:lang = "ru">
    /// Тело сообщение содержит кортеж из двух элементов - 
    /// игровой сессии типа <see cref="IGameSession"/> и
    /// приклепленное к ней значение типа <typeparamref name="TValue"/>.
    /// </summary>
    public Tuple<IGameSession, TValue> Value { get; }

    /// <inheritdoc/>
    public TSender Sender { get; }
}
