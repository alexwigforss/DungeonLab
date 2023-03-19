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
using System;
using System.Reflection.Emit;
using static System.Console;
namespace DungeonLab
{
    enum DirE { left, up, right, down }
    enum P { Y, X }

    class Player
    {
        int xpos, ypos, hp, strength, agility;
        bool hasSeen;
        public Player()
        {
            xpos = 1;
            ypos = 1;
            hp = 10;
            strength = 10;
            agility = 10;
            hasSeen = false;
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
    }
    class Zombie
    {
        Random rnd = new Random();
        // DID: Påbörja klassen Zombies
        // (Förväntat Zoombiebeteende är att dem ska driva planlöst tills spelaren kommer närmre än 5 steg)
        // Å andra sidan är det låttare att checka om (zombien & spelaren är på samma rad) och (inget är emellan) 
        int xpos, ypos, sight;
        char[,] ca;
        public Zombie(int Xpos, int Ypos)
        {
            xpos = Xpos;
            ypos = Ypos;
            sight = 5;
            Detected = false;
        }
        public int Xpos { get => xpos; set => xpos = value; }
        public int Ypos { get => ypos; set => ypos = value; }
        public bool Detected { get; set; }

        public void Update(Player plr, ref char[,] chararr)
        {
            ca = chararr;
            IsInSight(plr);
            if (Detected)
                Move(ref plr);
            else
                Drift();
            Draw();
        }
        public void Draw()
        {
            SetCursorPosition(xpos, ypos);
            if (!Detected) Write("Z");
            else Write("D");
        }
        public bool IsInSight(Player plr)
        {
            // Om på samma rad & inom synhåll
            if ((xpos == plr.Xpos) && (Math.Abs(ypos - plr.Ypos) < sight))
            {
                Detected = true;
                // TBD check for walls i x-led
                return true;
            }
            // Om på samma rad & inom synhåll
            else if ((ypos == plr.Ypos) && (Math.Abs(xpos - plr.Xpos) < sight * 2))
            {
                Detected = true;
                return true;
            }
            return false;
        }
        // DEBUG Blandat ihop riktningarna här lite
        public void Move(ref Player p)
        {
            bool moved = false;
            int distY = (Math.Abs(xpos - p.Xpos));
            int distX = (Math.Abs(ypos - p.Ypos));
            // TBD Om avstånd i yled > avstånd i xled
            //while (!moved)
            //{

            if (distY >= distX)
            {
                if ((xpos > p.Xpos) && IsPosFree(xpos - 1, ypos, ca))
                { xpos--; moved = true; }// left
                else if ((xpos < p.Xpos) && IsPosFree(xpos + 1, ypos, ca))
                { xpos++; moved = true; } // right
                else if (xpos == p.Xpos)
                    if ((ypos > p.Ypos) && IsPosFree(xpos, ypos - 1, ca))
                    { ypos--; moved = true; } // up
                    else if ((ypos < p.Ypos) && IsPosFree(xpos, ypos + 1, ca))
                    { ypos++; moved = true; } // down
            }
            if ((distY < distX) || (!moved))
            // else
            {
                if ((ypos > p.Ypos) && IsPosFree(xpos, ypos - 1, ca))
                { ypos--; moved = true; } // up
                else if ((ypos < p.Ypos) && IsPosFree(xpos, ypos + 1, ca))
                { ypos++; moved = true; } // down
                else if (ypos == p.Ypos)
                    if ((xpos > p.Xpos) && IsPosFree(xpos - 1, ypos, ca))
                    { xpos--; moved = true; }// left
                    else if ((xpos < p.Xpos) && IsPosFree(xpos + 1, ypos, ca))
                    { xpos++; moved = true; } // right
            }
            if (!moved)
            {
                Drift();
            }
            //}
        }
        public void Drift()
        {
            int num = rnd.Next(0, 5);
            if (num == 4) return;
            else if ((num == 0) && IsPosFree(xpos, ypos + 1, ca)) ypos++;
            else if ((num == 1) && IsPosFree(xpos + 1, ypos, ca)) xpos++;
            else if ((num == 2) && IsPosFree(xpos, ypos - 1, ca)) ypos--;
            else if ((num == 3) && IsPosFree(xpos - 1, ypos, ca)) xpos--;
        }
        // TBD: Collision Player
        // DOIN: Collision Wall
        public void IsNextToPlayer()
        {
            throw new NotImplementedException();
        }
        private static bool IsPosFree(int x, int y, char[,] chararr)
        {
            if (chararr[y, x] == ' ')
                return true;
            return false;
        }
    }

