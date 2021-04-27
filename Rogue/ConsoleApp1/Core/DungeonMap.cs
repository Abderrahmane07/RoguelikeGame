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
        public List<Rectangle> Rooms;

        public DungeonMap()
        {
            // Initialisation de la liste des rooms quand on cree un nouveau donjon
            Rooms = new List<Rectangle>();
        }
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


        // Methode appelee par MapGenerator lors de la creation d'une nouvelle map pour positionner le joueur là-bas
        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
        }
        // Retourne true si on est capable de poser le joueur sur la cellule qu'on veut et false sinon
        public bool SetActorPosition(Actor actor, int x, int y)
        {
            // Permet de placer l'acteur si la cellule est walkable
            if (GetCell(x, y).IsWalkable)
            {
                // La cellule ou etait le joueur est walkable maintenant
                SetIsWalkable(actor.X, actor.Y, true);
                // Mettre a jour la position de l'acteur 
                actor.X = x;
                actor.Y = y;
                // La nouvelle n'est plus walkable
                SetIsWalkable(actor.X, actor.Y, false);
                // Ne pas mettre a jour le champ de vision quand on deplace le joueur 
                if (actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

        // Methode pour aide
        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Cell cell = (Cell)GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
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
