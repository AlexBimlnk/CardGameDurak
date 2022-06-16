using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Network.Messages;

/// <summary xml:lang = "ru">
/// Сообщение о регистрации.
/// </summary>
/// <typeparam name="TValue" xml:lang = "ru">
/// Тип тела сообщения.
/// </typeparam>
/// <typeparam name="TSender" xml:lang = "ru">
/// Тип отправителя сообщения.
/// </typeparam>
public sealed class RegistrationMessage<TValue, TSender> : IMessage<TValue, TSender>
    where TValue : IGameSession
    where TSender : ISender
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="RegistrationMessage{TValue, TSender}"/>.
    /// </summary>
    /// <param name="value" xml:lang = "ru">
    /// Тело сообщения.
    /// </param>
    /// <param name="sender" xml:lang = "ru">
    /// Отправитель сообщения.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из параметров равен <see langword="null"/>.
    /// </exception>
    public RegistrationMessage(TValue value, TSender sender)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    /// <summary xml:lang = "ru">
    /// Тело сообщение типа <typeparamref name="TValue"/>.
    /// </summary>
    public TValue Value { get; }

    /// <summary xml:lang = "ru">
    /// Отправитель сообщения типа <typeparamref name="TSender"/>.
    /// </summary>
    public TSender Sender { get; }
}
