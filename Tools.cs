using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterHunterWikiExplorer
{
    public static class Tools
    {
        public static bool debugText = false;

        /// <summary>
        /// for string A that is less than B
        /// evaluate the levenshtien distance for every transposition of A onto an equal length segment of B
        /// assume strings under length 1000
        /// levenshtien distance in steps of 1000
        /// distance from the start in steps of 1
        /// this prioritizes matches closer to the beginning of the string
        /// ex.  "test"
        ///     [whichisbest] = 1007
        ///     [bestiswhich] = 1000
        /// return the smallest of all transpositions
        /// LAGALERT!!!!!!!!!!
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int VeryHardlevenshtein(string a, string b)
        {

            string aa = a.ToLower();
            string bb = b.ToLower();

            if (debugText)
            {
                c.w("       VeryHardlevenshtein score for [");
                c.w(aa, ConsoleColor.Green);
                c.w("] and [");
                c.w(bb, ConsoleColor.Green);
                c.ww("]");
            }
            //Get ready for some lag
            int lowestDistance = int.MaxValue;
            if (debugText) c.w("           Breakdown:");
            //evaluate a segment of b starting at index 0
            //this loop will iterate through every segment of length a
            // EX:  A: for  B: therefor
            //    [for/the]refor > t[for/her]efor > th[for/ere]for > Etc..
            for (int i = 0; i < bb.Length - aa.Length; i++)
            {
                int end = i + aa.Length;
                string segment = bb.Substring(i, end - i);
                int leven = levenshtein(aa, segment) * 1000 + i;

                if (debugText)
                {
                    c.w(aa, ConsoleColor.Yellow);
                    c.w("/");
                    c.w(segment, ConsoleColor.Yellow);
                    c.w(":");
                    c.w(leven.ToString(), ConsoleColor.Magenta);
                    c.w(" ");
                }
                if (leven < lowestDistance)
                {
                    lowestDistance = leven;
                }
            }

            //also compare against regular levenshtein
            int rleven = levenshtein(aa, bb) * 1000;
            if (rleven < lowestDistance)
            {
                lowestDistance = rleven;
            }


            if (debugText)
            {
                c.w("Lowest=");
                c.ww(lowestDistance.ToString(), ConsoleColor.Red);
            }
            return lowestDistance;

            return int.MaxValue;
        }

        /// <summary>
        /// https://en.wikibooks.org/wiki/Algorithm_Implementation/Strings/Levenshtein_distance#C#
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int levenshtein(string a, string b)
        {

            if (string.IsNullOrEmpty(a))
            {
                if (!string.IsNullOrEmpty(b))
                {
                    return b.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(b))
            {
                if (!string.IsNullOrEmpty(a))
                {
                    return a.Length;
                }
                return 0;
            }

            int cost;
            int[,] d = new int[a.Length + 1, b.Length + 1];
            int min1;
            int min2;
            int min3;

            for (int i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (int i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (int i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (int j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = (a[i - 1] != b[j - 1]) ? 1 : 0;

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];

        }
    }

    public static class c
    {
        public static ConsoleColor foreground = ConsoleColor.White;
        public static ConsoleColor Background = ConsoleColor.Black;
        public static List<string> errorLog = new List<string>();


        public static void w(string s)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = Background;
            Console.Write(s);
        }
        public static void ww(string s)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = Background;
            Console.WriteLine(s);
        }
        public static void w(string s, ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.BackgroundColor = Background;
            Console.Write(s);
        }
        public static void ww(string s, ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.BackgroundColor = Background;
            Console.WriteLine(s);
        }

        public static void l()
        {
            Console.WriteLine();
        }
        public static void l(int lines)
        {
            for (int i = 0; i < lines; i++)
                Console.WriteLine();
        }

        public static void e(string s)
        {
            l();
            ww(s, ConsoleColor.Red);
            l();
        }

        public static void er(string s)
        {

            errorLog.Add(s);
        }

        public static string r()
        {
            return Console.ReadLine();
        }
    }
}
