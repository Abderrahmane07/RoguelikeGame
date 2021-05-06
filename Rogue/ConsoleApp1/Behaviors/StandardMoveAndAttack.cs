using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using ConsoleApp1.Systems;
using ConsoleApp1.Core;
using ConsoleApp1.Interfaces;

namespace ConsoleApp1.Behaviors
{
    public class StandardMoveAndAttack : IBehavior
    {
        public bool Act(Monster monster, CommandSystem commandSystem)
        {
            DungeonMap dungeonMap = Game.DungeonMap;
            Player player = Game.Player;
            FieldOfView monsterFov = new FieldOfView(dungeonMap);

            // Si le monstre n'est pas aletrte on calcul son champ de vison qui depend de Awareness 
            // et on verifie si le joueur est dans le champ du monstre et on l'alerte le cas echeant
            // par un message dans Message.Log
            if (!monster.TurnsAlerted.HasValue)
            {
                monsterFov.ComputeFov(monster.X, monster.Y, monster.Awareness, true);
                if (monsterFov.IsInFov(player.X, player.Y))
                {
                    Game.MessageLog.Add($"{monster.Name} is eager to fight {player.Name}");
                    monster.TurnsAlerted = 1;
                }
            }

            if (monster.TurnsAlerted.HasValue) // Pourquoi ne pas juste faire else
            {
                // Avant de trouver la trajectoire on rend d'abord les cellules du monster et du joueur walkable
                dungeonMap.SetIsWalkable(monster.X, monster.Y, true);
                dungeonMap.SetIsWalkable(player.X, player.Y, true);

                PathFinder pathFinder = new PathFinder(dungeonMap);
                Path path = null;

                try
                {
                    path = pathFinder.ShortestPath(dungeonMap.GetCell(monster.X, monster.Y), dungeonMap.GetCell(player.X, player.Y));
                }
                catch (PathNotFoundException)
                {
                    // Si le monstre voit le joueur mais est incapable de trouver une trajectoire vers lui potentiellement 
                    // parcque son chemin est bloqué, on écrit un message ou on le signale 
                    Game.MessageLog.Add($"{monster.Name} attend pour un tour");
                }

                // On met de retour les cellules en non walkable
                dungeonMap.SetIsWalkable(monster.X, monster.Y, false);
                dungeonMap.SetIsWalkable(player.X, player.Y, false);

                // Dans le cas ou il y a un chemin on informe CommadSystem pour bouger le monstre
                if (path != null)
                {
                    try
                    {
                        commandSystem.MoveMonster(monster, (Cell) path.StepForward());
                    }
                    catch(NoMoreStepsException)
                    {
                        Game.MessageLog.Add($"{monster.Name} attend impatiemment");
                    }
                }

                monster.TurnsAlerted++;

                // Perdre le statut d'alerte chaque 15 tours et tant que le joueur est dans le champ de vision du monstre 
                // le monstre est alerte dans le cas contraire le monstre va abandonner cette poursuite
                if (monster.TurnsAlerted > 15)
                {
                    monster.TurnsAlerted = null;
                }
            }
            return true;
        }
    }
}
