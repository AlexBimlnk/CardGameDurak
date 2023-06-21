﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Events;

/// <summary>
/// Описывает типизированное игровое событие.
/// </summary>
/// <typeparam name="TGame">
/// Тип игры.
/// </typeparam>
public interface IGameEvent<TGame> : IGameEvent
    where TGame : IGameType
{
}
