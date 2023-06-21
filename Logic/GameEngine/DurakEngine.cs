using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logic.Events.GameEvents;
using Logic.GameTypes;

namespace Logic.GameEngine;


public sealed class DurakEngine : IGameEngine<DurakGame>
{
    /// <inheritdoc/>
    public Task ProcessEvent(IGameEvent<DurakGame> gameEvent, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(gameEvent);

        token.ThrowIfCancellationRequested();

        return Task.CompletedTask;
    }
}
