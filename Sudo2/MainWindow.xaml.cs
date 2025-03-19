using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;


namespace Sudo2
{
    public partial class MainWindow : Window
    {
        //MainWindow secondWindow = new MainWindow();
        private ContextMenu secondWindow;
        public static MainWindow CurrentInstance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Icon=null;
            CurrentInstance = this; // Сохраняем ссылку на текущий экземпляр
            //Добавляем контекстное окно и задаем параметры
            secondWindow = new ContextMenu();

            secondWindow.Left = SystemParameters.PrimaryScreenWidth/2+this.Width/2;
            secondWindow.Top = 0;
            secondWindow.Top= SystemParameters.PrimaryScreenHeight/2-this.Height/2;
            //проверяем наличие сохранений
            if (DataFunc.AutoSave == true)
            {
                MenuAutoSave.IsChecked = true;
            }
            CheckSaveSpace(LBAdd);
            var PathSavesName = Directory.GetCurrentDirectory();
            PathSavesName += $@"\SudoData\SavesNameOnly\SavesName.txt";
            if (File.Exists(PathSavesName))
            {
                // Получаем размер файла
                long fileSize = new FileInfo(PathSavesName).Length;

                if (fileSize >= 1)
                {
                    RsBt.IsEnabled = true;
                }

            }
            if (Game.FileError == true) 
            {
                if (!secondWindow.IsVisible)
                {
                    DataFunc.ERROR = 6;
                    secondWindow.Show();
                    Delay();
                }
                Game.FileError =false;
            }
        }
        //кнопки взаимодействия (все)
       
