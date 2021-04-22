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

        static void Main(string[] args)
        {
            // Ca doit porter exactement le même nom que l'image qu'on importe
            string fontFileName = "terminal8x8.png";
            // Le titre de notre console
            string consoleTitle = "Jeu du roguelike - Niveau 1";
            // On informe RLNet que chaque tile est 8x8 pixels et qu'il faut utiliser l'image bitmap qu'on a précisé
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);
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
            _rootConsole.Print(10, 10, "it worked!", RLColor.White);
        }

        // Event handler for RLNET's Render event 
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // Tell RLNET to draw the console that we set 
            _rootConsole.Draw();
        }
    }
}
