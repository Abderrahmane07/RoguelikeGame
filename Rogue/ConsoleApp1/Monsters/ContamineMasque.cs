using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Core;
using RogueSharp.DiceNotation;

namespace ConsoleApp1.Monsters
{
    public class ContamineMasque: Monster
    {
        public static ContamineMasque Create(int level)
        {
            int health = Dice.Roll("2D5");
            return new ContamineMasque
            {
                Attack = Dice.Roll("1D3") + level / 3,
                AttackChance = Dice.Roll("25D3"),
                Vol = 0,
                Awareness = 10,
                Color = Colors.ContamineMasque,
                Defense = Dice.Roll("1D3") + level / 3,
                DefenseChance = Dice.Roll("5D4"),
                Gold = Dice.Roll("5D5"),
                Health = health,
                MaxHealth=health,
                Name = "Contamine Masque",
                Speed = 14,
                Symbol = 'c'
            };
            
        }
    }
}
