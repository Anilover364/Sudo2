using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Sudo2
{

    public partial class Game : Window
    {
        //private MainWindow MainsecondWindow;
        public static Game CurrentInstance { get; private set; }
        public Game()
        {
            InitializeComponent();
            CurrentInstance = this;
            //Title = Game.ChSave;
            //MessageBox.Show(Game.ChSave);
            //подставлем значение массива на игровое поле, заполняем массив для сверки
            //MainsecondWindow = new MainWindow();
            truemistMax =true;
            if (DataFunc.AutoSave == true)
            {
                MenuAutoSave.IsChecked = true;
            }
            if (DataFunc.WinMod == false)
            {
                //MenuWinStyle.IsChecked = false;
                DataFunc.WinMod = false;
                this.WindowStyle = WindowStyle.ThreeDBorderWindow;
                MenuWinStyle.Header = "Полноэкранный режим";
                MenuSaveName.Visibility = Visibility.Collapsed;

            }
            else
            {
                DataFunc.WinMod = true;
                //MenuWinStyle.IsChecked = true;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Normal;
                this.WindowState = WindowState.Maximized;
                MenuWinStyle.Header = "Оконный режим";
                MenuSaveName.Visibility = Visibility.Visible;
                MenuSaveName.Header = Title;

            }
            if (Saved == false)
            {
                int k = 0;
                mist = 0;
                int[,] sud = MapGener.sudokuGenerator(k);
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        string buttonName = $"Bt{i}{j}";
                        if (this.FindName(buttonName) is Button button)
                        {
                            button.Content = sud[i, j];
                            if (sud[i, j] == 0) { button.Content = ""; }
                            n[i, j] = MapGener.n[i, j];
                            m[i, j] = sud[i, j];
                        }

                    }
                }
            }
            //ТУТ В САМОМ КОНЦЕ РАБОТЫ ПЕРЕДЕЛАТЬ И СКОМПОНОВАТЬ ПРАВИЛЬНО
            else
            {
                //string Save = "Save";
                var PathSave = Directory.GetCurrentDirectory();
                PathSave += $@"\SudoData\Saves\{ChSave}.txt";
                StreamReader test = new StreamReader(PathSave);
                //string line;
                //int localzero = 0;
                //zero = 0;
                //string jaja;
                string gigi = MainWindow.gigi;
                string prov = MainWindow.prov;
                int t = 0;

                //это проверка на 0 в готовой карте

                //foreach (char c in prov)
                //{

                //    int p = Convert.ToInt32(c.ToString());
                //    if (p == 0)
                //    {
                //        if (!MainsecondWindow.IsVisible)
                //        {
                //            delay();
                //        }
                //        //MessageBox.Show(c.ToString());

                //    }
                //}

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        string buttonName = $"Bt{i}{j}";
                        if (this.FindName(buttonName) is Button button)
                        {
                            button.Content = gigi[t];
                            if (gigi[t] == '0') { /*zero++;*/ button.Content = ""; }
                            m[i, j] = Convert.ToInt32(gigi[t].ToString());
                            n[i, j] = Convert.ToInt32(prov[t].ToString());

                            t++;
                        }
                    }
                }


            }
     
                DisplayResult();
            //}
            string ButName;
            int LocCount = 0;
            char[] ch = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };


            for (int k = 0; k < ch.Length; k++)
            {
                for (int i = 0; i < 9; i++) 
                {
                    for (int j = 0; j < 9; j++) 
                    {
                        ButName = $"Bt{i}{j}";
                        if (this.FindName(ButName) is Button NowButNamel)
                        {
                            string STButcon = Convert.ToString(NowButNamel.Content);
                            if (ch[k].ToString() == STButcon.ToString())
                            {
                                LocCount++;
                                if (LocCount == 9)
                                {
                                    string ButNamePan = "BtPan" + STButcon;
                                    if (this.FindName(ButNamePan) is Button NowutNamePan)
                                    {
                                        NowutNamePan.IsEnabled = false;
                                    }
                                }
                            }
                        }
                   }
                }
                LocCount = 0;
            }

            FirstClicl =true;

        }
        // private async void delay() 
        //{
        //await Task.Delay(1);
        //    FileError = true;

        //        MainsecondWindow.Show();

        //        this.Close();

        //}



        //потом в класс отдельный запихнуть
        public static Binding BindingBack = new Binding("Background");
        public static Button LastButton;
        public static Border LastBorder;
        public static string LastChangeButtonRow;
        public static string LastChangeButtonCol;
        public static string Lastbutcon;
        public static int y=10,yy=10;
        public static string butconGrid;
        bool GameEnd=false; 
        public static int Count = 0;
        public static bool Saved = false;
        public static string ChSave;
        public static bool truemistMax=true;
     



        private void SelectACell(object sender, RoutedEventArgs e)
        {
            //данные для изменения вида поля
            var Bt = sender as Button;
            int x = int.Parse(Bt.Name[2].ToString());
            int xx = int.Parse(Bt.Name[3].ToString());
            context.Text = "";
            string ChangeButtonRow;
            string ChangeButtonCol;
            string butcon = Convert.ToString(Bt.Content);
            //butconGrid = butcon;
            string ButName;
            Count = 0; 
            string ButNameGrid;
           var B = (Border)((GroupBox)((Grid)((Viewbox)Bt.Parent).Parent).Parent).Parent;
            var binding = new Binding("Backgroung");
            binding.Source = B;
            string hexColor = "#FFCEC4C4";
            string hexColor1 = "#FFD4CC00";
            Color color = (Color)ColorConverter.ConvertFromString(hexColor);
            Color color1 = (Color)ColorConverter.ConvertFromString(hexColor1);

            //убираем кнопки
            if (Lastbutcon != null)
            {
                for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ButName = $"Bt{i}{j}";
                    if (this.FindName(ButName) is Button LastButName)
                    {



                            string STLastButcon = Convert.ToString(LastButName.Content);
                            if (Lastbutcon == STLastButcon & Lastbutcon != butcon) { LastButName.SetBinding(Panel.BackgroundProperty, binding); /*SetBinding(Panel.BackgroundProperty, binding);*/ }
                        }
                        }
                    }
                }
            //если мы заполнили все элементы, то потом для отчищения на карте нужна эта хуйня потом переделать
            if (butconGrid != null)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {

                        ButNameGrid = $"Bt{i}{j}"; if (this.FindName(ButNameGrid) is Button LastButNameGrid)
                        {



                            string STLastButcon = Convert.ToString(LastButNameGrid.Content);
                            if (butconGrid == STLastButcon) { LastButNameGrid.SetBinding(Panel.BackgroundProperty, binding); }
                        }

                    }
                }
            }
            //убираем линии
            if (LastButton != null)
            {
                for (int i = 0; i < 9; i++)
                {
                    LastChangeButtonRow = $"Bt{y}{i}";
                    LastChangeButtonCol = $"Bt{i}{yy}";
                    if (this.FindName(LastChangeButtonRow) is Button buttonRow)
                    {
                        buttonRow.SetBinding(Panel.BackgroundProperty, binding);
                        //UpdateButtonRowBackground(LastChangeButtonRow, binding: binding);
        
                    }


                    if (this.FindName(LastChangeButtonCol) is Button buttonCol)
                    {
                        buttonCol.SetBinding(Panel.BackgroundProperty, binding);
                        //UpdateButtonColBackground(LastChangeButtonCol, binding: binding);
                    }

                }
            }

            //убираем поле
            if (LastButton != null)
            {
            LastBorder.Background = new SolidColorBrush(color);
            }
            //убираем кнопку
            if (LastButton != null)
            {
                LastButton.SetBinding(Panel.BackgroundProperty, binding);
            }
            //добавляем кнопки
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ButName = $"Bt{i}{j}";
                    if (this.FindName(ButName) is Button NowButName)
                    {

                        string STButcon = Convert.ToString(NowButName.Content);
                        if (butcon == STButcon & butcon != "") { NowButName.Background = new SolidColorBrush(color1); Count++; }
                    }
                }
            }

            //добавляем линии
            for (int i = 0; i < 9; i++) 
            {
                ChangeButtonRow = $"Bt{x}{i}";
                ChangeButtonCol = $"Bt{i}{xx}";
                if (this.FindName(ChangeButtonRow) is Button buttonRow )
                {
                    //buttonRow.SetBinding(Panel.BackgroundProperty, binding);
                    buttonRow.Background = new SolidColorBrush(color1);
                    //UpdateButtonRowBackground(ChangeButtonRow, customColor: color1);
                }

                if (this.FindName(ChangeButtonCol) is Button buttonCol)
                {
                    buttonCol.Background = new SolidColorBrush(color1);
                    //UpdateButtonColBackground(ChangeButtonCol, customColor: color1);
                }
               
            }

            //добавляем поле
            B.Background = new SolidColorBrush(color1);
            //добавляем кнопку
            Bt.Background = Brushes.Yellow;

            //забиваем наши данные как ластовые
            BindingBack = binding;
            LastButton = sender as Button;
            LastBorder = B;
            Lastbutcon= butcon;
            y = x;
            yy = xx;
         
        }
     
        //private void UpdateButtonRowBackground(string buttonRowName, Binding binding = null, Color? customColor = null)
        //{
        //    if (this.FindName(buttonRowName) is Button buttonRow)
        //    {
        //        if (binding != null)
        //        {
        //            // Если передан биндинг, используем его
        //            buttonRow.SetBinding(Panel.BackgroundProperty, binding);
        //        }
        //        else if (customColor.HasValue)
        //        {
        //            // Если передан цвет, используем его
        //            buttonRow.Background = new SolidColorBrush(customColor.Value);
        //        }
        //    }
        //}
        //private void UpdateButtonColBackground(string buttonColName, Binding binding = null, Color? customColor = null)
        //{
        //    if (this.FindName(buttonColName) is Button buttonCol) 
        //    {
        //        if (binding != null)
        //        {
        //            buttonCol.SetBinding(Panel.BackgroundProperty, binding);
        //        }
        //        else if (customColor.HasValue)
        //        {
        //            // Если передан цвет, используем его
        //            buttonCol.Background = new SolidColorBrush(customColor.Value);
        //        }
        //    }
        //}
        public static int[,] n = new int[9, 9];
        public static int[,] m= new int[9, 9];
        public static int zero = 0;
        public static int comp = 0;
        public static int mist = 0;
        public static int mistMax = 0;
        public static bool FirstClicl=true;
        public static bool TextFocus=false;
        public static bool FileError=false;
        

 

        //текущая информация об игре
        private void DisplayResult()
        {
            
                quan.Document.Blocks.Clear();

                Paragraph paragraph = new Paragraph();

                paragraph.Inlines.Add(new Run("Количество ошибок ")
                {
                    Foreground = Brushes.White
                });
                if ((mist * 100) / mistMax > 60)
                {
                    paragraph.Inlines.Add(new Run(mist.ToString())
                    {
                        Foreground = Brushes.Red
                    });
                }
                else if ((mist * 100) / mistMax > 30)
                {
                    paragraph.Inlines.Add(new Run(mist.ToString())
                    {
                        Foreground = Brushes.Yellow
                    });
                }
                else if ((mist * 100) / mistMax <= 30)
                {
                    paragraph.Inlines.Add(new Run(mist.ToString())
                    {
                        Foreground = Brushes.Green
                    });
                }
                paragraph.Inlines.Add(new Run($" из {Convert.ToString(mistMax)}")
                {
                    Foreground = Brushes.White
                });
                quan.Document.Blocks.Add(paragraph);
                nana.Text = $"Осталось: {Convert.ToString(Game.zero)} клетки(ок)";

            
        }

        //выбор значения клетки при помощи клавиатуры
        private void SelNumDown(object sender, KeyEventArgs e)
        {
            if (TextFocus == false)
            {
                if (DataFunc.KeyToValueMap.TryGetValue(e.Key, out int value))
                {
                    if (LastButton != null)
                    {
                        CheckGrid(LastButton, value);
                    }
                }
            }
            //else { context.Text = "baka"; }
        }

