namespace CardGameDurak.Abstractions.Messages;

/// <summary xml:lang = "ru">
/// Сообщение.
/// </summary>
/// <typeparam name="TValue" xml:lang = "ru">
/// Тип тела сообщение типа.
/// </typeparam>
/// <typeparam name="TSender" xml:lang = "ru">
/// Тип отправителя сообщения.
/// </typeparam>
public interface IMessage<TValue, TSender>
    where TSender : ISender
{
    /// <summary xml:lang = "ru">
    /// Тело сообщение типа <typeparamref name="TValue"/>.
    /// </summary>
    public TValue Value { get; }

    /// <summary xml:lang = "ru">
    /// Отправитель сообщения типа <typeparamref name="TSender"/>.
    /// </summary>
    public TSender Sender { get; }
}