        private void Click(object sender, RoutedEventArgs e)
        {
            var Bt = sender as Button;
            var ChItem = LBSave.SelectedItem;
            switch (Bt.Name)
                {
                case "NgBt":
                    GBComp.Visibility = Visibility.Visible;
                    GoGameBt.IsEnabled = false;
                    GBMenu.Visibility=Visibility.Collapsed;
                    VBRCPOut.Margin = new Thickness(15, 0, 170, 55); VBGoGame.Margin = new Thickness(170, 0, 15, 55); BFree.Visibility = Visibility.Hidden;
                    Game.zero = 0;
                    Game.mistMax = 0;
                    TBZeros.Text = "Zeros";
                    TBMistakes.Text = "Mistakes";
                    break;
                case "RsBt":
                    GBResm.Visibility = Visibility;
                    RsGoGameBt.IsEnabled=false;
                    GBMenu.Visibility = Visibility.Collapsed;
                    VBRsOut.Margin = new Thickness(15, 0, 170, 55); VBRsGoGame.Margin = new Thickness(170, 0, 15, 55); VBItDel.Visibility = Visibility.Collapsed;
                    LBAdd=true;
                    CheckSaveSpace(LBAdd);
                    //var PathSavesName = Directory.GetCurrentDirectory();
                    //PathSavesName += $@"\SudoData\SavesNameOnly\SavesName.txt";
                    //var TempPathSavesName = Directory.GetCurrentDirectory();
                    //TempPathSavesName += $@"\SudoData\SavesNameOnly\TempSavesName.txt";
                    //bool Space=false;
                    //StreamReader SavesNameOUT = new StreamReader(PathSavesName);
                    //StreamWriter TempSaveName = new StreamWriter(TempPathSavesName);
                    //string line;

                    //while ((line = SavesNameOUT.ReadLine()) != null)
                    //{
                    //    if (!string.IsNullOrWhiteSpace(line))
                    //    {
                    //        LBSave.Items.Add(line);
                    //        TempSaveName.WriteLine(line);
                    //    }
                    //    else { Space = true; }
                    //    //else 
                    //    //{
                    //    //    StreamWriter SavesNameON=new StreamWriter(PathSavesName,true);
                            
                    //    //}
                    //}
                    //        TempSaveName.Close();
                    //SavesNameOUT.Close();
                    //if (Space == true)
                    //{
                    //    File.Delete(PathSavesName);
                    //    File.Move(TempPathSavesName, PathSavesName);
                    //}
                    //else { File.Delete(TempPathSavesName); }
                    break;
                case "HpBt":
                    GBHelp.Visibility = Visibility.Visible;
                    GBMenu.Visibility = Visibility.Collapsed;
                    break;
                case "HpOutBt":
                    GBHelp.Visibility = Visibility.Collapsed;
                    GBMenu.Visibility = Visibility.Visible;
                    break;
                case "PCOutBt":
                    GBComp.Visibility = Visibility.Collapsed;
                    GBMenu.Visibility = Visibility.Visible;
                    RBLig.IsChecked = false;
                    RBDif.IsChecked = false;
                    RBHar.IsChecked = false;
                    RBFree.IsChecked = false;
                    break ;
                case "GoGameBt":
                    if (DataFunc.AutoSave == false)
                    {
                    Game.Saved = false;
                    //GBMenu.Visibility = Visibility.Visible;
                    new Game().Show();
                        this.Close();

                    }
                    else 
                    {
                        if (!secondWindow.IsVisible)
                        {
                            //GoGameBt.IsEnabled = false;
                            PCOutBt.Visibility = Visibility.Collapsed;
                            GoGameBt.Visibility = Visibility.Collapsed;
                            secondWindow.AutoSave.Visibility = Visibility.Visible;
                            secondWindow.Show();
                            secondWindow.text.Focus(); secondWindow.text.SelectAll();
                        }
                    }
                    //GoGameBt.IsEnabled = false;
                    break;
                case "RsOutBt":
                    GBResm.Visibility = Visibility.Collapsed;
                    GBMenu.Visibility = Visibility.Visible;
                    LBSave.Items.Clear();
                    //testik.Text=LBSave.SelectedIndex.ToString();

                    break;
                case "RsGoGameBt":
                    GBMenu.Visibility = Visibility.Visible;

                    if (ChItem != null)
                    {
                        Game.ChSave = ChItem.ToString();
                       
                        
                        if (ErrorChar == false)
                        {
                            //MessageBox.Show("piska");
                            var PathSave = Directory.GetCurrentDirectory();
                            PathSave += $@"\SudoData\Saves\{Game.ChSave}.txt";
                            if (File.Exists(PathSave))
                            {
                                StreamReader test = new StreamReader(PathSave);
                                string line1;
                                int localzero = 0;
                                Game.zero = 0;
                               bool SaveChoosen=false;
                            
                          
                            //    int LineMap = 0;
                            //    int LineProv = 10;
                            //int LineMist = 20;
                            //    int LineMistMax = 21;
                            //    int LineZero = 22;
                                
                                int[] m = new int[81];
                                int[] n = new int[81];
                                string jaja;
                                gigi = "";
                                prov = "";


                                //int t = 0;

                                string lineContentMap = File.ReadLines(PathSave).ElementAtOrDefault(0);
                                string lineContentProv = File.ReadLines(PathSave).ElementAtOrDefault(10);
                                string lineContentMist = File.ReadLines(PathSave).ElementAtOrDefault(20);
                                lineContentMist = lineContentMist.Substring(0,5);
                                string lineContentMMax = File.ReadLines(PathSave).ElementAtOrDefault(21);
                                lineContentMMax = lineContentMMax.Substring(0, 5);
                                string lineContentzero = File.ReadLines(PathSave).ElementAtOrDefault(22);
                                lineContentzero = lineContentzero.Substring(0, 5);

                                if (lineContentMap != "карта" || lineContentProv != "проверка"|| lineContentMist!="mist="|| lineContentMMax!="MMax="|| lineContentzero!="zero=")
                                {
                                    //   Game.Saved = true;
                                    //   new Game().Show();
                                    //this.Close();
                                    SaveChoosen = true;
                                    //MessageBox.Show(lineContentMMax);

                                }
                                else
                                {
                                    while ((line1 = test.ReadLine()) != null)
                                    {
                                        if (line1 == "карта")
                                        {
                                            for (int i = 0; i < 9; i++)
                                            {
                                                jaja = test.ReadLine();
                                                gigi += jaja;
                                            }
                                        }
                                        if (line1 == "проверка")
                                        {
                                            for (int i = 0; i < 9; i++)
                                            {
                                                jaja = test.ReadLine();
                                                prov += jaja;

                                            }
                                        }
                                        if (line1.Substring(0, 4) == "mist")
                                        {
                                            jaja = line1.Substring(5);
                                            //context.Text = jaja;
                                            Game.mist = Convert.ToInt32(jaja);

                                        }
                                        if (line1.Substring(0, 4) == "MMax")
                                        {
                                            jaja = line1.Substring(5);
                                            //context.Text = jaja;
                                            Game.mistMax = Convert.ToInt32(jaja);

                                        }
                                        if (line1.Substring(0, 4) == "zero")
                                        {
                                            jaja = line1.Substring(5);
                                            //context.Text = jaja;
                                            localzero = Convert.ToInt32(jaja);

                                        }
                                    }
                                    //РАБОТАЕТ
                                    foreach (char c in prov)
                                    {

                                        int p = Convert.ToInt32(c.ToString());
                                        if (p == 0)
                                        {
                                            SaveChoosen = true;
                                            //MessageBox.Show("PISA");

                                        }
                                    }
                                    //РАБОТАЕТ НО ДОДЕЛАТЬ
                                    for (int i = 0; i < 81; i++)
                                    {



                                        m[i] = Convert.ToInt32(gigi[i].ToString());
                                        n[i] = Convert.ToInt32(prov[i].ToString());

                                        if (gigi[i] == '0') { Game.zero++; }
                                        if (m[i] != 0) if (m[i] != n[i])
                                            {
                                                SaveChoosen = true;
                                                //MessageBox.Show(NoMistStep.ToString());
                                                break;
                                            }
                                


                                    }

                                    if (Game.mist > Game.mistMax || Game.mist < 0 || Game.mist == Game.mistMax || Game.mistMax >= 10000 || Game.mistMax <= 0 || localzero <= 0 || localzero >= 81 || localzero != Game.zero || Game.zero == 0)
                                    {
                                        SaveChoosen = true;
                                        //MessageBox.Show(localzero.ToString());
                                    
                                    }

                                    test.Close();

                                }
                                if (SaveChoosen == false)
                                {
                                    Game.Saved = true;
                                    new Game().Show();
                                    Game.CurrentInstance.Title = Game.ChSave;
                                    this.Close();

                                }
                                else 
                                {
                                    if (!secondWindow.IsVisible)
                                    {
                                        DataFunc.ERROR = 6;
                                        secondWindow.Show();
                                        Delay();
                                    }
                                }


                            }
                            else
                            {
                                //RsBt.IsEnabled = true;
                                if (!secondWindow.IsVisible)
                                {
                                    DataFunc.ERROR = 2;
                                    secondWindow.Show();
                                    Delay();
                                }
                                //else if (secondWindow.IsVisible & secondWindow.AutoSave.IsVisible == true) 
                                //{
                                //this.Close();
                                //}
                            }
                        }
                        else 
                        {
                            //MessageBox.Show("piska");
                            if (!secondWindow.IsVisible)
                            {
                                DataFunc.ERROR = 5;
                                secondWindow.Show();

                                Delay();
                            }
                        }
                    }
                    break;
                case "ItDelBt":
                    //LBSave.SelectedItems.Clear();
                   
                    if (LBSave.Items.Count != 0)
                    {
                        //var ChItem = LBSave.SelectedItem;
                        string Wine;

                        var PathSavesNameDel = Directory.GetCurrentDirectory();
                        PathSavesNameDel += $@"\SudoData\SavesNameOnly\SavesName.txt";
                        var TempPathSavesNameDel = Directory.GetCurrentDirectory();
                        TempPathSavesNameDel += $@"\SudoData\SavesNameOnly\TempSavesName.txt";
                        var PathSave = Directory.GetCurrentDirectory();
                        PathSave += $@"\SudoData\Saves\{ChItem.ToString()}.txt";
                        StreamReader SavesNameRead = new StreamReader(PathSavesNameDel);
                        StreamWriter TempSavesNameWrite = new StreamWriter(TempPathSavesNameDel);
                        while ((Wine = SavesNameRead.ReadLine()) != null)

                        {
                            if (ChItem != null)
                            {
                                if (Wine != ChItem.ToString())
                                {
                                    TempSavesNameWrite.WriteLine(Wine);
                                }
                            }
                            //ОТМЕТКА ГДЕ НАДО УБРАТЬ СОХРАНЕНИЕ
                        }
                        SavesNameRead.Close();
                        TempSavesNameWrite.Close();
                        File.Delete(PathSavesNameDel);
                        File.Move(TempPathSavesNameDel, PathSavesNameDel);
                        if(ErrorChar==false)
                        File.Delete(PathSave);
                        LBSave.Items.RemoveAt(LBSave.SelectedIndex);
                        if (!secondWindow.IsVisible)
                        {
                            DataFunc.ERROR = 3;
                            secondWindow.Show();
                            Delay();
                        }
                    }
                    else MessageBox.Show("lox");
                    //this.Close();
                    break;
                   


            }
            
        }
        public static string gigi = "";
        public static string prov = "";
        bool LBAdd=false;
        public static void CheckSaveSpace(bool LocLBAdd) 
        {
            DataFunc.CheckDirectory();
            //var SudoData = Directory.GetCurrentDirectory();
            //SudoData += $@"\SudoData";
            //var PathSave = Directory.GetCurrentDirectory();
            //PathSave += $@"\SudoData\Saves";
            //var PathSavesName = Directory.GetCurrentDirectory();
            //PathSavesName += $@"\SudoData\SavesNameOnly\SavesName.txt";
            //var PathSavesNameDir = Directory.GetCurrentDirectory();
            //PathSavesNameDir += $@"\SudoData\SavesNameOnly";
            //if (!File.Exists(PathSave))
            //{
            //    // Если файл не существует, создаем его
            //    Directory.CreateDirectory(PathSave);
            //    //MessageBox.Show("lala");
            //}
            //if (!File.Exists(SudoData))
            //{
            //    // Если файл не существует, создаем его
            //    Directory.CreateDirectory(SudoData);
            //    //MessageBox.Show("lala");
            //}
            //if (!File.Exists(PathSavesNameDir))
            //{
            //    // Если файл не существует, создаем его
            //    Directory.CreateDirectory(PathSavesNameDir);
            //    //MessageBox.Show("lala");
            //}
            //if (!File.Exists(PathSavesName))
            //{
            //    // Если файл не существует, создаем его
            //    File.Create(PathSavesName).Close();
            //    //MessageBox.Show("lala");
            //}
            var PathSavesName = Directory.GetCurrentDirectory();
            PathSavesName += $@"\SudoData\SavesNameOnly\SavesName.txt";
            var TempPathSavesName = Directory.GetCurrentDirectory();
            TempPathSavesName += $@"\SudoData\SavesNameOnly\TempSavesName.txt";
            bool Space = false;
            StreamReader SavesNameOUT = new StreamReader(PathSavesName);
            StreamWriter TempSaveName = new StreamWriter(TempPathSavesName);
            string line;

            while ((line = SavesNameOUT.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (LocLBAdd == true)
                    {
                        
                        MainWindow.CurrentInstance.LBSave.Items.Add(line);
                    }
                    TempSaveName.WriteLine(line);
                }
                else { Space = true; }
                //else 
                //{
                //    StreamWriter SavesNameON=new StreamWriter(PathSavesName,true);

                //}
            }
            TempSaveName.Close();
            SavesNameOUT.Close();
            if (Space == true)
            {
                File.Delete(PathSavesName);
                File.Move(TempPathSavesName, PathSavesName);
            }
            else { File.Delete(TempPathSavesName); }
        }
     
