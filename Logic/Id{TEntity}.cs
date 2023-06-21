namespace Logic;

/// <summary>
/// Идентификатор сущности.
/// </summary>
/// <typeparam name="TEntity">
/// Тип конкретной сущности, для которой существует идентификатор.
/// </typeparam>
/// <param name="Value">
/// Значение идентификатора.
/// </param>
public readonly record struct Id<TEntity>(long Value);