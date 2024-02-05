using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterHunterWikiExplorer
{
    public static class MonsterData
    {
        public static Monster[] monsters;
        public static string[] MonsterNames = new string[0];

        public static void Init()
        {
            readMonsterList();
            Search.UpdateSearchResults("");
        }

        public static void readMonsterList()
        {
            string[] lines = File.ReadAllLines(MainWindow.path + "MonsterData.txt");
            monsters = new Monster[lines.Length];
            List<string> names = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] split = line.Split(',');
                bool hr = false;
                bool mr = false;
                bool both = false;
                int poison = 0, sleep = 0, paralysis = 0, blast = 0, stun = 0;
                if (split[2] == "h")
                {
                    hr = true;
                }
                if (split[2] == "m")
                {
                    mr = true;
                }
                if (split.Length > 3)
                {
                    if (split[3] == "h")
                    {
                        both = true;
                        hr = true;
                    }
                    if (split[3] == "m")
                    {
                        both = true;
                        mr = true;
                    }
                }
                names.Add(split[1]);
                if (both)
                {
                    poison = Convert.ToInt32(split[4]);
                    sleep = Convert.ToInt32(split[5]);
                    paralysis = Convert.ToInt32(split[6]);
                    blast = Convert.ToInt32(split[7]);
                    stun = Convert.ToInt32(split[8]);
                }
                else
                {
                    poison = Convert.ToInt32(split[3]);
                    sleep = Convert.ToInt32(split[4]);
                    paralysis = Convert.ToInt32(split[5]);
                    blast = Convert.ToInt32(split[6]);
                    stun = Convert.ToInt32(split[7]);
                }
                Monster M = new Monster(split[1], split[0], hr, mr, poison, sleep, paralysis, blast, stun);
                monsters[i] = M;
            }
            MonsterNames = names.ToArray();
        }

        public static Monster getMonsterByVisibleName(string name)
        {
            foreach (Monster monster in monsters)
            {
                if (monster.VisibleName == name)
                {
                    return monster;
                }
            }
            return null;
        }
    }

    public class Monster
    {
        public string VisibleName;
        public string InternalName;
        public bool Hr;
        public bool Mr;
        public int Poison = 0;
        public int Sleep = 0;
        public int Paralysis = 0;
        public int Blast = 0;
        public int Stun = 0;

        public Monster(string visibleName, string internalName, bool Hr, bool Mr, int Poison, int Sleep, int Paralysis, int Blast, int Stun)
        {
            VisibleName = visibleName;
            InternalName = internalName;
            this.Hr = Hr;
            this.Mr = Mr;
            this.Poison = Poison;
            this.Sleep = Sleep;
            this.Paralysis = Paralysis;
            this.Blast = Blast;
            this.Stun = Stun;
        }
    }
}
