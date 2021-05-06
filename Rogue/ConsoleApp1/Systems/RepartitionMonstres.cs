using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Monsters;
using RogueSharp.DiceNotation;
using ConsoleApp1.Core;


namespace ConsoleApp1.Systems
{
    public class RepartitionMonstres
    {
        static int repContamineMasque = 100;
        static int repContamineSansMasque = 0;
        static int repEscroc = 0;
        static int repPolice = 0;

        public static void Repartition(int level)
        {
            switch (level)
            {
                case 2:
                    repContamineMasque = 80;
                    repContamineSansMasque = 0;
                    repEscroc = 20;
                    repPolice = 0;
                    break;
                case 3:
                    repContamineMasque = 60;
                    repContamineSansMasque = 20;
                    repEscroc = 20;
                    repPolice = 0;
                    break;
                case 4:
                    repContamineMasque = 40;
                    repContamineSansMasque = 30;
                    repEscroc = 20;
                    repPolice = 10;
                    break;
                case 5:
                    repContamineMasque = 30;
                    repContamineSansMasque = 40;
                    repEscroc = 20;
                    repPolice = 10;
                    break;
                case 6:
                    repContamineMasque = 20;
                    repContamineSansMasque = 40;
                    repEscroc = 20;
                    repPolice = 20;
                    break;
                case 7:
                    repContamineMasque = 20;
                    repContamineSansMasque = 35;
                    repEscroc = 20;
                    repPolice = 25;
                    break;
                case 8:
                    repContamineMasque = 20;
                    repContamineSansMasque = 30;
                    repEscroc = 20;
                    repPolice = 30;
                    break;
                case 9:
                    repContamineMasque = 0;
                    repContamineSansMasque = 50;
                    repEscroc = 20;
                    repPolice = 30;
                    break;
                default:
                    repContamineMasque = 100;
                    repContamineSansMasque = 0;
                    repEscroc = 0;
                    repPolice = 0;
                    break;
            }
        }

        public static Monster LesMonstres(int level)
        {
            int de = Dice.Roll("1D100");
            Repartition(level);
            int l1 = repContamineMasque;
            int l2 = l1 + repEscroc;
            int l3 = l2 + repContamineMasque;
            if (0 < de && de < l1)
            {
                return ContamineMasque.Create(level);
            }
            else if (l1 < de && de < l2)
            {
                return Escroc.Create(level);
            }
            else if (l2 < de && de < l3)
            {
                return ContamineSansMasque.Create(level);
            }
            else
            {
                return Police.Create(level);
            }
        }
    }
}
