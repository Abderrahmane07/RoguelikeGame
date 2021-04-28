using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Core;
using RogueSharp.DiceNotation;

namespace ConsoleApp1.Monsters
{
    class Kobold: Monster
    {
        public static Kobold Create(int level)
        {
            int health = Dice.Roll("2D5");
            return new Kobold
            {
                Attack = Dice.Roll("1D3") + level / 3,
                AttackChance = Dice.Roll("25D3"),
                Awareness = 10,
                Color = Colors.KoboldColor,
                Defense = Dice.Roll("1D3") + level / 3,
                DefenseChance = Dice.Roll("10D4"),
                Gold = Dice.Roll("5D5"),
                Health = health,
                Name = "kobold",
                Speed = 14,
                Symbol = 'k'
            };
            
        }
    }
}
