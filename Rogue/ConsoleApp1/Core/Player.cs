using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace ConsoleApp1.Core
{
    public class Player: Actor 
    {
        public Player()
        {
            Attack = 2;
            AttackChance = 50;
            Awareness = 15;
            Color = Colors.Player;
            Defense = 2;
            DefenseChance = 40;
            Gold = 0;
            Health = 100;
            MaxHealth = 100;
            Name = "Rogue";
            Speed = 10;
            Symbol = '@';
        }

        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print(1, 1, $"Nom:     {Name}", Colors.Text);
            statConsole.Print(1, 3, $"Immunite:   {Health}/{MaxHealth}", Colors.Text);
            statConsole.Print(1, 5, $"Attaque:   {Attack} ({AttackChance}%)", Colors.Text);
            statConsole.Print(1, 7, $"Defence:  {Defense} ({DefenseChance}%)", Colors.Text);
            statConsole.Print(1, 9, $"Argent:     {Gold}", Colors.Gold);
        }


    }
}