        //радио кнопки меню выбора режима
        private void RB_Click(object sender, RoutedEventArgs e)
        {
            var Rb = sender as RadioButton;
            GoGameBt.IsEnabled = true;
            if (secondWindow.IsVisible /*& secondWindow.AutoSave.IsVisible*/)
            {
                secondWindow.BtAutoSaveGo.IsEnabled = true;
            }
            if (RBFree.IsChecked == false) { VBRCPOut.Margin = new Thickness(15, 0, 170, 55); VBGoGame.Margin = new Thickness(170, 0, 15, 55); BFree.Visibility = Visibility.Hidden; }
            switch (Rb.Name) 
            {
                case "RBLig":
                    Game.comp = 1;
                   
                    break;
                case "RBDif":
                    Game.comp = 2;
                 
                    break;
                case "RBHar":
                    Game.comp = 3;
           
                    break;
                case "RBFree":
                    Game.comp = 4;
                    VBRCPOut.Margin = new Thickness(15, 60, 170, -5);
                    VBGoGame.Margin = new Thickness(170,60,15,-5);
                    BFree.Visibility = Visibility;
                    GoGameBt.IsEnabled = false;
                    if (secondWindow.IsVisible /*& secondWindow.AutoSave.IsVisible*/)
                    {
                    secondWindow.BtAutoSaveGo.IsEnabled = false;
                    }
                    if (Game.mistMax != 0 & Game.zero != 0) { GoGameBt.IsEnabled = true; 
                        if (secondWindow.IsVisible /*& secondWindow.AutoSave.IsVisible*/)
                        {
                            secondWindow.BtAutoSaveGo.IsEnabled = true;
                        }
                    }
                        break;
            }
        }
        //Изменение текста

