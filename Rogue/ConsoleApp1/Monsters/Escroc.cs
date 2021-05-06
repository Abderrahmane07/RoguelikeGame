using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Core;
using RogueSharp.DiceNotation;

namespace ConsoleApp1.Monsters
{
    public class Escroc: Monster
    {
        public static Escroc Create(int level)
        {
            int health = Dice.Roll("2D4");
            return new Escroc
            {
                Attack = 0,
                AttackChance = 0,
                Vol = Dice.Roll("5D6"),
                Awareness = 12,
                Color = Colors.Escroc,
                Defense = Dice.Roll("1D3") + level / 3,
                DefenseChance = Dice.Roll("3D4"),
                Gold = 0,
                Health = health,
                MaxHealth = health,
                Name = "Escroc",
                Speed = 12,
                Symbol = 'v'
            };

        }
    }
}
