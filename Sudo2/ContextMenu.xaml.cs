using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sudo2
{
    /// <summary>
    /// Логика взаимодействия для ContextMenu.xaml
    /// </summary>
    public partial class ContextMenu : Window
    {
        public static ContextMenu CurrentInstance { get; private set; }
        //private MainWindow secondWindows;
        public ContextMenu()
        {
            InitializeComponent();
            CurrentInstance = this;
            //secondMainWindow =  MainWindow();
            //secondWindows = new MainWindow();
            ////double left = .Left + this.Width + 10;
            //this.Left = 10;
            //this.Top = 405;

        }
        private  void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //switch (DataFunc.ERROR)
            //{
            //    case 0:
            //        TBERROR.Text = "lalala";
            //        break;
            //    case 1:
            //        TBERROR.Text = "Введите цифры от 1 до 9";
            //        break;

            //}
            //await Task.Delay(2500);
            //this.Visibility = Visibility.Hidden;
        }

        private  void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            contx.Text = "Ошибка";
            switch (DataFunc.ERROR)
            {
                case 0:
                    TBERROR.Text = "этот текст вылезает когда Тимур не исправил ошибку ИСПРАВЬ ДОЛБАЕБ";
                    break;
                case 1:
                    TBERROR.Text = "Введите цифры от 1 до 9";
                    break;
                case 2:
                    TBERROR.Text = "Выбранного сохранения не существует";
                    break;
                case 3:
                    contx.Text = "";
                    TBERROR.Text = "Сохранение успешно удалено";
                    break;
                case 4:
                    TBERROR.Text = "Число выходит за разрешенные пределы";
                    break;
                case 5:
                    TBERROR.Text = "Сохранение имеет запрещенные символы";
                    break;
                    case 6:
                    TBERROR.Text = "Сохранение имеет неверный вид\r\nФайл изменен";
                    break;
                    case 7:
                    TBERROR.Text = "Название сохранения не может быть пустым";
                    break;
                case 8:
                    TBERROR.Text = "Название сохранения имеет запрещенные символы";
                    break;
                case 9:
                    TBERROR.Text = "Сохранение c данным названием уже существует";
                    break;
                case 10:
                    TBERROR.Text = "Сохранение доступно только для чтения";
                    break;

            }
            //await Task.Delay(2500);
            //this.Visibility = Visibility.Hidden;
        }
        //private MainWindow secondMainWindow;
        //MainWindow secondWindow = new MainWindow();
        private void AutoSave_Click(object sender, RoutedEventArgs e)
        {
            var send = sender as Button;
            switch (send.Name) 
            {
                case "BtAutoSaveOut":
                    MainWindow.CurrentInstance.GoGameBt.Visibility = Visibility.Visible;
                    MainWindow.CurrentInstance.PCOutBt.Visibility = Visibility.Visible;  
                    AutoSave.Visibility = Visibility.Collapsed;
                    this.Visibility = Visibility.Hidden;

                    break;
                case "BtAutoSaveGo":
                    DataFunc.InGame = false;
                    DataFunc.CheckingSaveNameAndStartGame(DataFunc.InGame, text);
                    break;
            }
        }
        
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (MainWindow.CurrentInstance.WindowState == WindowState.Minimized)
            {
                //this.WindowState=WindowState.Minimized;
                //this.Close();
                this.WindowState = WindowState.Minimized;
            }
        }

        private void text_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                return;
            }
        }
        //public static string ERROR;
    }
}
