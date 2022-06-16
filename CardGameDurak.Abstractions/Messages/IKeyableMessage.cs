namespace CardGameDurak.Abstractions.Messages;

/// <summary xml:lang = "ru">
/// Сообщение с идентификатором.
/// </summary>
/// <typeparam name="TKey" xml:lang = "ru">
/// Тип идентификатора сообщения.
/// </typeparam>
/// <typeparam name="TValue" xml:lang = "ru">
/// Тип тела сообщения.
/// </typeparam>
/// <typeparam name="TSender" xml:lang = "ru">
/// Тип отправителя сообщения.
/// </typeparam>
public interface IKeyableMessage<TKey, TValue, TSender> : IMessage<TValue, TSender> 
    where TKey : struct
{
    /// <summary xml:lang = "ru">
    /// Ключ сообщения типа <typeparamref name="TKey"/>.
    /// </summary>
    public TKey Key { get; }
}
