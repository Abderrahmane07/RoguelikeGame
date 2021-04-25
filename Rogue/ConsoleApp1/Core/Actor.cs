using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using ConsoleApp1.Interfaces;

namespace ConsoleApp1.Core
{
    class Actor: IActor, IDrawable
    {
        // IActor
        public string Name { get; set; }
        public int Awareness { get; set; }

        //IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Draw(RLConsole console, IMap map)
        {
            // Ne pas dessiner dans les cellules qui ne sont pas découvertes encore
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            // Dessiner seulement l'acteur si il est dans le champ de vision
            if (map.IsInFov(X, Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                // Quand c'est hors le champ de vision
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
            }
        }

    }
}
