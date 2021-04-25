using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;

namespace ConsoleApp1.Core
{
    // Extension de la classe originale de RogueSharp
    class DungeonMap : Map
    {
        // Methode appelee a chaque mouvement du joueur pour mettre a jour son champ de vision
        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            // Calcul du champ de vision a la base du position du joueur
            ComputeFov(player.X, player.Y, player.Awareness, true);
            // Modifier les cellules dans le champ de vision en cellules explorees
            foreach (Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        // La méthode Draw va être appellée à chaque modification pour donner les symboles et les couleurs à chaque cellule
        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();
            foreach(Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }

        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
            // Si la cellule n'est pas explorée, ne rien dessiner
            if(!cell.IsExplored)
            {
                return;
            }
            // Si la cellule est dans le champ de vision
            if(IsInFov(cell.X, cell.Y))
            {
                // Choix du symbole selon qu'on peut marcher ou non sur cette cellule
                // '.' pour la terre et '#' pour les murs
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            // Si la cellule est hors le champ de vision
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }
        }
    }
}