//выбор значения клетки при помощи панели кнопок
        private void SelNumClick(object sender, RoutedEventArgs e)
        {
       
                var Bt = sender as Button;
                int value = Convert.ToInt32(Bt.Content);

                string cont = Convert.ToString(Bt.Content);
                if (LastButton != null)
                {
                    CheckGrid(LastButton, value);
                }
     
         
        }
        //проверка значений, начало конца игры
        public void CheckGrid(Button lastButton, int value)
        {
            int x = int.Parse(lastButton.Name[2].ToString());
            int y = int.Parse(lastButton.Name[3].ToString());
            string ButName;
            int LocConut=0;
            int FastLocCount = 0;
            string numbut = "";
            string Save = Game.ChSave;
            string hexColor1 = "#FFD4CC00";
    
            Color color1 = (Color)ColorConverter.ConvertFromString(hexColor1);
            //if (m[x, y] == n[x, y]) { context.Text = ""; }
            if (GameEnd == false & DataFunc.AutoSave == true & Game.CurrentInstance.Title == "Новая игра ") { GameEnd = true; context.Text = "При автосохранении обьявите\r\n название сохранeния";/* if (text.IsVisible == false) {*/ text.Visibility = Visibility; /* text.Focus();  text.SelectAll(); } */}
            //добавляем кнопки
            if (GameEnd == false)
            {
                if (Count != 9)
                {
                    if (m[x, y] == 0)
                    {
                        if (value == n[x, y])
                        {
                            lastButton.Content = n[x, y].ToString();
                            m[x, y] = n[x, y];
                            if (m[x, y] == n[x, y]) { context.Text = ""; }
                            FastLocCount++;
                            zero--;

                            if (zero == 0)
                            {
                                //lala.Text = "Победа!";
                                GBEnd.Visibility = Visibility.Visible;
                                GameEnd = true;
                            }
                        }
                        else
                        {
                            /*  lala.Text = "Дурак, число не подходит"*/
                            mist += 1; ;
                            context.Text = "Выбранное число не подходит";
                        }
                        if (mist == mistMax) { GBEnd.Visibility = Visibility.Visible; TBEnd.Text = "Поражение"; TBEnd.Foreground = Brushes.Red; GameEnd = true; context.Text = "";/*GBGrid.IsEnabled = false; context.Text = "";*/ }
                    }

                    else { context.Text = "Выбранная клетка имеет значение"; }
                }
                else { context.Text = $"Значение {m[x, y]} есть во всех блоках "; }
                DisplayResult();
                if (GameEnd == false & DataFunc.AutoSave == true & Game.CurrentInstance.Title != "Новая игра ")
                    DataFunc.TempSaved(numbut, Title);
                //else if (GameEnd == false & DataFunc.AutoSave == true & Game.CurrentInstance.Title == "Новая игра ") { GameEnd = true; context.Text = "При автосохранении обьявите\r\n название сохранeния";/* if (text.IsVisible == false) {*/ text.Visibility = Visibility; /* text.Focus();  text.SelectAll(); } */}

            }
            //else 
            //{
            
            //}
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ButName = $"Bt{i}{j}";
                    if (this.FindName(ButName) is Button NowButNamel)
                    {

                        string STButcon = Convert.ToString(NowButNamel.Content);
                        if (m[x, y].ToString() == STButcon )
                        {
                            NowButNamel.Background = new SolidColorBrush(color1); ;
            butconGrid = STButcon;
                            LocConut +=1;
                            //nana.Text = LocConut.ToString();
                            if (LocConut == 9) 
                            {
                                string ButNamePan = "BtPan"+STButcon;
                                if (this.FindName(ButNamePan) is Button NowutNamePan)
                                {
                                NowutNamePan.IsEnabled = false;
                                    if(context.Text == "Выбранная клетка имеет значение"){context.Text = $"Значение {m[x, y]} есть во всех блоках "; }
                           
                                }
                            }
                        }
                    }
                }
            }
            LastButton.Background = Brushes.Yellow;
            //if (m[x, y] == n[x, y]) { context.Text = ""; }

        }

 
        private void text_LostFocus(object sender, RoutedEventArgs e)
        {
            //this.Close();
            TextFocus=false;
        }

        private void text_GotFocus(object sender, RoutedEventArgs e)
        {
            //this.Close();
            TextFocus=true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void text_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                return;
            }
        }

        //private void text_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (text.Focusable == false) 
        //    {
        //    this.Close();
        //    }
        //}

        //public static void CheckSybol( ref string  SaveName, ref bool SymError, TextBlock Context) 
        //{
        //    char[] er = { '<', '>', ':', '"', '|', '/', '?', '\\', '*' };
        //    foreach (char ch in SaveName)
        //    {
        //        for (int i = 0; i < er.Length; i++)
        //        {
        //            if (er[i] == ch) { Context.Text = "Название имеет запрещенные символы"; SymError = true; } /*доделаааааааааааааааааааааааааааааааать*/
        //        }

        //    }
        //}

        //кнопки (все из окна блока информации)
        private void SeptClick(object sender, RoutedEventArgs e)
        {
            var Bt = sender as Button;
            switch (Bt.Name) 
            {
                case "BtOut":
                    this.Close();
                    break;
                case "BtMenu":
                    new MainWindow().Show();
                    this.Close();
                    break;
                case "BtSave":
                    if (GameEnd == false||GameEnd == true & context.Text == "При автосохранении обьявите\r\n название сохранeния")
                    {
                        if(GameEnd==true)GameEnd = false;
                        //if (FirstClicl == false) { FirstClicl = true; }
                        if (FirstClicl == false)
                        {
                            DataFunc.InGame = true;
                            DataFunc.CheckingSaveNameAndStartGame(DataFunc.InGame, text);

                            //string Save = text.Text;
                            //string numbut;
                            //string line;
                            //bool DoubleS = false;
                            //bool ErrorChar = false;
                            ////CheckSybol( ref Save,ref ErrorChar, context);

                            //char[] er = { '<', '>', ':', '"', '|', '/', '?', '\\', '*' };
                            //if (text.Text != "")
                            //{
                            //foreach (char ch in Save)
                            //{
                            //    for (int i = 0; i < er.Length; i++)
                            //    {
                            //        if (er[i] == ch) { context.Text = "Название имеет запрещенные символы"; text.Focus(); ErrorChar = true; } /*доделаааааааааааааааааааааааааааааааать*/
                            //    }

                            //}
                            //    if (ErrorChar == false)
                            //    {

                            //        var PathSavesName = Directory.GetCurrentDirectory();
                            //        PathSavesName += $@"\SudoData\SavesNameOnly\SavesName.txt";
                            //        //string testmg = text.Text;
                            //        //создаем текстовик с названием файлов
                            //        //StreamWriter SavedNameCreate = new StreamWriter(PathSavesName);
                            //        //SavedNameCreate.Close();
                            //        if (!File.Exists(PathSavesName))
                            //        {
                            //            // Если файл не существует, создаем его
                            //            File.Create(PathSavesName).Close();
                            //            //MessageBox.Show("lala");
                            //        }
                            //        //else { MessageBox.Show("lalalalal"); }


                            //            StreamReader SavesNameRead = new StreamReader(PathSavesName);
                            //        while ((line = SavesNameRead.ReadLine()) != null)
                            //        {
                            //            if (line == Save) { DoubleS = true; context.Text = "Сохранение успешно обновлено"; /*nana.Text = " gg";*/ }
                            //        }
                            //        SavesNameRead.Close();
                            //        if (DoubleS == false)
                            //        {


                            //            StreamWriter SavesNameIN = new StreamWriter(PathSavesName, true);
                            //            SavesNameIN.WriteLine(Save);
                            //            SavesNameIN.Close();

                            //            var PathSave = Directory.GetCurrentDirectory();
                            //            PathSave += $@"\SudoData\Saves\{Save}.txt";
                            //            StreamWriter lana = new StreamWriter(PathSave);
                            //            //lana.WriteLine(testmg);
                            //            lana.WriteLine("карта");


                            //            for (int i = 0; i < 9; i++)
                            //            {
                            //                for (int j = 0; j < 9; j++)
                            //                {
                            //                    //string buttonName = $"Bt{i}{j}";
                            //                    //if (this.FindName(buttonName) is Button button)
                            //                    //{
                            //                    //    numbut = Convert.ToString(button.Content);
                            //                    //    if (Convert.ToString(button.Content) == "") { numbut = "0"; }
                            //                    //    lana.Write(numbut);
                            //                    //}
                            //                    numbut = m[i, j].ToString();

                            //                    lana.Write(numbut);
                            //                }
                            //                lana.WriteLine();
                            //            }

                            //            lana.WriteLine("проверка");
                            //            for (int i = 0; i < 9; i++)
                            //            {
                            //                for (int j = 0; j < 9; j++)
                            //                {
                            //                    numbut = n[i, j].ToString();

                            //                    lana.Write(numbut);
                            //                }
                            //                lana.WriteLine();
                            //            }
                            //            lana.WriteLine($"mist={mist}");
                            //            lana.WriteLine($"MMax={mistMax}");
                            //            lana.WriteLine($"zero={zero}");
                            //            //lana.WriteLine(":lalala");
                            //            lana.Close();
                            //            context.Text = "Сохранение успешно сохранено";
                            //        }
                            //        else
                            //        {

                            //            var PathSaveCopy = Directory.GetCurrentDirectory();
                            //            PathSaveCopy += $@"\SudoData\Saves\{Save}-Copy.txt";
                            //            var PathSave = Directory.GetCurrentDirectory();
                            //            PathSave += $@"\SudoData\Saves\{Save}.txt";
                            //            StreamWriter SaveCopy = new StreamWriter(PathSaveCopy);
                            //            SaveCopy.WriteLine("карта");


                            //            for (int i = 0; i < 9; i++)
                            //            {
                            //                for (int j = 0; j < 9; j++)
                            //                {
                            //                    //string buttonName = $"Bt{i}{j}";
                            //                    //if (this.FindName(buttonName) is Button button)
                            //                    //{
                            //                    //    numbut = Convert.ToString(button.Content);
                            //                    //    if (Convert.ToString(button.Content) == "") { numbut = "0"; }
                            //                    //    lana.Write(numbut);
                            //                    //}
                            //                    numbut = m[i, j].ToString();

                            //                    SaveCopy.Write(numbut);
                            //                }
                            //                SaveCopy.WriteLine();
                            //            }

                            //            SaveCopy.WriteLine("проверка");
                            //            for (int i = 0; i < 9; i++)
                            //            {
                            //                for (int j = 0; j < 9; j++)
                            //                {
                            //                    numbut = n[i, j].ToString();

                            //                    SaveCopy.Write(numbut);
                            //                }
                            //                SaveCopy.WriteLine();
                            //            }
                            //            SaveCopy.WriteLine($"mist={mist}");
                            //            SaveCopy.WriteLine($"MMax={mistMax}");
                            //            SaveCopy.WriteLine($"zero={zero}");
                            //            //lana.WriteLine(":lalala");
                            //            SaveCopy.Close();
                            //            File.Delete(PathSave);
                            //            File.Move(PathSaveCopy, PathSave);
                            //        }
                            //    }
                            //    //else { this.Close(); }
                            //}
                            //else { context.Text = " Поле для названия пустое"; text.Focus(); }
                        }
                        else
                        {
                            FirstClicl = false; text.Visibility = Visibility; text.Focus(); text.SelectAll();
                        }
                    }
                    //else if (GameEnd == true & context.Text == "При автосохранении обьявите\r\n название сохранeния") 
                    //{
                    
                    //}
                    //else { context.Text = "huui tebe"; }
                    break;
                    
            }
        }
        private void MenuClick(object sender, RoutedEventArgs e)
        { 
            var send = sender as MenuItem;
            switch (send.Name) 
            {
                case "MenuAutoSave":
                    if (DataFunc.AutoSave == true)
                    {
                        MenuAutoSave.IsChecked = false;
                        DataFunc.AutoSave = false;
                    }
                    else 
                    {
                        DataFunc.AutoSave = true;
                        MenuAutoSave.IsChecked = true;
                    }
                    break;
                case "MenuWinStyle":
                    if (DataFunc.WinMod == true)
                    {
                        //MenuWinStyle.IsChecked = false;
                        DataFunc.WinMod = false;
                        this.WindowStyle=WindowStyle.ThreeDBorderWindow;
                        MenuWinStyle.Header = "Полноэкранный режим";
                        MenuSaveName.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        DataFunc.WinMod = true;
                        //MenuWinStyle.IsChecked = true;
                        this.WindowStyle = WindowStyle.None;
                        this.WindowState = WindowState.Normal;
                        this.WindowState = WindowState.Maximized;
                        MenuWinStyle.Header = "Оконный режим";
                        MenuSaveName.Visibility = Visibility.Visible;
                        MenuSaveName.Header=Title;
                        //    var fileMenu = Menu.Items[0] as MenuItem;
                        //var exitMenuItem = new MenuItem
                        //{
                        //    Header = "Выход"
                        //};
                    }
                    break;
                case "MainMenu":
                    new MainWindow().Show();
                    this.Close();
                    break;
            }
        }
    }
}