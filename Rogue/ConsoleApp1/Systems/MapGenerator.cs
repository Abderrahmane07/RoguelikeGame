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

        private readonly DungeonMap _map;

        // Construire un nouveau MapGenerator requiert les dimensions de cette map
        public MapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }

        // Commencons par générer une map simple avec un sol et un mur qui l'entoure
        public DungeonMap CreateMap()
        {
            // Initisalisation de chaque cellule sur la map avec walkable, transparence et expoler en true 
            _map.Initialize(_width, _height);
            foreach(Cell cell in _map.GetAllCells())
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }

            // Mettre la première et dernière ligne en non tranparent et non walkable
            foreach (Cell cell in _map.GetCellsInRows(0, _height - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            // Mettre la première et dernière colonne en non tranparent et non walkable
            foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            return _map;
        }
    }
}
