// TODO: Ge spelaren några egenskaper Styrka, Smidighet, Hälsa.
// Dem behöver alltså egenskap Fjärrsyn. och Stryka (struntar i HP tillsvidare))
// TASK: Att kunna rita kartor med musen, och placera 
using System;
using static System.Console;
namespace DungeonLab
{
    class Player
    {
        int xpos, ypos, hp, strength, agility;

        public Player()
        {
            xpos = 1;
            ypos = 1;
            hp = 10;
            strength=10;
            agility=10;

        }

        public int[] Pos => new int[] { xpos, ypos };
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
        // TODO: Påbörja klassen Zombies
        // TBD: MoveZmb()
        // TBD: 
        // 
        // (Förväntat Zoombiebeteende är att dem ska driva planlöst tills spelaren kommer närmre än 5 steg)
        // Å andra sidan är det låttare att checka om (zombien & spelaren är på samma rad) och (inget är emellan) 
        int xpos, ypos, sight;
        public Zombie(int Xpos, int Ypos)
        {
            xpos = Xpos;
            ypos = Ypos;
            sight = 10;
        }
    }

    internal class Program
    {
        public static void Main()
        {
            char[,] Landscape = new char[20, 40];

            string lvl2 = "████████████████████████████████████████\n" +
                          "█  █████      ███                      █\n" +
                          "█  █████      ███        █████████████ █\n" +
                          "█  █████      ███        █          ██ █\n" +
                          "█  ██    ███   ██        █     Z    ██ █\n" + // (32, 5)
                          "█        ███   ██ ████ ███          ██ █\n" +
                          "█████    ███   ██ ████ ███████████████ █\n" +
                          "██████  ██████ ██ ████ ███             █\n" +
                          "█   ██  ████   ██ ████ ███ ██████████ ██\n" +
                          "█   ██  ████ ████ ████ ███ ██     ███ ██\n" +
                          "█ ████  ████ ████ ████ ███ ██     ███ ██\n" +
                          "█                                 ███ ██\n" +
                          "████ ███████ ██     ██ ██████     ███ ██\n" +
                          "████ ███████ ██     ██ ██████████████ ██\n" +
                          "████ ███████ ██     ██ ██████████████ ██\n" +
                          "████ ███████ █████████ █████          ██\n" +
                          "████      ██        ██ █████    Z     ██\n" + // (33, 17)
                          "████   Z  ██        ██ █████          ██\n" + // (8, 18)
                          "████      ██        ██                ██\n" +
                          "████████████████████████████████████████";
            // ↓    ↓   ↓   ↓   Converters   ↓    ↓   ↓   ↓   
            // from string TO string array
            string[] stringarr = lvl2.Split("\n");

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

            // Setups
            Player pl = new Player();
            ConsoleKeyInfo cki;
            List<Zombie> Zombies = new()
            {
                new Zombie(32, 5),
                new Zombie(33, 17),
                new Zombie(8, 18),
            };

            // TODO
            // TODO
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

                    SetCursorPosition(pl.Pos[0], pl.Pos[1]);
                    Write("☺");

                    SetCursorPosition(2, 22);
                    Write("Player Pos = " + pl.Pos[0] + " " + pl.Pos[1]);

                    SetCursorPosition(2, 24);
                    Write("Press esc to exit.");

                    cki = ReadKey(true);

                    switch (cki.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if ((pl.Pos[0] > 0) && IsPosFree(pl.Pos[0] - 1, pl.Pos[1]))
                                pl.Move(0);
                            break;
                        case ConsoleKey.UpArrow:
                            if ((pl.Pos[1] > 0) && IsPosFree(pl.Pos[0], pl.Pos[1] - 1))
                                pl.Move(1);
                            break;
                        case ConsoleKey.RightArrow:
                            if ((pl.Pos[0] < BufferWidth - 1) && IsPosFree(pl.Pos[0] + 1, pl.Pos[1]))
                                pl.Move(2);
                            break;
                        case ConsoleKey.DownArrow:
                            if ((pl.Pos[1] < 20 - 1) && IsPosFree(pl.Pos[0], pl.Pos[1] + 1))
                                pl.Move(3);
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
            bool IsPosFree(int x, int y)
            {
                if (chararr[y, x] == ' ')
                    return true;
                return false;
            }
        }
        public static void Cleaner()
        {

        }
    }
}

// TODO Blitta inte hela "skärmen" gör istället en statisk städare som byter ut tecknet på positionen aktören har lämnat