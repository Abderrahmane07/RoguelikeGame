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
        private readonly List<Monster> _monsters;

        public DungeonMap()
        {
            // Initialisation de la liste des rooms et de monstres quand on cree un nouveau donjon
            Rooms = new List<Rectangle>();
            _monsters = new List<Monster>();
        }

        // Methode pour ajouter le  monstre 
        public void AddMonster(Monster monster)
        {
            _monsters.Add(monster);
            // On rend la cellule unwalkable apres ajouter le monstre
            SetIsWalkable(monster.X, monster.Y, false);
        }

        // Cherche une cellule aleatoire dans la room qui est walkable
        public Point? GetRandomWalkableLocationInRoom(Rectangle room)
        {
            if (DoesRoomHaveWalkableSpace(room))
            {
                for(int i=0; i<100; i++)
                {
                    int x = Game.Random.Next(1, room.Width - 2) + room.X;
                    int y = Game.Random.Next(1, room.Height - 2) + room.Y;
                    if (IsWalkable(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }

            // Si on trouve pas de cellules convenable
            return null;
        }

        // Cette methode traverse tote les cellules de la room et retourne true s'il y a des cellules walkable
        public bool DoesRoomHaveWalkableSpace(Rectangle room)
        {
            for (int x=1; x <= room.Width - 2; x++)
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (IsWalkable(x+room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
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
        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            foreach(Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }

            // Garder un index pour savoir ou dessiner les stats du monstre
            int i = 0;

            // On passe par tous les mostres dans la list et on les dessine
            foreach(Monster monster in _monsters)
            {
                monster.Draw(mapConsole, this);
                // Quand le monstre est aussi dans le champ de vision dessiner ses stats
                if(IsInFov(monster.X, monster.Y))
                {
                    // Faire appel a la fonction pour dessiner les stats et incrementer l'index apress
                    monster.DrawStats(statConsole, i);
                    i++;
                }
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