    internal class Program
    {
        char[,] Landscape = new char[20, 40];
        public static void Main()
        {
            char[,] chararr;
            string landstring;
            Factory(out chararr, out landstring);

            // Setups
            Player pl = new Player();
            ConsoleKeyInfo cki;
            List<Zombie> Zombies = new()
            {
                new Zombie(32, 5),
                new Zombie(33, 17),
                new Zombie(8, 18),
            };

            // 
            // TODO Implement Zombie
            /*           __  __   __   ____ _  _    __   _____ _____ ____ 
                        (  \/  ) /__\ (_  _( \( )  (  ) (  _  (  _  (  _ \
                         )    ( /(__)\ _)(_ )  (    )(__ )(_)( )(_)( )___/
                        (_/\/\_(__)(__(____(_)\_)  (____(_____(_____(__)  */

            try
            {
                SetWindowSize(1, 1);
                SetBufferSize(40, 30);
                SetWindowSize(40, 30);
                CursorVisible = false;
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
                            if ((pl.Xpos > 0) && IsPosFree(pl.Xpos - 1, pl.Ypos, chararr))
                                pl.Move((int)DirE.left);
                            break;
                        case ConsoleKey.UpArrow:
                            if ((pl.Ypos > 0) && IsPosFree(pl.Xpos, pl.Ypos - 1, chararr))
                                pl.Move((int)DirE.up);
                            break;
                        case ConsoleKey.RightArrow:
                            if ((pl.Xpos < BufferWidth - 1) && IsPosFree(pl.Xpos + 1, pl.Ypos, chararr))
                                pl.Move((int)DirE.right);
                            break;
                        case ConsoleKey.DownArrow:
                            if ((pl.Ypos < 20 - 1) && IsPosFree
                                (pl.Xpos, pl.Ypos + 1, chararr))
                                pl.Move((int)DirE.down);
                            break;
                    }
                }
                while (cki.Key != ConsoleKey.Escape);  // end of do-while
            } // end of try
            catch (IOException e)
            {
                WriteLine(e.Message);
            }
            finally
            {
                Clear();
                WriteLine("Game Over Dude!");
            }
        }

        private static bool IsPosFree(int x, int y, char[,] chararr)
        {
            if (chararr[y, x] == ' ')
                return true;
            return false;
        }

        private static void Factory(out char[,] chararr, out string landstring)
        {
            string lvl2 = "████████████████████████████████████████\n" +
                          "█                                      █\n" +
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
                          "█                                      █\n" +
                          "████████████████████████████████████████";
            // ↓    ↓   ↓   ↓   Converters   ↓    ↓   ↓   ↓   
            // from string TO string array
            string[] stringarr = lvl2.Split("\n");

            // from stringArray TO char 2DcharArray
            chararr = new char[20, 40];
            for (int i = 0; i <= chararr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= chararr.GetUpperBound(1); j++)
                {
                    chararr[i, j] = stringarr[i][j];
                }
                if (i < chararr.GetUpperBound(0)) { }
            }

            // from 2DcharArray BACK TO string
            landstring = "";
            for (int i = 0; i <= chararr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= chararr.GetUpperBound(1); j++)
                {
                    landstring += chararr[i, j];
                }
                if (i < chararr.GetUpperBound(0)) landstring += "\n";
            }
        }
        public static void Cleaner()
        {

        }
    }
}

// TODO Blitta inte hela "skärmen" gör istället en statisk städare som byter ut tecknet på positionen aktören har lämnat