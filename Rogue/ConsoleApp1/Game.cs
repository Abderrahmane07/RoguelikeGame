﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp.Random;
using ConsoleApp1.Core;
using ConsoleApp1.Systems;

namespace ConsoleApp1
{
    class Game
    {
        private static bool _renderRequired = true;

        public static CommandSystem CommandSystem { get; private set; } 
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

        private static int _mapLevel = 1;

        public static MessageLog MessageLog { get; private set; }
        public static Player Player { get; set; }
        public static DungeonMap DungeonMap { get; private set; }
        public static SchedulingSystem SchedulingSystem { get; set; }

        // Singleton of IRandom used throughout the game when generating random numbers
        public static IRandom Random { get; private set; } 
        static void Main(string[] args)
        {
            // Etablir la seed pour le generateur des nomres aleatoirs
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            SchedulingSystem = new SchedulingSystem();

            // The title will appear at the top of the console window 
            // also include the seed used to generate the level
            string consoleTitle = $"Jeu du RogueLike - Level {_mapLevel} - Seed {seed}";

            CommandSystem = new CommandSystem();
            // Ca doit porter exactement le même nom que l'image qu'on importe
            string fontFileName = "terminal8x8.png";
            // Le titre de notre console
            // On informe RLNet que chaque tile est 8x8 pixels et qu'il faut utiliser l'image bitmap qu'on a précisé
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            // Initialisation des nouvelles sous-consoles qui vont apparaitre
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7, _mapLevel);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();

            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;
            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;
            // Begin RLNET's game loop

            // L'ancien code pour la partie message
            //// On colorie et écrit un petit massage pour chaque sous-console
            //_messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
            //_messageConsole.Print(1, 1, "Messages", RLColor.White);

            // Le nouveau code pour message
            MessageLog = new MessageLog();
            MessageLog.Add("The rogue is in Level 1");
            MessageLog.Add($"Level created with seed '{seed}'");

            // L'ancien code pour la zone stats
            //_statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
            //_statConsole.Print(1, 1, "Stats", RLColor.White);

            // Nouveau code pour stats dans OnRootConsoleRender()        

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", RLColor.White);

            _rootConsole.Run();

            
        }

        // Event handler for RLNET's Update event 
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();
            if (CommandSystem.IsPlayerTurn)
            {
                if (keyPress != null)
                {
                    if (keyPress.Key == RLKey.Up)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                    }
                    else if (keyPress.Key == RLKey.Down)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                    }
                    else if (keyPress.Key == RLKey.Left)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                    }
                    else if (keyPress.Key == RLKey.Right)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                    }
                    else if (keyPress.Key == RLKey.Escape)
                    {
                        _rootConsole.Close();
                    }
                    else if (keyPress.Key == RLKey.Period)
                    {
                        if (DungeonMap.CanMoveDownToNextLevel())
                        {
                            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7, ++_mapLevel);
                            DungeonMap = mapGenerator.CreateMap();
                            MessageLog = new MessageLog();
                            CommandSystem = new CommandSystem();
                            _rootConsole.Title = $"Jeu du RogueLike - Level {_mapLevel}";
                            didPlayerAct = true;
                        }
                    }
                }

                if (didPlayerAct)
                {
                    _renderRequired = true;
                    CommandSystem.EndPlayerTurn();
                }
            }
            else
            {
                CommandSystem.ActivateMonsters();
                _renderRequired = true;
            }
        }

        // Event handler for RLNET's Render event 
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // Ne rien faire si aucun changement n'a ete porte
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statConsole.Clear();
                _messageConsole.Clear();

                DungeonMap.Draw(_mapConsole, _statConsole);
                Player.Draw(_mapConsole, DungeonMap);
                Player.DrawStats(_statConsole);
                MessageLog.Draw(_messageConsole);

                // On 'blit' les sous-consoles
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                // Tell RLNET to draw the console that we set 
                _rootConsole.Draw();
                _renderRequired = false;
            }
            
        }
    }
}