        public static bool ErrorChar=false;
        private void LBSave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RsGoGameBt.IsEnabled = true;
            ErrorChar = false;
            VBRsOut.Margin = new Thickness(15, 55, 170, 0);
            VBRsGoGame.Margin = new Thickness(170, 55, 15, 0);
            VBItDel.Visibility = Visibility;
            var ChItem = LBSave.SelectedItem;

            if (ChItem == null)
            {
                RsGoGameBt.IsEnabled = false;
                VBRsOut.Margin = new Thickness(15, 0, 170, 55); VBRsGoGame.Margin = new Thickness(170, 0, 15, 55); VBItDel.Visibility = Visibility.Hidden;

            }
            else
            { 
              char[] er = { '<', '>', ':', '"', '|', '/', '?', '\\', '*' };
                foreach (char ch in ChItem.ToString())
                {
                    for (int i = 0; i < er.Length; i++)
                    {
                        if (er[i] == ch)
                        {
                        
                            ErrorChar = true;
                            //MessageBox.Show("piska");
                            break;
                        } /*доделаааааааааааааааааааааааааааааааать*/
                    }
                }
            }
            var PathSavesName = Directory.GetCurrentDirectory();
            PathSavesName += $@"\SudoData\SavesNameOnly\SavesName.txt";
            if (File.Exists(PathSavesName))
            {
                // Получаем размер файла
                long fileSize = new FileInfo(PathSavesName).Length;

                if (fileSize >= 1)
                {
                    RsBt.IsEnabled = true;
                }
                else { RsBt.IsEnabled = false; }

            }
        }
        //мутки с нулями и ошибками
        private void Comp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var send = sender as TextBox;
            if (send.Name == "TBZeros")
            {
                if (TBZeros.Text == "Zeros")
                {
                    TBZeros.Clear();

                }
                else
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        e.Handled = true;
                        return;
                    }

                }
            }
            else 
            {
                if (TBMistakes.Text == "Mistakes")
                {
                    TBMistakes.Clear();

                }
                else
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        e.Handled = true;
                        return;
                    }
                    
                }
            }
        }

        private void Comp_TextChanged(object sender, TextChangedEventArgs e)
        {
            var send = sender as TextBox;
            if (send.Name == "TBZeros")
            {
                if (TBZeros.Text != "Zeros")
                {
                    if (TBZeros.Text != "")
                    {
                        int.TryParse(TBZeros.Text, out int LocalZero);
                        int t;
                        int tt=1000;
                        if (LocalZero == 0 || LocalZero >= 81)
                        {
                            int.TryParse(TBZeros.Text.Substring(TBZeros.Text.Length - 1),out t);
                            //MessageBox.Show(t.ToString());
                            if (t != 0)
                            {
                                int.TryParse(TBZeros.Text, out  tt);
                            }
                            TBZeros.Text = TBZeros.Text.Substring(0, TBZeros.Text.Length - 1);
                            //MessageBox.Show(tt.ToString());
                            //for (int i = 0; i < TBMistakes.Text.Length; i++)
                            //{
                            //    TBZeros.SelectionStart++;
                            //}
                            TBZeros.CaretIndex = TBZeros.Text.Length;
                            if (TBZeros.Text.Length != 0)
                            {
                                LocalZero = Convert.ToInt32(TBZeros.Text);
                            }

                            if (t == 0 )
                            {
                                DataFunc.ERROR = 1;
                                if (!secondWindow.IsVisible)
                                {
                                 
                                    secondWindow.Show();
                                    TBZeros.Focus();
                                    Delay();
                                }
                                else if (secondWindow.IsVisible & secondWindow.AutoSave.IsVisible)
                                {

                                    TBZeros.Focus();
                                    Delay2();
                                }
                            }
                            else if (tt > 80)
                            {
                                DataFunc.ERROR = 4;
                                if (!secondWindow.IsVisible)
                                {
                              
                                    secondWindow.Show();
                                    TBZeros.Focus();
                                    Delay();
                                }
                                else if (secondWindow.IsVisible & secondWindow.AutoSave.IsVisible)
                                {
                                    
                                    TBZeros.Focus();
                                    Delay2();
                                }
                            }
                        }
                
                        Game.zero = LocalZero;
                    }
                    else { Game.zero = 0; }
                }
            }
            else 
            {
                if (TBMistakes.Text != "Mistakes")
                {
                    if (TBMistakes.Text != "")
                    {
                        int t;
                        int tt = 100000;
                        int.TryParse(TBMistakes.Text, out int LocalMist);
                        if (LocalMist == 0||LocalMist>9999)
                        {
                            //int y = 0;
                            //foreach (char ch in TBMistakes.Text) 
                            //{
                            //if()
                            //}
                            int.TryParse(TBMistakes.Text.Substring(TBMistakes.Text.Length - 1), out t);
                            if (t != 0)
                            {
                                int.TryParse(TBMistakes.Text, out tt);
                            }
                            TBMistakes.Text = TBMistakes.Text.Substring(0, TBMistakes.Text.Length - 1);
                
                            TBMistakes.CaretIndex = TBMistakes.Text.Length;
                            if (TBMistakes.Text.Length != 0)
                            {
                                LocalMist = Convert.ToInt32(TBMistakes.Text);
                            }
                            if (t == 0)
                            {
                                    DataFunc.ERROR = 1;
                                if (!secondWindow.IsVisible)
                                {
                                    secondWindow.Show();
                                    TBMistakes.Focus();
                                    Delay();
                                }
                                else if (secondWindow.IsVisible & secondWindow.AutoSave.IsVisible)
                                {
                                    TBMistakes.Focus();
                                    Delay2();
                                }
                            }
                            else if (tt > 9999)
                            {
                                    DataFunc.ERROR = 4;
                                if (!secondWindow.IsVisible)
                                {
                                    secondWindow.Show();
                                    TBMistakes.Focus();
                                    Delay();
                                }
                                else if (secondWindow.IsVisible & secondWindow.AutoSave.IsVisible)
                                {
                                    TBMistakes.Focus();
                                    Delay2();
                                }
                            }
                        }
                        //if (Keyboard.Modifiers == ModifierKeys.Control) { TBMistakes.Text = "Mistakes"; }
                        Game.mistMax = LocalMist;
                    }
                    else { Game.mistMax = 0; }
                }
            }
                if (GoGameBt != null) if (Game.mistMax != 0 & Game.zero != 0) { GoGameBt.IsEnabled = true; }
                    else if (GoGameBt != null) if (Game.mistMax == 0 || Game.zero == 0) { GoGameBt.IsEnabled = false; }
            if (secondWindow != null)
            {
                if (secondWindow.IsVisible /*& secondWindow.AutoSave.IsVisible*/)
                {
                    if (Game.mistMax != 0 & Game.zero != 0)
                    {
                        secondWindow.BtAutoSaveGo.IsEnabled = true;
                    }
                    else
                    {
                        if (Game.mistMax == 0 || Game.zero == 0)
                        {
                            secondWindow.BtAutoSaveGo.IsEnabled = false;
                        }
                    }
                }
            }
        }
        //задержка
        public  async void Delay()
        {
            // ContextMenu secondWindow;
            //secondWindow = new ContextMenu();
            await Task.Delay(2000);
            secondWindow.Visibility = Visibility.Hidden;
        }
        // void asa()
        //{
        //    if (TBZeros.IsFocused & !TBMistakes.IsFocused & !secondWindow.text.Focusable)
        //    {
        //        MessageBox.Show("ljlj");
        //    }
        //         else if (TBMistakes.IsFocused & !TBZeros.IsFocused & !secondWindow.text.Focusable)
        //    {
        //        MessageBox.Show("12313aaaaaaaaaaaaaaaaaaa");
        //    }
        //    else if (secondWindow.text.Focusable & !TBZeros.IsFocused & !TBMistakes.IsFocused)
        //    {
        //        MessageBox.Show("12313");
        //    }
        //    if (secondWindow.text.Focusable == true)
        //    {
        //        //SecWinFoc = true;
        //        MessageBox.Show("ljlj");
        //    }
        //}
        public async void Delay2()
        {
  


            secondWindow.AutoSave.Visibility = Visibility.Collapsed;
            await Task.Delay(2000);
            secondWindow.AutoSave.Visibility = Visibility;
   
            if (DataFunc.TextFocus == true) { secondWindow.text.Focus(); DataFunc.TextFocus = false; }
        }
        //привязка контекстного окна к закрытию меню
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //new ContextMenu();
            secondWindow.Close();
            //MessageBox.Show("ldld");
        }

        private void check(object sender, RoutedEventArgs e)
        {
            if (DataFunc.AutoSave == true)
            {
                if (secondWindow.IsVisible & secondWindow.AutoSave.IsVisible)
                {
                    secondWindow.AutoSave.Visibility = Visibility.Collapsed;
                    secondWindow.Visibility= Visibility.Hidden;
                }
                //MessageBox.Show("1");
                PCOutBt.Visibility= Visibility.Visible;
                GoGameBt.Visibility= Visibility.Visible;
                MenuAutoSave.IsChecked = false;
                DataFunc.AutoSave=false;
            }
            else 
            {
                //MessageBox.Show("0");
                DataFunc.AutoSave = true;
                MenuAutoSave.IsChecked = true;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            //if (secondWindow != null)
            //{
            //this.Close
            //}
            secondWindow.WindowState=this.WindowState;
        }
    }
   
}
