﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLogic.Players
{
    public interface IPlayer
    {
        int CountCards { get; }
    }
}