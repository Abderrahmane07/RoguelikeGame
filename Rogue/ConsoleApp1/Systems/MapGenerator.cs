using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Core;
using RogueSharp;
using ConsoleApp1.Monsters;
using RogueSharp.DiceNotation;

namespace ConsoleApp1.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _maxRooms;
        private readonly int _roomMaxSize;
        private readonly int _roomMinSize;


        private readonly DungeonMap _map;

        // Construire un nouveau MapGenerator requiert les dimensions de cette map
        public MapGenerator(int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, int mapLevel)
        {
            _width = width;
            _height = height;
            _maxRooms = maxRooms;
            _roomMaxSize = roomMaxSize;
            _roomMinSize = roomMinSize;
            _map = new DungeonMap();
        }

        // [Commencons par générer une map simple avec un sol et un mur qui l'entoure] Maintenant une nouvelle map avec des rooms places aleatoirement
        public DungeonMap CreateMap()
        {
            // Initisalisation de chaque cellule sur la map avec walkable, transparence et expoler en true 
            _map.Initialize(_width, _height);
            // Essayer de placer le max de rooms que specifier dans maxRooms
            for (int r = _maxRooms; r > 0; r--)
            {
                // Determination aleatoire de la taille et de la position des rooms
                int roomWidth = Game.Random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = Game.Random.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = Game.Random.Next(0, _width - roomWidth - 1);
                int roomYPosition = Game.Random.Next(0, _height - roomHeight - 1);

                // Toutes nos rooms peuvent etre presentees en Rectangles
                var newRoom = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

                // S'assurer que notre room n'intersecte pas avec une autre
                bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                // S'il n'y a pas intersection, l'ajouter
                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);
                }

                // La partie pour creer les connections entre les differentes rooms
                // Iterer a travers chaque room generee, en commencant par la room 1 au lieu de 0
                for (int rok = 1; rok < _map.Rooms.Count; rok++)
                {
                    // commecons par trouver le centre de la room r et de celle qui la precede
                    int previousRoomCenterX = _map.Rooms[rok - 1].Center.X;
                    int previousRoomCenterY = _map.Rooms[rok - 1].Center.Y;
                    int currentRoomCenterX = _map.Rooms[rok].Center.X;
                    int currentRoomCenterY = _map.Rooms[rok].Center.Y;

                    // 50% de chance de choisir un des deux choix
                    if (Game.Random.Next(1, 2) == 1)
                    {
                        CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                        CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    }
                    else
                    {
                        CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                        CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    }

                }
            }
            // Passer par chaque chambre et voir si on va creer des portes
            foreach (Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
                CreateDoors(room);
            }

            CreateStairs();

            PlacePlayer();

            PlaceMonsters();

            return _map;
        }

        // Tunnel selon l'axe x
        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                _map.SetCellProperties(x, yPosition, true, true);
            }
        }

        // Tunnel selon l'axe y
        private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                _map.SetCellProperties(xPosition, y, true, true);
            }
        }

        // Cette fonction trouve le centre de la première room et met le joueur la bas
        private void PlacePlayer()
        {
            Player player = Game.Player;
            if (player == null)
            {
                player = new Player();
            }
            player.X = _map.Rooms[0].Center.X;
            player.Y = _map.Rooms[0].Center.Y;

            _map.AddPlayer(player);
        }

        // En ayant une zone du map rectangulaire mettre toutes les proprietes des cellules pour cette zone en true
        private void CreateRoom(Rectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, false);
                }
            }
        }

        // Pour creer les portes
        private void CreateDoors(Rectangle room)
        {
            // Le contour de la chambre
            int xMin = room.Left;
            int xMax = room.Right;
            int yMin = room.Top;
            int yMax = room.Bottom;


            // On depose ces coordonnes de cette chambre dans une liste
            List<ICell> borderCells = _map.GetCellsAlongLine(xMin, yMin, xMax, yMin).ToList();
            borderCells.AddRange(_map.GetCellsAlongLine(xMin, yMin, xMin, yMax));
            borderCells.AddRange(_map.GetCellsAlongLine(xMin, yMax, xMax, yMax));
            borderCells.AddRange(_map.GetCellsAlongLine(xMax, yMin, xMax, yMax));

            // Parcourir chacune des cellules du contour et chercher ou placer les portes
            foreach (ICell cell in borderCells)
            {
                if (IsPotentialDoor(cell))
                {
                    // La porte dooit empecher le champ de vision quand elle est fermee 
                    _map.SetCellProperties(cell.X, cell.Y, false, true);
                    _map.Doors.Add(new Door
                    {
                        X = cell.X,
                        Y = cell.Y,
                        IsOpen = false
                    });
                }
            }
        }

        // Evalue si la cellule est bonne pour une porte
        private bool IsPotentialDoor(ICell cell)
        {
            // Si la cellule est non walkable alors c'est un mur et donc c'est pas une bonne place a mettre la porte
            if (!cell.IsWalkable)
            {
                return false;
            }

            // Conserve les coordonnees de toutes les cellules qui l'entoure
            ICell right = _map.GetCell(cell.X + 1, cell.Y);
            ICell left = _map.GetCell(cell.X - 1, cell.Y);
            ICell top = _map.GetCell(cell.X, cell.Y - 1);
            ICell bottom = _map.GetCell(cell.X, cell.Y + 1);

            // S'assurer qu'il n' y a pas deja une porte
            if (_map.GetDoor(cell.X, cell.Y) != null ||
                _map.GetDoor(right.X, right.Y) != null ||
                _map.GetDoor(left.X, left.Y) != null ||
                _map.GetDoor(top.X, top.Y) != null ||
                _map.GetDoor(bottom.X, bottom.Y) != null)
            {
                return false;
            }

            // Ceci est une bonne place pour mettre une porte dans le cote droite ou gauche de la chambre
            if (right.IsWalkable && left.IsWalkable && !top.IsWalkable && !bottom.IsWalkable)
            {
                return true;
            }
            // Ceci est une bonne place pour mettre une porte dans le cote haut ou bat de la chambre
            if (top.IsWalkable && bottom.IsWalkable && !right.IsWalkable && !left.IsWalkable)
            {
                return true;
            }

            return false;
        }

        // Pour placer les monstres
        private void PlaceMonsters()
        {
            foreach (var room in _map.Rooms)
            {
                // Chaque room a 60% de chance de contenir des monstres
                if (Dice.Roll("1D10") < 7)
                {
                    // Generer de 1 a 4 monstres 
                    var numberOfMonsters = Dice.Roll("1D4");
                    for (int i = 0; i < numberOfMonsters; i++)
                    {
                        //Trouver ou mettre le monstre
                        Point randomRoomLocation = (Point)_map.GetRandomWalkableLocationInRoom(room); // Errue apparaissait ici et pour solution on forcait la nature avec (Point), source d'erreur potentielle. A revoir !
                        // Si ce n'est pas possible on saute 
                        if (randomRoomLocation != null)
                        {
                            // ? Temporarily hard code this monster to be created at level 1 ?
                            var monster = Kobold.Create(1);
                            monster.X = randomRoomLocation.X;
                            monster.Y = randomRoomLocation.Y;
                            _map.AddMonster(monster);
                        }
                    }
                }
            }
        }

        // Pour traiter les echelles
        private void CreateStairs()
        {
            _map.StairsUp = new Stairs
            {
                X = _map.Rooms.First().Center.X + 1,
                Y = _map.Rooms.First().Center.Y,
                IsUp = true
            };
            _map.StairsDown = new Stairs
            {
                X = _map.Rooms.Last().Center.X,
                Y = _map.Rooms.Last().Center.Y,
                IsUp = false
            };
        }
    }
}
