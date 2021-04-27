using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Core;
using RogueSharp;

namespace ConsoleApp1.Systems
{
    class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _maxRooms;
        private readonly int _roomMaxSize;
        private readonly int _roomMinSize;


        private readonly DungeonMap _map;

        // Construire un nouveau MapGenerator requiert les dimensions de cette map
        public MapGenerator(int width, int height, int maxRooms, int roomMaxSize, int roomMinSize)
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
            for (int r=_maxRooms; r>0; r--)
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
            }
            foreach(Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
            }

            PlacePlayer();

            return _map;
        }

        // Tunnel selon l'axe x
        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for (int x=Math.Min(xStart, xEnd); x<=Math.Max(xStart,xEnd); x++)
            {
                _map.SetCellProperties(x, yPosition, true, true);
            }
        }

        // Tunnel selon l'axe 
        private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                _map.SetCellProperties(y, xPosition, true, true);
            }
        }

        // Cette fonction trouve le centre de la première room et met le joueur la bas
        private void PlacePlayer()
        {
            Player player = Game.Player;
            if(player == null)
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
            for(int x=room.Left+1; x<room.Right; x++)
            {
                for(int y=room.Top+1; y<room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, true);
                }
            }
        }
    }
}
