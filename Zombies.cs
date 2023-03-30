using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace DungeonLab
{
    class Zombie
    {
        Random rnd = new Random();
        // DID: Påbörja klassen Zombies
        // DID: (Förväntat Zoombiebeteende är att dem ska driva planlöst tills spelaren kommer närmre än 5 steg)
        // TBD Å andra sidan är det mer realistiskt att checka om (zombien & spelaren är på samma rad) och (inget är emellan) 
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

}
