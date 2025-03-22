using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Sudo2
{
    internal class DataFunc
    {


        //public static DataFunc CurrentInstance { get; private set; }

        //CurrentInstance = this;
        //директория кнопок (я пока хуй знает как они работают)
        public static readonly Dictionary<Key, int> KeyToValueMap = new Dictionary<Key, int>
{
    { Key.D1, 1 }, { Key.NumPad1, 1 },
    { Key.D2, 2 }, { Key.NumPad2, 2 },
    { Key.D3, 3 }, { Key.NumPad3, 3 },
    { Key.D4, 4 }, { Key.NumPad4, 4 },
    { Key.D5, 5 }, { Key.NumPad5, 5 },
    { Key.D6, 6 }, { Key.NumPad6, 6 },
    { Key.D7, 7 }, { Key.NumPad7, 7 },
    { Key.D8, 8 }, { Key.NumPad8, 8 },
    { Key.D9, 9 }, { Key.NumPad9, 9 }
};
        public static int ERROR=0;
        public static bool AutoSave=false;
        public static bool InGame=false;
        public static bool TextFocus=false;
        public static bool InTemp=false;
        public static bool WinMod=false;



        public static void CheckingSaveNameAndStartGame(bool InGame, TextBox text)
        {

                    DataFunc.CheckDirectory();
            string Save = text.Text;
            string numbut="";
            string line;
            bool DoubleS = false;
            bool ErrorChar = false;
            char[] er = { '<', '>', ':', '"', '|', '/', '?', '\\', '*' };
            if (/*text.Text != ""*/ !string.IsNullOrWhiteSpace(text.Text))
            {
                foreach (char ch in Save)
                {
                    for (int i = 0; i < er.Length; i++)
                    {
                        if (er[i] == ch) 
                        {
                            if (InGame == true)
                            {
                                text.Focus();
                                Game.CurrentInstance.context.Text = "Название имеет запрещенные символы";
                            }
                            else 
                            {
                                DataFunc.ERROR = 8;
                                MainWindow.CurrentInstance.Delay2();
                            }
                           /* text.Focus();*/ ErrorChar = true; 
                        } 
                    }

                }
                if (ErrorChar == false)
                {

                    //else { MessageBox.Show("lalalalal"); }
                    var PathSavesName = Directory.GetCurrentDirectory();
                    PathSavesName += $@"\SudoData\SavesNameOnly\SavesName.txt";

                    StreamReader SavesNameRead = new StreamReader(PathSavesName);
                    while ((line = SavesNameRead.ReadLine()) != null)
                    {
                        if (line == Save) {
                            DoubleS = true;
                            if (InGame == true) 
                            {
                                if (Game.CurrentInstance.Title == "Новая игра "|| Game.CurrentInstance.Title == Save)
                                {

                                    Game.CurrentInstance.context.Text = "Сохранение успешно обновлено";
                                }
                                else if(Game.CurrentInstance.Title != "Новая игра "&Game.CurrentInstance.Title !=Save) { Game.CurrentInstance.context.Text = "Скопированное сохранение успешно обновлено"; }
                            }
                            else
                            {
                                DataFunc.ERROR = 9;
                                //ContextMenu.CurrentInstance.text.Focus();
                                TextFocus =true;
                                MainWindow.CurrentInstance.Delay2();
                            }

                        }
                    }
                    SavesNameRead.Close();
                    if (DoubleS == false)
                    {

                        if (InGame == true)
                        {
                           
                            //    StreamWriter SavesNameIN = new StreamWriter(PathSavesName, true);
                            //    SavesNameIN.WriteLine(Save);
                            //    SavesNameIN.Close();

                            //    var PathSave = Directory.GetCurrentDirectory();
                            //    PathSave += $@"\SudoData\Saves\{Save}.txt";
                            //    StreamWriter lana = new StreamWriter(PathSave);
                            //    //lana.WriteLine(testmg);
                            //    lana.WriteLine("карта");


                            //    for (int i = 0; i < 9; i++)
                            //    {
                            //        for (int j = 0; j < 9; j++)
                            //        {

                            //            numbut = Game.m[i, j].ToString();

                            //            lana.Write(numbut);
                            //        }
                            //        lana.WriteLine();
                            //    }

                            //    lana.WriteLine("проверка");
                            //    for (int i = 0; i < 9; i++)
                            //    {
                            //        for (int j = 0; j < 9; j++)
                            //        {
                            //            numbut = Game.n[i, j].ToString();

                            //            lana.Write(numbut);
                            //        }
                            //        lana.WriteLine();
                            //    }
                            //    lana.WriteLine($"mist={Game.mist}");
                            //    lana.WriteLine($"MMax={Game.mistMax}");
                            //    lana.WriteLine($"zero={Game.zero}");
                            //    //lana.WriteLine(":lalala");
                            //    lana.Close();
                            SaveSaved(numbut, Save, PathSavesName);
                            if (Game.CurrentInstance.Title == "Новая игра ")
                            {

                                Game.CurrentInstance.context.Text = "Сохранение успешно сохранено";
                            }
                            else { Game.CurrentInstance.context.Text = "Сохранение успешно скопированно"; }
                        }
                        else 
                        {
                            Game.Saved = false;
                            //GBMenu.Visibility = Visibility.Visible;
                            Game.ChSave = Save;
                            //MainWindow.CurrentInstance.Title = Game.ChSave;
                            new Game().Show();
                            SaveSaved(numbut, Save, PathSavesName);


                            MainWindow.CurrentInstance.Close();
                        }
                    }
                    else
                    {
                        if (InGame == true)
                        {
                            DataFunc.TempSaved(numbut, Save);
                            //if (Game.CurrentInstance.Title != "Новая игра ")
                            //{
                            //    Game.ChSave = Save;
                            //}
                            //TempSaved(numbut, Save);
                            //var PathSaveCopy = Directory.GetCurrentDirectory();
                            //PathSaveCopy += $@"\SudoData\Saves\{Save}-Copy.txt";
                            //var PathSave = Directory.GetCurrentDirectory();
                            //PathSave += $@"\SudoData\Saves\{Save}.txt";
                            //StreamWriter SaveCopy = new StreamWriter(PathSaveCopy);
                            //SaveCopy.WriteLine("карта");


                            //for (int i = 0; i < 9; i++)
                            //{
                            //    for (int j = 0; j < 9; j++)
                            //    {

                            //        numbut = Game.m[i, j].ToString();

                            //        SaveCopy.Write(numbut);
                            //    }
                            //    SaveCopy.WriteLine();
                            //}

                            //SaveCopy.WriteLine("проверка");
                            //for (int i = 0; i < 9; i++)
                            //{
                            //    for (int j = 0; j < 9; j++)
                            //    {
                            //        numbut = Game.n[i, j].ToString();

                            //        SaveCopy.Write(numbut);
                            //    }
                            //    SaveCopy.WriteLine();
                            //}
                            //SaveCopy.WriteLine($"mist={Game.mist}");
                            //SaveCopy.WriteLine($"MMax={Game.mistMax}");
                            //SaveCopy.WriteLine($"zero={Game.zero}");
                            ////lana.WriteLine(":lalala");
                            //SaveCopy.Close();
                            //File.Delete(PathSave);
                            //File.Move(PathSaveCopy, PathSave);
                            //if (Game.CurrentInstance.Title == "Новая игра ")
                            //{
                            //    Game.CurrentInstance.Title = Save;
                            //}
                        }
                    }
                }
                //else { this.Close(); }
            }
            else 
            {
                if (InGame == true)
                {
                text.Focus();
                    Game.CurrentInstance.context.Text = " Поле для названия пустое";
                }
                else 
                {
                    DataFunc.ERROR = 7;
                    MainWindow.CurrentInstance.Delay2();
                }
            }
        }
        public static void TempSaved(string numbut, string Save) 
        {
            InTemp=true;
            DataFunc.CheckDirectory();
            string first = "";
            string sec = "";
            double check;
            var PathSaveCopy = Directory.GetCurrentDirectory();
            PathSaveCopy += $@"\SudoData\Saves\{Save}-Copy.txt";
            var PathSave = Directory.GetCurrentDirectory();
            PathSave += $@"\SudoData\Saves\{Save}.txt";
            StreamWriter SaveCopy = new StreamWriter(PathSaveCopy);
            SaveCopy.WriteLine("карта");


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    numbut = Game.m[i, j].ToString();

                    SaveCopy.Write(numbut);
                }
                SaveCopy.WriteLine();
            }

            SaveCopy.WriteLine("проверка");
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    numbut = Game.n[i, j].ToString();

                    SaveCopy.Write(numbut);
                }
                SaveCopy.WriteLine();
            }
            SaveCopy.WriteLine($"mist={Game.mist}");
            SaveCopy.WriteLine($"MMax={Game.mistMax}");
            SaveCopy.WriteLine($"zero={Game.zero}");
            //lana.WriteLine(":lalala");
            first = (Game.n[0, 0] + Game.n[0, 1] + Game.n[1, 0] + Game.n[1, 1]).ToString();
            sec = (Game.n[7, 7] + Game.n[7, 8] + Game.n[8, 7] + Game.n[8, 8]).ToString();
            check = (Convert.ToDouble(first) + Convert.ToDouble(Game.mist)) / Convert.ToDouble(sec);
            check=Math.Round(check,5);
            SaveCopy.WriteLine($"check={check}");
            SaveCopy.Close();
            File.Delete(PathSave);
            File.Move(PathSaveCopy, PathSave);
            if (Game.CurrentInstance.Title == "Новая игра ")
            {
                Game.CurrentInstance.Title = Save;
                if (DataFunc.WinMod == true) 
                {
                    Game.CurrentInstance.MenuSaveName.Header = Game.CurrentInstance.Title;
                }
            }
        }
        public static void SaveSaved( string numbut, string Save, string PathSavesName) 
        {

            StreamWriter SavesNameIN = new StreamWriter(PathSavesName, true);
            SavesNameIN.WriteLine(Save);
            SavesNameIN.Close();
            string first = "";
            string sec = "";
            double check ;
            var PathSave = Directory.GetCurrentDirectory();
            PathSave += $@"\SudoData\Saves\{Save}.txt";
            StreamWriter lana = new StreamWriter(PathSave);
            //lana.WriteLine(testmg);
            lana.WriteLine("карта");


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    numbut = Game.m[i, j].ToString();

                    lana.Write(numbut);
                }
                lana.WriteLine();
               
            }

            lana.WriteLine("проверка");
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    numbut = Game.n[i, j].ToString();

                    lana.Write(numbut);
                }
                lana.WriteLine();
            }
            lana.WriteLine($"mist={Game.mist}");
            lana.WriteLine($"MMax={Game.mistMax}");
            lana.WriteLine($"zero={Game.zero}");
            first = (Game.n[0,0]+Game.n[0, 1] + Game.n[1, 0]+ Game.n[1, 1]).ToString();
            sec = (Game.n[7, 7] + Game.n[7, 8] + Game.n[8,7] + Game.n[8, 8]).ToString();
            check=(Convert.ToDouble(first)+ Convert.ToDouble(Game.mist))/ Convert.ToDouble(sec);
            check = Math.Round(check, 5);
            lana.WriteLine($"check={check}");
            //lana.WriteLine(Game.n[0,0].ToString(), Game.n[0, 1].ToString(), Game.n[0, 2].ToString());
            lana.Close();
            if (Game.CurrentInstance.Title == "Новая игра ")
            {
                Game.CurrentInstance.Title = Save;
                if (DataFunc.WinMod == true)
                {
                    Game.CurrentInstance.MenuSaveName.Header = Game.CurrentInstance.Title;
                }
            }
        }
            public static void CheckDirectory() 
        {
            var SudoData = Directory.GetCurrentDirectory();
            SudoData += $@"\SudoData";
            var PathSave = Directory.GetCurrentDirectory();
            PathSave += $@"\SudoData\Saves";
            var PathSavesName = Directory.GetCurrentDirectory();
            PathSavesName += $@"\SudoData\SavesNameOnly\SavesName.txt";
            var PathSavesNameDir = Directory.GetCurrentDirectory();
            PathSavesNameDir += $@"\SudoData\SavesNameOnly";
            if (!File.Exists(PathSave))
            {
                // Если файл не существует, создаем его
                Directory.CreateDirectory(PathSave);

                //MessageBox.Show("lala");
            }
            if (!File.Exists(SudoData))
            {
                // Если файл не существует, создаем его
                Directory.CreateDirectory(SudoData);
                //MessageBox.Show("lala");
            }
            if (!File.Exists(PathSavesNameDir))
            {
                // Если файл не существует, создаем его
                Directory.CreateDirectory(PathSavesNameDir);
                //MessageBox.Show("lala");
            }
            if (!File.Exists(PathSavesName))
            {
                // Если файл не существует, создаем его
                File.Create(PathSavesName).Close();
                if (InTemp == true) { StreamWriter SN = new StreamWriter(PathSavesName);SN.WriteLine(Game.CurrentInstance.Title);SN.Close(); }
                //MessageBox.Show("lala");
            }
        }
    }

}
