using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Systems;
using ConsoleApp1.Behaviors;
using RLNET;
using RogueSharp;

namespace ConsoleApp1.Core
{
    public class Monster : Actor
    {
        public int? TurnsAlerted { get; set; }

        public virtual void PerformAction(CommandSystem commandSystem)
        {
            var behavior = new StandardMoveAndAttack();
            behavior.Act(this, commandSystem);
        }
        public void DrawStats(RLConsole statConsole, int position)
        {
            // Commencer a l'ordonnee y = 13 qui est le point ou se termine les stats du joueur
            // Doubler par 2 pour laisser de l'espace
            int yPosition = 13 + (position * 2);

            // Commencer la ligne par dessiner le symbol du monstre dans la bonne couleur 
            statConsole.Print(1, yPosition, Symbol.ToString(), Color);

            // On commence par determiner la largeur de cette bande sante en effectuant la division de health sur maxHealth
            int width = Convert.ToInt32(((double)Health / (double)MaxHealth) * 16.0);
            int remainingWidth = 16 - width;

            // Chosir la couleur de cette barre de santé pour le monstre 
            statConsole.SetBackColor(3, yPosition, width, 1, Swatch.Primary);
            statConsole.SetBackColor(3 + width, yPosition, remainingWidth, 1, Swatch.PrimaryDarkest);

            // Ecrire le nom du monstre au dessus de cette barre
            statConsole.Print(2, yPosition, $": {Name}", Swatch.DbLight);
        }
    }
}
