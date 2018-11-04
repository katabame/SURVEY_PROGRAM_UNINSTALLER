using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SURVEY_PROGRAM_UNINSTALLER
{
    class Program
    {
        static void Main()
        {
            Dictionary<string, string> localeJapanese = new Dictionary<string, string>()
            {
                { "notFound", "DELTARUNEは見つかりませんでした！" },
                { "deleting", "削除中: " },
                { "directoryNotFoundException", "次のフォルダが見つかりませんでした。処理をスキップします: " },
                { "uninstallFinished", "アンインストール完了！" },
                { "pressAnyKey", "何かキーを押して終了..." }
            };

            Dictionary<string, string> localeEnglish = new Dictionary<string, string>()
            {
                { "notFound", "No DELTARUNE found!" },
                { "deleting", "Deleting: " },
                { "directoryNotFoundException", "Folder not found, Skipping process: " },
                { "uninstallFinished", "Uninstall finished!" },
                { "pressAnyKey", "Press any key to exit..." }
            };

            string locale = System.Globalization.CultureInfo.CurrentCulture.Name;
            Dictionary<string, string> localeSet;
            if (locale == "ja-JP")
            {
                localeSet = localeJapanese;
            }
            else
            {
                localeSet = localeEnglish;
            }

            void DeleteFile(FileInfo info)
            {
                if (info.Exists)
                {
                    Console.WriteLine(localeSet["deleting"] + info.FullName);
                    if ((info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        info.Attributes = FileAttributes.Normal;
                    }
                    info.Delete();

                    while (info.Exists)
                    {
                        info.Refresh();
                    }
                }
            }

            void DeleteDirectory(DirectoryInfo info)
            {
                if (info.Exists)
                {
                    Console.WriteLine(localeSet["deleting"] + info.FullName);
                    if ((info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        info.Attributes = FileAttributes.Normal;
                    }
                    info.Delete();

                    while (info.Exists)
                    {
                        info.Refresh();
                    }
                }
            }

            List<string> files = new List<string>
            {
                "AUDIO_INTRONOISE.ogg",
                "audiogroup1.dat",
                "data.win",
                "DELTARUNE.exe",
                "license.txt",
                "snd_closet_fall.ogg",
                "snd_closet_impact.ogg",
                "snd_great_shine.ogg",
                "snd_paper_rumble.ogg",
                "snd_paper_surf.ogg",
                "snd_revival.ogg",
                "snd_rurus_appear.ogg",
                "snd_usefountain.ogg",
                "uninstall.exe",
                @"lang\lang_en.json",
                @"lang\lang_ja.json",
                @"mus\april_2012.ogg",
                @"mus\AUDIO_ANOTHERHIM.ogg",
                @"mus\AUDIO_DARKNESS.ogg",
                @"mus\AUDIO_DRONE.ogg",
                @"mus\AUDIO_STORY.ogg",
                @"mus\basement.ogg",
                @"mus\battle.ogg",
                @"mus\bird.ogg",
                @"mus\card_castle.ogg",
                @"mus\castletown_empty.ogg",
                @"mus\charjoined.ogg",
                @"mus\checkers.ogg",
                @"mus\creepychase.ogg",
                @"mus\creepydoor.ogg",
                @"mus\creepylandscape.ogg",
                @"mus\dogcheck.ogg",
                @"mus\dontforget.ogg",
                @"mus\elevator.ogg",
                @"mus\fanfare.ogg",
                @"mus\field_of_hopes.ogg",
                @"mus\forest.ogg",
                @"mus\friendship.ogg",
                @"mus\GALLERY.ogg",
                @"mus\hip_shop.ogg",
                @"mus\home.ogg",
                @"mus\joker.ogg",
                @"mus\kingboss.ogg",
                @"mus\lancer.ogg",
                @"mus\lancer_susie.ogg",
                @"mus\lancerfight.ogg",
                @"mus\legend.ogg",
                @"mus\man.ogg",
                @"mus\mus_birdnoise.ogg",
                @"mus\mus_introcar.ogg",
                @"mus\mus_school.ogg",
                @"mus\ocean.ogg",
                @"mus\prejoker.ogg",
                @"mus\quiet_autumn.ogg",
                @"mus\ruruskaado.ogg",
                @"mus\s_neo.ogg",
                @"mus\s_neo_clip.ogg",
                @"mus\shop1.ogg",
                @"mus\tense.ogg",
                @"mus\THE_HOLY.ogg",
                @"mus\thrash_rating.ogg",
                @"mus\thrashmachine.ogg",
                @"mus\town.ogg",
                @"mus\vs_susie.ogg",
                @"mus\w.ogg",
                @"mus\wind.ogg"
            };

            List<string> directories = new List<string>
            {
                "lang",
                "mus"
            };

            List<string> shortcuts = new List<string>
            {
                "SURVEY_PROGRAM License.lnk",
                "SURVEY_PROGRAM.lnk",
                "Uninstall.lnk"
            };


            // Check installed folder
            string uninstallString;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\SURVEY_PROGRAM"))
            {
                if (key == null)
                {
                    Console.WriteLine(localeSet["notFound"]);
                    Console.WriteLine(localeSet["pressAnyKey"]);
                    Console.ReadLine();
                    return;
                }
                else
                {
                    uninstallString = (string)key.GetValue("UninstallString");
                    uninstallString = uninstallString.Trim('\"');
                }
            }


            // Delete files
            string deltaruneFolder = Path.GetDirectoryName(uninstallString);
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(deltaruneFolder + @"\" + file);
                DeleteFile(fi);
            }

            // Delete Empty Directories
            foreach (string directory in directories)
            {
                DirectoryInfo di = new DirectoryInfo(deltaruneFolder + @"\" + directory);
                DeleteDirectory(di);
            }
            DeleteDirectory(new DirectoryInfo(deltaruneFolder));


            // Delete Save datas
            string localAppdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string savePath = localAppdata + @"\DELTARUNE\";
            DirectoryInfo sdi = new DirectoryInfo(savePath);

            try
            {
                foreach (FileInfo ch in sdi.EnumerateFiles("filech1_?"))
                {
                    DeleteFile(ch);
                }

                foreach (FileInfo ch in sdi.EnumerateFiles("filech2_?"))
                {
                    DeleteFile(ch);
                }

                foreach (FileInfo ch in sdi.EnumerateFiles("filech3_?"))
                {
                    DeleteFile(ch);
                }

                DeleteFile(new FileInfo(savePath + "dr.ini"));
                DeleteDirectory(new DirectoryInfo(savePath));
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(localeSet["directoryNotFoundException"] + e.Message);
            }


            // Delete startmenu shortcuts
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string shortcutsPath = appdata + @"\Microsoft\Windows\Start Menu\Programs\SURVEY_PROGRAM\";
            foreach (string shortcut in shortcuts)
            {
                FileInfo sfi = new FileInfo(shortcutsPath + shortcut);
                DeleteFile(sfi);
            }
            DeleteDirectory(new DirectoryInfo(shortcutsPath));


            // Delete desktop shortcut
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FileInfo dsi = new FileInfo(desktop + @"\SURVEY_PROGRAM.lnk");
            DeleteFile(dsi);


            // Delete Registry key
            Registry.CurrentUser.DeleteSubKeyTree(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\SURVEY_PROGRAM");

            Console.WriteLine(localeSet["uninstallFinished"]);
            Console.WriteLine(localeSet["pressAnyKey"]);
            Console.ReadLine();
        }
    }
}
