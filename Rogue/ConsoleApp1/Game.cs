using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace ConsoleApp1
{
    class Game
    {
        // On configure la hauteur et la largeur de l'écran qui apparait
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        // La map, qui prend la majorité de l'écran
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        // Au dessous de la map, la sous console qui affiche les messages et différentes infos
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;


        // La zone de statistiques à droite de la map 
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        // Au dessus de la map, l'inventaire
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        static void Main(string[] args)
        {
            // Ca doit porter exactement le même nom que l'image qu'on importe
            string fontFileName = "terminal8x8.png";
            // Le titre de notre console
            string consoleTitle = "Jeu du roguelike - Niveau 1";
            // On informe RLNet que chaque tile est 8x8 pixels et qu'il faut utiliser l'image bitmap qu'on a précisé
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            // Initialisation des nouvelles sous-consoles qui vont apparaitre
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;
            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;
            // Begin RLNET's game loop
            _rootConsole.Run();
        }

        // Event handler for RLNET's Update event 
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            // On colorie et écrit un petit massage pour chaque sous-console
            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, RLColor.Black);
            _mapConsole.Print(1, 1, "Map", RLColor.White);

            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, RLColor.Gray);
            _messageConsole.Print(1, 1, "Messages", RLColor.White);

            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, RLColor.Green);
            _statConsole.Print(1, 1, "Stats", RLColor.White);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, RLColor.Cyan);
            _inventoryConsole.Print(1, 1, "Inventory", RLColor.White);
        }

        // Event handler for RLNET's Render event 
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // On 'blit' les sous-consoles
            RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
            RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
            RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
            RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

            // Tell RLNET to draw the console that we set 
            _rootConsole.Draw();
        }
    }
}
