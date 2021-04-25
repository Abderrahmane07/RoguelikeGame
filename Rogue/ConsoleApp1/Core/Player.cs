﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Core
{
    class Player: Actor 
    {
        public Player()
        {
            Awareness = 15;
            Name = "Rogue";
            Color = Colors.Player;
            Symbol = '@';
            X = 10;
            Y = 10;
        }
    }
}
