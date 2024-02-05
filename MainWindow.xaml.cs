using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MonsterHunterWikiExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string path;
        public static MainWindow instance;
        public static bool FinishedLoad = false;
        public static string lastMonsterName = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            path = Environment.CurrentDirectory + "\\";
            Console.WriteLine(path);
            instance = this;
            MonsterData.Init();
            ImageLoader.loadSavedImages();
            c.ww("Hello!");
            FinishedLoad = true;
        }

        public void testcommand()
        {
            
        }

        private void MakeList_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] lines = File.ReadAllLines(path + "Names.txt");
                string[] lines2 = new string[lines.Length];
                Console.WriteLine("Lines: " + lines.Length);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    Console.WriteLine(line);
                    string result = line.ToLower();
                    result = result.Replace(' ', '_');
                    lines2[i] = result + "," + lines[i];

                }
                Console.WriteLine("===========================================================");
                for(int k = 0; k < lines2.Length; k++)
                {
                    Console.WriteLine(lines2[k]);
                }
                File.WriteAllLines("Monsters.txt", lines2);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        public static void UpdateSearchresults(string[] results)
        {
            instance.SearchOrderResults.ItemsSource = results;
            //foreach (string s in results)
            //{
            //    instance.SearchOrderResults.Items.Add(s);
            //}
        }

        private void SearchBoxtextChanged(object sender, TextChangedEventArgs e)
        {
            Search.UpdateSearchResults(SearchBox.Text);
        }

        private void TextBoxKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter || e.Key == Key.Tab)
            {
                SearchOrderResults.SelectedIndex = 0;
            }
        }

        private void ReselectAfterChange()
        {
            if (string.IsNullOrEmpty(lastMonsterName)) 
            {
                SearchOrderResults.SelectedIndex = 0;
            }
            else
            {
                bool matched = false;
                for (int i = 0; i < Search.SearchResults.Count; i++)
                {
                    string s = Search.SearchResults[i];
                    string monsterName = s;
                    if (s.Contains('['))
                    {
                        int start = s.IndexOf("[") - 1;
                        monsterName = s.Substring(0, start);
                    }
                    if (monsterName == lastMonsterName)
                    {
                        matched = true;
                        SearchOrderResults.SelectedIndex = i;
                        break;
                    }
                }
                if (!matched)
                {
                    SearchOrderResults.SelectedIndex = 0;
                }
            }
        }

        public Monster GetSelectedMonster()
        {
            string selection = Search.SearchResults[SearchOrderResults.SelectedIndex];
            if (selection.Contains('['))
            {
                int start = selection.IndexOf("[") - 1;
                string filtered = selection.Substring(0, start);
                c.ww("Filtered Name to {" + filtered + "}");
                return MonsterData.getMonsterByVisibleName(filtered);
            }
            else
                return MonsterData.getMonsterByVisibleName(selection);
        }

        private void SearchOrderResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchOrderResults.SelectedIndex < 0)
            {
                ReselectAfterChange();
            }
            //c.ww("Selection Changed To :" + SearchOrderResults.SelectedValue);
            Monster monster = GetSelectedMonster();
            lastMonsterName = monster.VisibleName;
            c.w("Loaded monster [");
            c.w(monster.VisibleName, ConsoleColor.Yellow);
            c.ww("]");
            c.w("Ranks: ");

            if (monster.Hr)
            {
                c.w("HR", ConsoleColor.Red);
                HighRankTab.IsEnabled = true;
            }
            else
            {
                HighRankTab.IsEnabled = false;
            }
            if (monster.Mr)
            {
                c.w("MR", ConsoleColor.Green);
                MasterRankTab.IsEnabled = true;
            }
            else
            {
                MasterRankTab.IsEnabled = false;
            }
            if (!(monster.Mr && monster.Hr))
            {
                if (monster.Hr)
                {
                    RankTabs.SelectedIndex = 1;
                }
                else
                {
                    RankTabs.SelectedIndex = 0;
                }
            }
            c.l();
            c.ww("Resistences{ poison:" + monster.Poison + ", sleep:" + monster.Sleep + ", paralysis:" + monster.Paralysis + ", blast:" + monster.Blast + ", stun:" + monster.Stun + " }");
            BitmapImage hrImage = null;
            BitmapImage mrImage = null;
            //Task task = Task.Run(() => {
            if (monster.Hr)
            {
                hrImage = ImageLoader.loadImage(monster, false);
                SetImages(hrImage, false);
            }
            if (monster.Mr)
            {
                mrImage = ImageLoader.loadImage(monster, true);
                SetImages(mrImage, true);
            }
            //});
        }

        public static void SetImages(BitmapImage image, bool TFMrHr)
        {
            instance.Dispatcher.Invoke(() => {
                if (TFMrHr)
                {
                    c.ww("SetMRImageSource");
                    instance.MasterRankImageSource.Source = image;
                }
                else
                {
                    c.ww("SetHRImageSource");
                    instance.HighRankImageSource.Source = image;
                }
            }); 
        }

        private void testButton_OnClick(object sender, RoutedEventArgs e)
        {
            ImageLoader.SaveImages();
        }

        private void SortingButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!FinishedLoad) return;

            //check each radio button and set sort
            int sort = 0;
            if (SBNoSort.IsChecked.Value) sort = 0;
            if (SBPoison.IsChecked.Value) sort = 1;
            if (SBSleep.IsChecked.Value) sort = 2;
            if (SBParalysis.IsChecked.Value) sort = 3;
            if (SBBlast.IsChecked.Value) sort = 4;
            if (SBStun.IsChecked.Value) sort = 5;

            //if sort is = 0(NoSort) we enable the searchBox
            //if not 0 then we disable the searchBox

            if (sort == 0) 
            { 
                SearchBox.IsEnabled = true;
                Search.UpdateSearchResults(SearchBox.Text);
                return;
            }
            else SearchBox.IsEnabled = false;
            Search.SortResults(sort);
        }
    }
}
