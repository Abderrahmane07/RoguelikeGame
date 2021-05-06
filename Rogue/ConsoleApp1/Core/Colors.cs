using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;


namespace ConsoleApp1.Core
{
    public static class Colors
    {

        public static RLColor FloorBackground = RLColor.Black;
        public static RLColor Floor = Swatch.AlternateDarkest;
        public static RLColor FloorBackgroundFov = Swatch.DbDark; // Fov = Field of view
        public static RLColor FloorFov = Swatch.Alternate;

        public static RLColor WallBackground = Swatch.SecondaryDarkest;
        public static RLColor Wall = Swatch.Secondary;
        public static RLColor WallBackgroundFov = Swatch.SecondaryDarker;
        public static RLColor WallFov = Swatch.SecondaryLighter;

        public static RLColor TextHeading = RLColor.White;
        public static RLColor Text = Swatch.DbLight;
        public static RLColor Gold = Swatch.DbSun;

        public static RLColor DoorBackground = Swatch.ComplimentDarkest;
        public static RLColor Door = Swatch.ComplimentLighter;
        public static RLColor DoorBackgroundFov = Swatch.ComplimentDarker;
        public static RLColor DoorFov = Swatch.ComplimentLightest;

        public static RLColor Player = Swatch.DbLight;

        public static RLColor ContamineMasque = Swatch.SangClair;
        public static RLColor ContamineSansMasque = Swatch.Sang;
        public static RLColor Escroc = new RLColor(255, 165, 0);
        public static RLColor Police = new RLColor(51, 160, 255);

        public static RLColor JourneeLightest = new RLColor(255, 255, 160);
        public static RLColor JourneeLighter = new RLColor(249, 249, 114);
        public static RLColor Journee = new RLColor(227, 227, 75);
        public static RLColor JourneeDarker = new RLColor(219, 219, 39);
        public static RLColor JourneeDarkest = new RLColor(187, 187, 17);

        public static RLColor Journee2 = new RLColor(191, 191, 52);
        public static RLColor JourneeCdV = new RLColor(227, 227, 75);


        
        public static RLColor Soir = new RLColor(1, 68, 97);
        public static RLColor SoirCdV = new RLColor(9, 133, 186);
        
        public static RLColor Nuit = new RLColor(51, 45, 65);
        public static RLColor NuitCdV = new RLColor(115, 101, 148);
    }
}
