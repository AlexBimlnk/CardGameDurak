using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLogic
{
    /// <summary>
    /// Представляет сущность Id для сессии.
    /// </summary>
    public sealed class SessionId
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id"> Id сессии </param>
        public SessionId(long id) => Value = Value;

        /// <summary>
        /// Возвращает Id сессии.
        /// </summary>
        public int Value { get; }
    }
}
