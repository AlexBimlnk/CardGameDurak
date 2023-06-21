namespace Logic;

/// <summary>
/// Название сущности.
/// </summary>
public sealed record class Name
{
    /// <summary>
    /// Создает новый экземпляр типа <see cref="Name"/>.
    /// </summary>
    /// <param name="name">
    /// Имя.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="name"/> был пустым или состоял только из пробельных символов.
    /// </exception>
    public Name(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException(
                "Name can't be null, empty or has only whitespaces",
                nameof(name));

        Value = name;
    }

    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; }
}