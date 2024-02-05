using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MonsterHunterWikiExplorer
{
    public static class ImageLoader
    {
        public static Dictionary<string, BitmapImage> ImageCache = new Dictionary<string, BitmapImage>();
        public static string imageCacheFolder = MainWindow.path + "Images\\";
        public static Dictionary<string, BitmapImage> ImagesToSave = new Dictionary<string, BitmapImage>();
        

        public static void loadSavedImages()
        {
            string[] files = Directory.GetFiles(imageCacheFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file).Split('.')[0];
                string ext = Path.GetExtension(file).Substring(1);
                if (ext == "png")
                {
                    ImageCache.Add(name, new BitmapImage(new Uri(file)));
                }
            }
        }

        public static BitmapImage loadImage(Monster monster, bool TFMrHr)
        {
            string resourcename = monster.InternalName + "_hzv";
            if (!TFMrHr)
            {
                if (monster.Hr && monster.Mr)
                {
                    resourcename = "hr_" + resourcename;
                }
            }
            c.w("Loading Image {");
            c.w(resourcename, ConsoleColor.Magenta);
            c.ww("}");

            if (ImageCache.ContainsKey(resourcename))
            {
                c.ww("Found in cache!");
                return ImageCache[resourcename];
            }
            else
            {
                c.w("Loading from web from URL=");
                BitmapImage image = loadImageFromWeb(resourcename);
                ImageCache.Add(resourcename, image);
                ImagesToSave.Add(resourcename, image);
                MainWindow.instance.outputLabel.Content = "New Images Downloaded Make Sure to Save!";
                return image;
            }
        }

        public static BitmapImage loadImageFromWeb(string resourceName)
        {
            string url = "https://monsterhunterworld.wiki.fextralife.com/file/Monster-Hunter-World/" + resourceName + ".png";
            c.ww(url, ConsoleColor.Magenta);
            
            try 
            { 
                BitmapImage image = new BitmapImage(new Uri(url));
                return image;
            } catch (Exception ex)
            {
                c.ww("Failed!", ConsoleColor.Red);
                return null;
            }
            
        }

        public static void Save(this BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static void SaveImages()
        {
            foreach (string key in ImagesToSave.Keys) 
            {
                Save(ImagesToSave[key], imageCacheFolder + key + ".png");
            }
            ImagesToSave.Clear();
            MainWindow.instance.outputLabel.Content = "";
        }
    }
}
