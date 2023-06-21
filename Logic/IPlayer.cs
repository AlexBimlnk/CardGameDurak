using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic;

/// <summary>
/// Описывает игрока.
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public PlayerId Id { get; }

    /// <summary>
    /// Имя.
    /// </summary>
    public Name Name { get; }
}
