using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;
using CardGameDurak.Abstractions.Messages;

namespace CardGameDurak.Network.Messages;

/// <summary xml:lang = "ru">
/// Сообщение, требующее обновление.
/// </summary>
/// <typeparam name="TValue" xml:lang = "ru">
/// Тип значения которое нужно обновить.
/// </typeparam>
/// <typeparam name="TSender" xml:lang = "ru">
/// Тип отправителя сообщения.
/// </typeparam>
public sealed class UpdateMessage<TValue, TSender>
    : IKeyableMessage<GameSessionId, TValue, TSender>
    where TSender : IPlayer, ISender
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="UpdateMessage{TValue, TSender}"/>.
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
    public UpdateMessage(GameSessionId key, TValue value, TSender sender)
    {
        Key = key;
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    /// <inheritdoc/>
    public GameSessionId Key { get; }

    /// <summary xml:lang = "ru">
    /// Тело сообщение содержит кортеж из двух элементов - 
    /// игрового события <see cref="PlayerEvent"/> и
    /// приклепленное к нему значение типа <typeparamref name="TValue"/>.
    /// </summary>
    public TValue Value { get; }

    /// <summary xml:lang = "ru">
    /// Отправитель сообщения типа <typeparamref name="TSender"/>.
    /// </summary>
    public TSender Sender { get; }
}