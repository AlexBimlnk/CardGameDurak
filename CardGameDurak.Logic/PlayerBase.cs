﻿using CardGameDurak.Abstractions;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Базовый класс для всех игроков.
/// </summary>
internal abstract class PlayerBase : IPlayer
{
    protected readonly List<ICard> _cards = new List<ICard>(6);

    public PlayerBase(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentNullException("Имя бота не соответствует требованиям!");
        else Name = name;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public int CountCards => _cards.Count;

    /// <inheritdoc/>
    public int? Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Принимает карты, которые ему выдают.
    /// </summary>
    /// <param name="cards" xml:lang = "ru">
    /// Список карт, который нужно добавить в руку.
    /// </param>
    public void ReceiveCards(params ICard[] cards) =>
        _cards.AddRange(cards ?? throw new ArgumentNullException(nameof(cards)));
}
