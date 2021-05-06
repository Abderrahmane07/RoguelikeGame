using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Core;
using RogueSharp.DiceNotation;

namespace ConsoleApp1.Monsters
{
    public class ContamineSansMasque: Monster
    {
        public static ContamineSansMasque Create(int level)
        {
            int health = Dice.Roll("2D6");
            return new ContamineSansMasque
            {
                Attack = Dice.Roll("2D3") + level / 3,
                AttackChance = Dice.Roll("25D3"),
                Vol = 0,
                Awareness = 12,
                Color = Colors.ContamineSansMasque,
                Defense = Dice.Roll("1D3") + level / 3,
                DefenseChance = Dice.Roll("5D10"),
                Gold = Dice.Roll("5D7"),
                Health = health,
                MaxHealth = health,
                Name = "Contaminé sans Masque",
                Speed = 12,
                Symbol = 'C'
            };

        }

    }
}
