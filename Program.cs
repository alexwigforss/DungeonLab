// TODO: Försök att greppa det här med screenbuffers. Så jag kan bygga nästa bild "huvudlöst"
// och "blitta" till consolen för att slippa det lilla flimmret vid ny bild.
// https://learn.microsoft.com/en-us/windows/console/console-screen-buffers
// TODO: Ge spelaren några egenskaper Styrka, Smidighet, Hälsa.
// TODO: Påbörja klassen Zombies
// (Förväntat Zoombiebeteende är att dem ska driva planlöst tills spelaren kommer närmre än 5 steg)
// Dem behöver alltså egenskap Fjärrsyn. och stryka (struntar i HP tillsvidare))

using System;
using static System.Console;
namespace DungeonLab
{
    class Player
    {
        int xpos, ypos;
        public Player()
        {
            xpos = 10;
            ypos = 10;
        }
        public int[] Pos => new int[] { xpos, ypos };
        public void Move(int direction)
        {
            if (direction == 0) xpos--;
            else if (direction == 1) ypos--;
            else if (direction == 2) xpos++;
            else if (direction == 3) ypos++;
        }
        internal class Program
        {
            public static void Main()
            {
                char[,] landscape = new char[20, 40];

                string lvl2 = "████████████████████████████████████████\n" +
                              "█    █            █             █      █\n" +
                              "█    █            █             █      █\n" +
                              "█    █            █             █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█    █     █      █      █      █      █\n" +
                              "█          █             █             █\n" +
                              "█          █             █             █\n" +
                              "█          █             █             █\n" +
                              "████████████████████████████████████████";

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

                Player pl = new Player();
                ConsoleKeyInfo cki;
                try
                {
                    SetWindowSize(1, 1);
                    SetBufferSize(40, 30);
                    SetWindowSize(40, 30);
                    CursorVisible = false;

                    do
                    {
                        Clear();

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
        }
    }
}