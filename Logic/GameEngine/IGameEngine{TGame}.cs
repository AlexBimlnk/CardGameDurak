using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logic.Events;

namespace Logic.GameCore;
public interface IGameEngine<TGame>
{
    /// <summary>
    /// Обрабатывает игровое событие.
    /// </summary>
    /// <param name="gameEvent">
    /// Игровое событие.
    /// </param>
    /// <param name="token">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// <see cref="Task"/>.
    /// </returns>
    public Task ProcessEvent(IGameEvent gameEvent, CancellationToken token);
}
