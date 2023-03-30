/* TOREAD Image Collapse
 * https://robertheaton.com/2018/12/17/wavefunction-collapse-algorithm/
 */

/* TOREAD A* Algoritm
 * https://en.wikipedia.org/wiki/A*_search_algorithm
 * https://en.wikipedia.org/wiki/Taxicab_geometry
 */
// DID: Ge spelaren några egenskaper Styrka, Smidighet, Hälsa.
// Dem behöver alltså egenskap Fjärrsyn. och Stryka (struntar i HP tillsvidare))
// TASK: Funktion för att och placera in Monster

// TASK: Metoder för att slumpa fram kartor,
// där man kan välja vilka man ska spara till en json
using System.Numerics;
using static System.Console;
namespace DungeonLab
{
    enum DirE { left, up, right, down }
    enum P { Y, X }
    struct V2
    {
        public int x;
        public int y;

        public V2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class Player
    {
        int xpos, ypos, hp, strength, agility;
        public Player()
        {
            xpos = 0;
            ypos = 1;
            hp = 10;
            strength = 10;
            agility = 10;
        }
        public int Xpos { get => xpos; set => xpos = value; }
        public int Ypos { get => ypos; set => ypos = value; }
        public void Move(int direction)
        {
            if (direction == 0) xpos--;
            else if (direction == 1) ypos--;
            else if (direction == 2) xpos++;
            else if (direction == 3) ypos++;
        }
        public void NewLevelInit()
        {
            xpos = 0;
            ypos = 1;
        }
    }

    internal class Program
    {
        static bool edit = true;
        static bool ingame = false;
        // char[,] Landscape = new char[20, 40];
        public static void Main()
        {
            char[,] chararr;
            string landstring;
            Levels.Factory(out chararr, out landstring);
            // Setups
            Player pl = new Player();
            ConsoleKeyInfo cki;
            List<Zombie> Zombies = NewZombieList();
            V2 goal = new(39, 18);

            // 
            // TODO Implement Zombie
            /*           __  __   __   ____ _  _    __   _____ _____ ____ 
                        (  \/  ) /__\ (_  _( \( )  (  ) (  _  (  _  (  _ \
                         )    ( /(__)\ _)(_ )  (    )(__ )(_)( )(_)( )___/
                        (_/\/\_(__)(__(____(_)\_)  (____(_____(_____(__)  */

            SetWindowSize(1, 1);
            SetBufferSize(40, 30);
            SetWindowSize(40, 30);
            CursorVisible = false;

            if (edit)
            {
                cki = InEdit(ref chararr, ref landstring, pl, ref Zombies, goal);
            }
            if (ingame)
            {
                cki = InGame(ref chararr, ref landstring, pl, ref Zombies, goal);
            }

            Clear();
            WriteLine("Game Over Dude!");

            static ConsoleKeyInfo InEdit(ref char[,] chararr, ref string landstring, Player pl, ref List<Zombie> Zombies, V2 goal)
            {
                ConsoleKeyInfo cki;
                do
                {
                    SetCursorPosition(0, 0);
                    //Write(lvl2);
                    Write(landstring);

                    SetCursorPosition(2, 22);
                    Write("Press r for random maze.");

                    SetCursorPosition(2, 24);
                    Write("blablabla");

                    SetCursorPosition(2, 25);
                    Write("blablabla");

                    SetCursorPosition(2, 26);
                    Write("Press esc to exit.");

                    cki = ReadKey(true);

                    switch (cki.Key)
                    {
                        case ConsoleKey.R:
                            // Write("BAM Du tryckte på r");
                            Levels.CharArrRandomLines(out chararr, out landstring);
                            break;
                    }


                }
                while (cki.Key != ConsoleKey.Escape);  // end of do-while
                return cki;
            }
            static ConsoleKeyInfo InGame(ref char[,] chararr, ref string landstring, Player pl, ref List<Zombie> Zombies, V2 goal)
            {
                ConsoleKeyInfo cki;
                do
                {
                    SetCursorPosition(0, 0);
                    //Write(lvl2);
                    Write(landstring);

                    SetCursorPosition(pl.Xpos, pl.Ypos);
                    Write("☻");

                    foreach (var z in Zombies)
                    {
                        z.Update(pl, ref chararr);
                    }

                    SetCursorPosition(2, 22);
                    Write("Player Pos = " + pl.Xpos + " " + pl.Ypos);

                    SetCursorPosition(2, 24);
                    Write("Detected = " + Zombies[0].Detected);

                    SetCursorPosition(2, 25);
                    Write("Detected =  " + Zombies[0].Xpos + " " + Zombies[0].Ypos);

                    SetCursorPosition(2, 26);
                    Write("Press esc to exit.");

                    cki = ReadKey(true);

                    switch (cki.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if ((pl.Xpos >= 0) && IsPosFree(pl.Xpos - 1, pl.Ypos, chararr))
                                pl.Move((int)DirE.left);
                            break;
                        case ConsoleKey.UpArrow:
                            if ((pl.Ypos >= 0) && IsPosFree(pl.Xpos, pl.Ypos - 1, chararr))
                                pl.Move((int)DirE.up);
                            break;
                        case ConsoleKey.RightArrow:
                            if ((pl.Xpos <= BufferWidth) && IsPosFree(pl.Xpos + 1, pl.Ypos, chararr))
                                pl.Move((int)DirE.right);
                            break;
                        case ConsoleKey.DownArrow:
                            if ((pl.Ypos <= 20) && IsPosFree
                                (pl.Xpos, pl.Ypos + 1, chararr))
                                pl.Move((int)DirE.down);
                            break;
                    }

                    if ((pl.Ypos == goal.y) && (pl.Xpos == goal.x))
                    {
                        Levels.NextLevel(out chararr, out landstring);
                        pl.NewLevelInit();
                        Zombies = NewZombieList();
                    }
                }
                while (cki.Key != ConsoleKey.Escape);  // end of do-while
                return cki;
            }
        }


        private static List<Zombie> NewZombieList()
        {
            return new()
            {
                new Zombie(32, 5),
                new Zombie(33, 17),
                new Zombie(8, 18),
            };
        }

        private static bool IsPosFree(int x, int y, char[,] chararr)
        {
            if (chararr[y, x] == ' ')
                return true;
            return false;
        }
        // TBD If Pos = g pos

        public static void Cleaner()
        {

        }
    }
}
// TODO Blitta inte hela "skärmen" gör istället en statisk städare som byter ut tecknet på positionen aktören har lämnat