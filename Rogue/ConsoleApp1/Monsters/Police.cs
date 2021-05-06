using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Core;
using RogueSharp.DiceNotation;

namespace ConsoleApp1.Monsters
{
    public class Police: Monster
    {
        public static Police Create(int level)
        {
            int health = Dice.Roll("2D6");
            return new Police
            {
                Attack = Dice.Roll("2D3") + level / 3,
                AttackChance = Dice.Roll("25D3"),
                Vol = Dice.Roll("7D5"),
                Awareness = 15,
                Color = Colors.Police,
                Defense = Dice.Roll("1D3") + level / 3,
                DefenseChance = Dice.Roll("5D13"),
                Gold = 0,
                Health = health,
                MaxHealth = health,
                Name = "Police",
                Speed = 11,
                Symbol = 'P'
            };

        }
    }
}
