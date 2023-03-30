using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLab
{
    internal class Levels
    {
        static Random rnd = new Random();
        static int INDEX = 0;
        static List<string> lvls = new List<string>();
        public static void Factory(out char[,] chararr, out string landstring)
        {

            lvls.Add("████████████████████████████████████████\n" +
                     "                                       █\n" +
                     "█  █████      ███        █████████████ █\n" +
                     "█  █████      ███        █          ██ █\n" +
                     "█  ██    ███   ██        █          ██ █\n" + // (32, 5)
                     "█        ███   ██ ████ █ █          ██ █\n" +
                     "█ ███    ███   ██ ████ █ █████████████ █\n" +
                     "█ ████  ██████ ██ ████ █               █\n" +
                     "█   ██  ████   ██ ████ ███ ██████████  █\n" +
                     "█   ██  ████ ████ ████ ███ ██     ███  █\n" +
                     "█ ████  ████ ████ ████ ███ ██     ███  █\n" +
                     "█                                 ███  █\n" +
                     "█ ██ ███████ ██     ██ ██████     ███  █\n" +
                     "█ ██ ███████ ██     ██ ██████████████  █\n" +
                     "█ ██ ███████ ██     ██ ██████████████  █\n" +
                     "█ ██ ███████ █████████ █████           █\n" +
                     "█ ██                ██ █████           █\n" + // (33, 17)
                     "█ ██                ██ █████           █\n" + // (8, 18)
                     "█                                       \n" +
                     "████████████████████████████████████████");

            lvls.Add("████████████████████████████████████████\n" +
                     "                                       █\n" +
                     "█             ███        █████████████ █\n" +
                     "█             ███        █          ██ █\n" +
                     "█        ███   ██        █          ██ █\n" + // (32, 5)
                     "█        ███   ██ █  █ █ █          ██ █\n" +
                     "█ ███    ███   ██ █  █ █ █████████████ █\n" +
                     "█ ████  ██████ ██ █  █ █               █\n" +
                     "█   ██  ████   ██ █  █ ███ ██████████  █\n" +
                     "█   ██  ████ ████ █  █ ███ ██     ███  █\n" +
                     "█ ████  ████ ████ █  █ ███ ██     ███  █\n" +
                     "█                                 ███  █\n" +
                     "█            ██     ██ ██████     ███  █\n" +
                     "█            ██     ██ ██████████████  █\n" +
                     "█            ██     ██ ██████████████  █\n" +
                     "█    ███████ █████████ █████           █\n" +
                     "█                   ██ █████           █\n" + // (33, 17)
                     "█                   ██ █████           █\n" + // (8, 18)
                     "█                                       \n" +
                     "████████████████████████████████████████");
            // ↓    ↓   ↓   ↓   Converters   ↓    ↓   ↓   ↓   
            // from string TO string array
            string[] stringarr = lvls[INDEX++].Split("\n");
            chararr = StringArrToCharArr(stringarr);
            landstring = CharArrToString(chararr);
        }

        public static void NextLevel(out char[,] chararr, out string landstring)
        {
            string[] stringarr = lvls[INDEX++].Split("\n");
            chararr = StringArrToCharArr(stringarr);
            landstring = CharArrToString(chararr);
        }

        public static string CharArrToString(char[,] chararr)
        {
            // from 2DcharArray BACK TO string
            string landstring = "";
            for (int i = 0; i <= chararr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= chararr.GetUpperBound(1); j++)
                {
                    landstring += chararr[i, j];
                }
                if (i < chararr.GetUpperBound(0)) landstring += "\n";
            }

            return landstring;
        }

        public static char[,] StringArrToCharArr(string[] stringarr)
        {
            // from stringArray TO char 2DcharArray
            char[,] chararr = new char[20, 40];
            for (int i = 0; i <= chararr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= chararr.GetUpperBound(1); j++)
                {
                    chararr[i, j] = stringarr[i][j];
                }
                if (i < chararr.GetUpperBound(0)) { }
            }

            return chararr;
        }
        public static void CharArrRandomLines(out char[,] chararr, out string landstring)
        {
            // from stringArray TO char 2DcharArray
            char[,] cha = new char[20, 40];
            for (int i = 0; i <= cha.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= cha.GetUpperBound(1); j++) { cha[i, j] = '█'; }
                if (i < cha.GetUpperBound(0)) { }
            }
            int ii = 30;
            while (ii > 0)
            {
                int x = rnd.Next(1,19);
                int[] y = new int[2] { rnd.Next(1, 39), rnd.Next(1, 39) };
                y = (y[0] > y[1]) ? new int[2] { y[1], y[0] } : y;
                for (int i = y[0]; i <= y[1]; i++)
                {
                    cha[x, i] = ' ';
                }
                ii--;
            }
            ii = 30;
            while (ii > 0)
            {
                int[] x = new int[2] { rnd.Next(1, 19), rnd.Next(1, 19) };
                int y = rnd.Next(1,39);
                x = (x[0] > x[1]) ? new int[2] { x[1], x[0] } : x;
                for (int i = x[0]; i <= x[1]; i++)
                {
                    cha[i, y] = ' ';
                }
                ii--;
            }


            chararr = cha;
            landstring = CharArrToString(cha);
        }


    }
}
