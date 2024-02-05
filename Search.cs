using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterHunterWikiExplorer
{
    internal static class Search
    {
        public static List<string> SearchResults = new List<string>();

        public static void SortResults(int sort)
        {
            SearchResults.Clear();
            Dictionary<string, int> distances = new Dictionary<string, int>();
            List<string> threeStarNames = new List<string>();
            List<string> twoStarNames = new List<string>();
            List<string> oneStarNames = new List<string>();
            List<string> noStarNames = new List<string>();

            foreach (Monster m in MonsterData.monsters)
            {
                int stars = 0;
                if (sort == 1) stars = m.Poison;
                if (sort == 2) stars = m.Sleep;
                if (sort == 3) stars = m.Paralysis;
                if (sort == 4) stars = m.Blast;
                if (sort == 5) stars = m.Stun;
                if (stars == 3)
                {
                    threeStarNames.Add(m.VisibleName);
                }
                else if (stars == 2)
                {
                    twoStarNames.Add(m.VisibleName);
                }
                else if (stars == 1)
                {
                    oneStarNames.Add(m.VisibleName);
                }
                else
                {
                    noStarNames.Add(m.VisibleName);
                }



                //string starText = "";
                //if (stars > 0)
                //{
                //    starText = " [";
                //    for (int i = 0; i < stars; i++)
                //    {
                //        starText = starText + "★";
                //    }
                //    starText = starText + "]";
                //}
                //distances.Add(m.VisibleName + starText, (3 - stars) * 100);
            }

            threeStarNames.Sort();
            twoStarNames.Sort();
            oneStarNames.Sort();
            noStarNames.Sort();

            for (int i = 0; i < threeStarNames.Count; i++)
            {
                distances.Add(threeStarNames[i] + " [★★★]", i);
            }
            for (int i = 0; i < twoStarNames.Count; i++)
            {
                distances.Add(twoStarNames[i] + " [★★]", 100 + i);
            }
            for (int i = 0; i < oneStarNames.Count; i++)
            {
                distances.Add(oneStarNames[i] + " [★]", 200 + i);
            }
            for (int i = 0; i < noStarNames.Count; i++)
            {
                distances.Add(noStarNames[i] + " [x]", 300 + i);
            }
            SearchResults.AddRange(SortDictionaryByValue(distances));
            MainWindow.UpdateSearchresults(SearchResults.ToArray());
        }

        public static void UpdateSearchResults(string filter)
        {
            if (filter == "" || !MainWindow.instance.SearchBox.IsEnabled)
            {
                SearchResults.Clear();
                SearchResults.AddRange(MonsterData.MonsterNames);
            }
            else
            {
                SearchResults.Clear();
                Dictionary<string, int> distances = new Dictionary<string, int>();
                foreach (string s in MonsterData.MonsterNames)
                {
                    distances.Add(s, Tools.VeryHardlevenshtein(filter, s));

                }
                SearchResults.AddRange(SortDictionaryByValue(distances));
            }
            MainWindow.UpdateSearchresults(SearchResults.ToArray());
        }

        public static string[] SortDictionaryByValue(Dictionary<string, int> dict)
        {
            // Convert dictionary to list of KeyValuePair objects
            List<KeyValuePair<string, int>> list = dict.ToList();

            // Sort list by value (int) in ascending order
            list.Sort((x, y) => x.Value.CompareTo(y.Value));

            // Create a string array of sorted keys
            string[] sortedKeys = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                sortedKeys[i] = list[i].Key;
            }

            return sortedKeys;
        }
            
    }
}
