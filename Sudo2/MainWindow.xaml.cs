using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace Sudo2
{
    public partial class MainWindow : Window
    {
       //задаем ссылки
        private ContextMenu secondWindow;
        public static MainWindow CurrentInstance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            // Сохраняем ссылку на текущий экземпляр
            CurrentInstance = this; 
            //Добавляем контекстное окно и задаем параметры
            secondWindow = new ContextMenu();

            secondWindow.Left = SystemParameters.PrimaryScreenWidth/2+this.Width/2;
            secondWindow.Top = 0;
            secondWindow.Top= SystemParameters.PrimaryScreenHeight/2-this.Height/2;
            //проверяем наличие режима сохранения
            if (DataFunc.AutoSave == true)
            {
                MenuAutoSave.IsChecked = true;
            }
            //если нужных директорий нет, то создаем ее
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
            if (DataFunc.FileError == true) 
            {
                if (!secondWindow.IsVisible)
                {
                    DataFunc.ERROR = 6;
                    secondWindow.Show();
                    Delay();
                }
                DataFunc.FileError =false;
            }
            //TBZeros.Text = "Количество нулей";
            display();
        }
        //кнопки взаимодействия (все)
       
        private void Click(object sender, RoutedEventArgs e)
        {
            var Bt = sender as Button;
            var ChItem = LBSave.SelectedItem;
            string name;
            switch (Bt.Name)
                {
                //новая игра
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
                    //меню сохранений
                case "RsBt":
                    GBResm.Visibility = Visibility;
                    RsGoGameBt.IsEnabled=false;
                    GBMenu.Visibility = Visibility.Collapsed;
                    VBRsOut.Margin = new Thickness(15, 0, 170, 55); VBRsGoGame.Margin = new Thickness(170, 0, 15, 55); VBItDel.Visibility = Visibility.Collapsed;
                    LBAdd=true;
                    CheckSaveSpace(LBAdd);
                    break;
                    //помощь
                case "HpBt":
                    GBHelp.Visibility = Visibility.Visible;
                    GBMenu.Visibility = Visibility.Collapsed;
                    break;
                    //выйти из помощи
                case "HpOutBt":
                    GBHelp.Visibility = Visibility.Collapsed;
                    GBMenu.Visibility = Visibility.Visible;
                    scroll.ScrollToHome();
                    break;
                    //выйти из новой игры
                case "PCOutBt":
                    name = "PCOutBt";
                    ClickOrBut(name); /*это всего лишь проверка ради хайпа*/
                    //GBComp.Visibility = Visibility.Collapsed;
                    //GBMenu.Visibility = Visibility.Visible;
                    //RBLig.IsChecked = false;
                    //RBDif.IsChecked = false;
                    //RBHar.IsChecked = false;
                    //RBFree.IsChecked = false;
                    break ;
                    //старт
                case "GoGameBt":
                     name = "GoGameBt";
                    ClickOrBut(name);
                    //if (DataFunc.AutoSave == false)
                    //{
                    //Game.Saved = false;
                    ////GBMenu.Visibility = Visibility.Visible;
                    //new Game().Show();
                    //    this.Close();

                    //}
                    //else 
                    //{
                    //    if (!secondWindow.IsVisible)
                    //    {
                    //        //GoGameBt.IsEnabled = false;
                    //        PCOutBt.Visibility = Visibility.Collapsed;
                    //        GoGameBt.Visibility = Visibility.Collapsed;
                    //        secondWindow.AutoSave.Visibility = Visibility.Visible;
                    //        secondWindow.Show();
                    //        secondWindow.text.Focus(); secondWindow.text.SelectAll();
                    //    }
                    //}
                    //GoGameBt.IsEnabled = false;
                    break;
                    //выйти из меню сохранений
                case "RsOutBt":
                    GBResm.Visibility = Visibility.Collapsed;
                    GBMenu.Visibility = Visibility.Visible;
                    LBSave.Items.Clear();
                    break;
                    //загрузить сохранение
                case "RsGoGameBt":

                    GBMenu.Visibility = Visibility.Visible;
                   DataFunc.CheckDirectory();

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
                                DataFunc.IsReadOnly(PathSave);
                                if (DataFunc.ReadOnly == false)
                                {
                                    StreamReader test = new StreamReader(PathSave);
                                    string line1;
                                    int localzero = 0;
                                    Game.zero = 0;
                                    double localcheck = 0;
                                    bool SaveChoosen = false;
                                    int[,] m = new int[9, 9];
                                    int[,] n = new int[9, 9];
                                    string jaja;
                                    gigi = "";
                                    prov = "";
                                    string lineContentMap = File.ReadLines(PathSave).ElementAtOrDefault(0);
                                    string lineContentProv = File.ReadLines(PathSave).ElementAtOrDefault(10);
                                    string lineContentMist = File.ReadLines(PathSave).ElementAtOrDefault(20);
                                    //if(lineContentMist)
                                    if (!string.IsNullOrWhiteSpace(lineContentMist))
                                    {
                                        lineContentMist = lineContentMist.Substring(0, 5);
                                    }
                                    //else { SaveChoosen = true; }
                                    string lineContentMMax = File.ReadLines(PathSave).ElementAtOrDefault(21);
                                    if (!string.IsNullOrWhiteSpace(lineContentMMax))
                                    {
                                        lineContentMMax = lineContentMMax.Substring(0, 5);
                                    }
                                    string lineContentzero = File.ReadLines(PathSave).ElementAtOrDefault(22);
                                    if (!string.IsNullOrWhiteSpace(lineContentzero))
                                    {
                                        lineContentzero = lineContentzero.Substring(0, 5);
                                    }
                                    string lineContentcheck = File.ReadLines(PathSave).ElementAtOrDefault(23);
                                    if (!string.IsNullOrWhiteSpace(lineContentcheck))
                                    {
                                        lineContentcheck = lineContentcheck.Substring(0, 6);
                                    }
                                    if (lineContentMap != "карта" || lineContentProv != "проверка" || lineContentMist != "mist=" || lineContentMMax != "MMax=" || lineContentzero != "zero=" || lineContentcheck != "check=")
                                    {
                                        SaveChoosen = true;
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
                                                //if(int.TryParse jaja,out q){ }
                                                if (!int.TryParse(jaja, out int q))
                                                {
                                                    SaveChoosen = true;
                                                    break;
                                                    //jaja = "666666";
                                                }
                                                //context.Text = jaja;
                                                if (jaja.Length >= 6)
                                                {
                                                    SaveChoosen = true;
                                                    break;
                                                }
                                                Game.mist = Convert.ToInt32(jaja);

                                            }
                                            if (line1.Substring(0, 4) == "MMax")
                                            {
                                                jaja = line1.Substring(5);
                                                if (!int.TryParse(jaja, out int q))
                                                {
                                                    SaveChoosen = true;
                                                    break;
                                                }
                                                //context.Text = jaja;
                                                if (jaja.Length >= 6)
                                                {
                                                    SaveChoosen = true;
                                                    //MessageBox.Show("asda");
                                                    break;
                                                }
                                                Game.mistMax = Convert.ToInt32(jaja);

                                            }
                                            if (line1.Substring(0, 4) == "zero")
                                            {
                                                jaja = line1.Substring(5);
                                                if (!int.TryParse(jaja, out int q))
                                                {
                                                    SaveChoosen = true;
                                                    break;
                                                }
                                                //context.Text = jaja;
                                                if (jaja.Length >= 3)
                                                {
                                                    SaveChoosen = true;
                                                    break;
                                                }
                                                localzero = Convert.ToInt32(jaja);

                                            }
                                            if (line1.Substring(0, 5) == "check")
                                            {
                                                jaja = line1.Substring(6);
                                                if (!double.TryParse(jaja, out double q))
                                                {
                                                    SaveChoosen = true;
                                                    break;
                                                }
                                                //context.Text = jaja;
                                                if (jaja.Length >= 8)
                                                {
                                                    SaveChoosen = true;
                                                    break;
                                                }
                                                localcheck = Convert.ToDouble(jaja);

                                            }
                                        }
                                        //РАБОТАЕТ
                                        //int.TryParse(gigi, out int cc);
                                        //int.TryParse(prov, out int ccc);
                                        //if (cc!=0&ccc!=0)
                                        //{
                                        if (SaveChoosen == false)
                                        {
                                            foreach (char c in gigi)
                                            {

                                                //int p = Convert.ToInt32(c.ToString());
                                                if (!int.TryParse(c.ToString(), out int p))
                                                {
                                                    SaveChoosen = true;
                                                    break;
                                                    //MessageBox.Show("PISA");

                                                }
                                            }
                                            foreach (char c in prov)
                                            {
                                                int.TryParse(c.ToString(), out int p);
                                                //int p = Convert.ToInt32(c.ToString());
                                                if (p == 0)
                                                {
                                                    SaveChoosen = true;
                                                    break;
                                                    //MessageBox.Show("PISA");

                                                }
                                            }
                                        }

                                        if (SaveChoosen == false)
                                        {
                                            int h = 0;
                                            int row = 0;
                                            int col = 0;
                                            for (int i = 0; i < 9; i++)
                                            {
                                                for (int j = 0; j < 9; j++)
                                                {
                                                    m[i, j] = Convert.ToInt32(gigi[h].ToString());
                                                    n[i, j] = Convert.ToInt32(prov[h].ToString());

                                                    if (gigi[h] == '0') { Game.zero++; }
                                                    if (m[i, j] != 0) if (m[i, j] != n[i, j])
                                                        {
                                                            SaveChoosen = true;
                                                            break;
                                                        }
                                                    row += n[i, j];
                                                    if ((h + 1) % 9 == 0)
                                                    {
                                                        if (row != 45)
                                                        {
                                                            SaveChoosen = true;
                                                            break;
                                                        }
                                                    }
                                                    h++;
                                                }
                                                row = 0;
                                            }
                                            h = 0;
                                            for (int i = 0; i < 9; i++)
                                            {
                                                for (int j = 0; j < 9; j++)
                                                {
                                                    col += n[j, i];
                                                    if ((h + 1) % 9 == 0)
                                                    {
                                                        if (col != 45)
                                                        {
                                                            SaveChoosen = true;
                                                            break;
                                                        }
                                                    }
                                                    h++;
                                                }
                                                col = 0;
                                            }
                                        }
                                        if (SaveChoosen == false)
                                        {
                                            int first = n[0, 0] + n[0, 1] + n[1, 0] + n[1, 1];
                                            int sec = n[7, 7] + n[7, 8] + n[8, 7] + n[8, 8];
                                            double check = (Convert.ToDouble(first) + Convert.ToDouble(Game.mist)) / (Convert.ToDouble(sec) + Convert.ToDouble(Game.mistMax));
                                            check = Math.Round(check, 5);
                                            if (localcheck != check)
                                            {
                                                SaveChoosen = true;
                                            }
                                        }
                                        if (Game.mist > Game.mistMax || Game.mist < 0 || Game.mist == Game.mistMax || Game.mistMax >= 10000 || Game.mistMax <= 0 || localzero <= 0 || localzero >= 81 || localzero != Game.zero || Game.zero == 0)
                                        {
                                            SaveChoosen = true;
                                        }
                                        test.Close();
                                    }
                                    if (SaveChoosen == false)
                                    {
                                        DataFunc.Saved = true;
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
                                    if (!secondWindow.IsVisible)
                                    {
                                        DataFunc.ERROR = 10;
                                        secondWindow.Show();
                                        Delay();
                                    }
                                }
                            }
                            else
                            {
                                if (!secondWindow.IsVisible)
                                {
                                    DataFunc.ERROR = 2;
                                    secondWindow.Show();
                                    Delay();
                                }
                            }
                        }
                        else 
                        {
                            if (!secondWindow.IsVisible)
                            {
                                DataFunc.ERROR = 5;
                                secondWindow.Show();
                                Delay();
                            }
                        }
                    }
                    break;
                    //удалить сохранение
                case "ItDelBt":
                    if (LBSave.Items.Count != 0)
                    {
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
                        if (ErrorChar == false)
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
                    break;
                   


            }
            
        }
        public static string gigi = ""; /*для записи нашей карты*/
        public static string prov = ""; /*для записи нашей базы*/
        bool LBAdd=false; /*для директорий*/
        //читаем названия сохранений
        public static void CheckSaveSpace(bool LocLBAdd) 
        {
            DataFunc.CheckDirectory();
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

        public static bool ErrorChar=false; /*неправильные название*/
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
                        } 
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
        //Кдаление первоначального текста
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
        //проверка полей свободного реждима
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
                            if (t != 0)
                            {
                                int.TryParse(TBZeros.Text, out  tt);
                            }
                            TBZeros.Text = TBZeros.Text.Substring(0, TBZeros.Text.Length - 1);
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
            await Task.Delay(2000);
            secondWindow.Visibility = Visibility.Hidden;
        }
        //задержка 2
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
            secondWindow.Close();
        }
        //проверка меню автосейв
        private void check(object sender, RoutedEventArgs e)
        {
            if (DataFunc.AutoSave == true)
            {
                if (secondWindow.IsVisible & secondWindow.AutoSave.IsVisible)
                {
                    secondWindow.AutoSave.Visibility = Visibility.Collapsed;
                    secondWindow.Visibility= Visibility.Hidden;
                }
                PCOutBt.Visibility= Visibility.Visible;
                GoGameBt.Visibility= Visibility.Visible;
                MenuAutoSave.IsChecked = false;
                DataFunc.AutoSave=false;
            }
            else 
            {
                DataFunc.AutoSave = true;
                MenuAutoSave.IsChecked = true;
            }
        }
        //скрытие контекстного окна при скрытии мейна
        private void Window_StateChanged(object sender, EventArgs e)
        {
            secondWindow.WindowState=this.WindowState;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string name;
                switch (true) 
                {
         
                    case bool _ when GoGameBt.IsVisible && GoGameBt.IsEnabled:
                        //MessageBox.Show("lala1");
                         name = "GoGameBt";
                        
                        ClickOrBut(name);

                        break;
                    case bool _ when secondWindow.BtAutoSaveGo.IsVisible && secondWindow.BtAutoSaveGo.IsEnabled:
                        MessageBox.Show("lal2");
                        break;
                    case bool _ when RsGoGameBt.IsVisible && RsGoGameBt.IsEnabled:
                        name = "RsGoGameBt";
                        ClickOrBut(name);
                        break;
                }
            }
            else if (e.Key == Key.Escape) 
            {
                string name;
                switch (true)
                {

                    case bool _ when PCOutBt.IsVisible && PCOutBt.IsEnabled:
                        name = "PCOutBt";
                        ClickOrBut(name);
                        break;
                }
                }
        }
        static void ClickOrBut(string name) 
        {
            switch (name) 
            {
                case "GoGameBt":
                    if (DataFunc.AutoSave == false)
                    {
                        DataFunc.Saved = false;
                        //GBMenu.Visibility = Visibility.Visible;
                        new Game().Show();
                        MainWindow.CurrentInstance.Close();

                    }
                    else
                    {
                        if (!MainWindow.CurrentInstance.secondWindow.IsVisible)
                        {
                            //GoGameBt.IsEnabled = false;
                            MainWindow.CurrentInstance.PCOutBt.Visibility = Visibility.Collapsed;
                            MainWindow.CurrentInstance.GoGameBt.Visibility = Visibility.Collapsed;
                            MainWindow.CurrentInstance.secondWindow.AutoSave.Visibility = Visibility.Visible;
                            MainWindow.CurrentInstance.secondWindow.Show();
                            MainWindow.CurrentInstance.secondWindow.text.Focus(); MainWindow.CurrentInstance.secondWindow.text.SelectAll();
                        }
                    }
                    break;
                case "PCOutBt":
                    MainWindow.CurrentInstance.GBComp.Visibility = Visibility.Collapsed;
                    MainWindow.CurrentInstance.GBMenu.Visibility = Visibility.Visible;
                    MainWindow.CurrentInstance.RBLig.IsChecked = false;
                    MainWindow.CurrentInstance.RBDif.IsChecked = false;
                    MainWindow.CurrentInstance.RBHar.IsChecked = false;
                    MainWindow.CurrentInstance.RBFree.IsChecked = false;
                    break;
            }
        }
        //вывод помощи
      public void display() 
        {
            quan.Document.Blocks.Clear();

            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run("      Правила игры\r\n\r\n")
            {
                FontSize = 30,
                Foreground = Brushes.Yellow,
                
            });
            paragraph.Inlines.Add(new Run("Поле судоку ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("- 9 квадратов 3 на 3, которые образуют квадрат 9 на 9.\r\rВ маленьком квадрате ниходятся числа ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            //paragraph.Inlines.Add(new Run("В каждой строке и столбце так же находятся числа от ")
            //{
            //    FontSize = 19,
            //    Foreground = Brushes.White,

            //});
            paragraph.Inlines.Add(new Run("от 1 до 9 ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("включительно.\r\rВ каждой строке и столбце так же находятся числа ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("от 1 до 9 ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("включительно.\r\rВ зависимости от сложности игры на поле рандомно числа заменяются на ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("пустые клетки.\r\rЦель игры ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("- заменить пустые клетки подходящими числами.\r\n\r\n")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run(" Особенности меню\r\n\r\n")
            {
                FontSize = 30,
                Foreground = Brushes.Yellow,

            });

            paragraph.Inlines.Add(new Run("Для начала ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("новой игры ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });

            paragraph.Inlines.Add(new Run("нужно выбрать сложность.\r\r")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("В игре есть 3 основных режима сложности:\r\r")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("Легко\r\r")
            {
                FontSize = 21,
                Foreground = Brushes.LightGreen,

            });
         
            paragraph.Inlines.Add(new Run("В игре на этой сложности игровое поле на ~ ")
            {
                FontSize = 19,
                Foreground = Brushes.White,
            });
            paragraph.Inlines.Add(new Run("61% ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("пустое, можно совершить ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("5 ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });

            paragraph.Inlines.Add(new Run("ошибок.\r\r")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });


            paragraph.Inlines.Add(new Run("Трудно\r\r")
            {
                FontSize = 21,
                Foreground = Brushes.Yellow,

            });

            paragraph.Inlines.Add(new Run("В игре на этой сложности игровое поле на ~ ")
            {
                FontSize = 19,
                Foreground = Brushes.White,
            });
            paragraph.Inlines.Add(new Run("72% ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("пустое, можно совершить ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("3 ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });

            paragraph.Inlines.Add(new Run("ошибки.\r\r")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });



            paragraph.Inlines.Add(new Run("Тяжело\r\r")
            {
                FontSize = 21,
                Foreground = Brushes.Orange,

            });

            paragraph.Inlines.Add(new Run("В игре на этой сложности игровое поле на ~ ")
            {
                FontSize = 19,
                Foreground = Brushes.White,
            });
            paragraph.Inlines.Add(new Run("78% ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("пустое, можно совершить ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("1 ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });

            paragraph.Inlines.Add(new Run("ошибку.\r\r")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });


            paragraph.Inlines.Add(new Run("Есть и дополнительный режим:\r\r")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("Свободно\r\r")
            {
                FontSize = 21,
                Foreground = Brushes.BlueViolet,

            });
            paragraph.Inlines.Add(new Run("Он позволяет самостоятельно настроить количество пустых клеток в поле ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });

            paragraph.Inlines.Add(new Run("\"Zeros\" ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("и максимальное количество ошибок в поле ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });

            paragraph.Inlines.Add(new Run("\"Mistakes\"")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });

            paragraph.Inlines.Add(new Run(".\r\rДля загрузки сохранения нужно выбрать сохранение из доступных.\r\rВ игре присутствует функция ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });


            paragraph.Inlines.Add(new Run("автосохранения")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run(".\r\rОна сохраняет игровое поле после ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("каждого хода")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run(".\r\rПри окончании игры в сохранении ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("последний ход не записывается.\r\n\r\n")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run("  Особенности игры\r\n\r\n")
            {
                FontSize = 30,
                Foreground = Brushes.Yellow,

            });

            paragraph.Inlines.Add(new Run("Для выбора клетки нужно использовать ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });

            paragraph.Inlines.Add(new Run("пкм")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });

            paragraph.Inlines.Add(new Run(".\r\rКлетку можно заполнить нажатием на клавишу ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("от 1 до 9 ")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run(".\r\rТак же можно использовать ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });

            paragraph.Inlines.Add(new Run("боковую панель")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run(".\r\rДля сохранения нужно нажать кнопку ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("\"сохранить\"")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run(".\r\rДалее в появившемся поле ввести ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("название сохранения")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run(".\r\rПосле повторно нажать кнопку ")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });
            paragraph.Inlines.Add(new Run("\"сохранить\"")
            {
                FontSize = 19,
                Foreground = Brushes.LightBlue,

            });
            paragraph.Inlines.Add(new Run(".")
            {
                FontSize = 19,
                Foreground = Brushes.White,

            });

            quan.Document.Blocks.Add(paragraph);

        }
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange < 0) // Прокрутка вниз
            {
                if (e.VerticalOffset + e.ViewportHeight <= e.ExtentHeight & quan.IsVisible == true)
                {
                    // Если достигнут конец, делаем кнопку видимой
                    //MessageBox.Show("pisa");
                    /*HpOutBt.Visibility = Visibility.Collapsed; */
                    HpOutBt.Visibility = Visibility.Visible;
                }
                else { /*HpOutBt.Visibility = Visibility.Collapsed; *//*HpOutBt.Visibility = Visibility.Visible;*/ }
            }
        }
    }
   
}
