using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowTemplateWithPopup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region global_values
        //Language strings
        public string systemLanguage;
        #endregion

        #region local_values
        Funcs.MainWindow_Funcs mainwindow_funcs = new Funcs.MainWindow_Funcs();

        private double newWindowHeight;
        private double newWindowWidth;

        private double newWindowPosX;
        private double newWindowPosY;

        private Classes.Enums.WindowState window_state;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            init();
        }

        //initialize func
        private void init()
        {
            //get language
            systemLanguage = System.Globalization.CultureInfo.CurrentCulture.Name;
            App.SelectCulture(systemLanguage);

            mainwindow_funcs.chkFirstStart("settings", "settings.json");
            mainwindow_funcs.create_json("temp.json");

            Classes.Data_Classes.Class_JSON_Setting setting = mainwindow_funcs.openJSONSetting();
            WindowSizeState(setting.maximilize_window);
            System.Windows.Application.Current.MainWindow.Height = setting.size_window[0];
            System.Windows.Application.Current.MainWindow.Width = setting.size_window[1];

            this.Left = setting.position_window[0];
            this.Top = setting.position_window[1];

            newWindowHeight = setting.size_window[0];
            newWindowWidth = setting.size_window[1];

            newWindowPosX = setting.position_window[0];
            newWindowPosY = setting.position_window[1];

            main_frame.NavigationService.Navigate(new Pages.Page_main());
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainwindow_funcs.removeTemp();
            mainwindow_funcs.saveJSONSetting(
                new Classes.Data_Classes.Class_JSON_Setting
                {
                    maximilize_window = window_state,
                    size_window = new List<double>() { newWindowHeight, newWindowWidth },
                    position_window = new List<double>() { newWindowPosX, newWindowPosY }
                });
        }

        //Изменение состояния окна
        private void WindowSizeState(Classes.Enums.WindowState key = Classes.Enums.WindowState.None)
        {
            if (this.WindowState == WindowState.Maximized || key == Classes.Enums.WindowState.Normal)
            {
                this.WindowState = WindowState.Normal;
                main_grid.Margin = new Thickness(0);
                window_state = Classes.Enums.WindowState.Normal;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
                img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_maximilize.png"));
            }
            else if (this.WindowState == WindowState.Normal || key == Classes.Enums.WindowState.Maximilize)
            {
                this.WindowState = WindowState.Maximized;
                main_grid.Margin = new Thickness(5);
                window_state = Classes.Enums.WindowState.Maximilize;
                img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_restore.png"));
            }
        }

        //Перетаскивание окна
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    WindowSizeState();
                }
                else
                {
                    System.Windows.Application.Current.MainWindow.DragMove();
                }
        }

        //Главные кнопки окна
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        private void btn_hide_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_max_min_Click(object sender, RoutedEventArgs e)
        {
            WindowSizeState();
        }

        //Событие изменение состояния окна
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    if (img_btn_max_min.Source != new BitmapImage(new Uri("pack://application:,,,/Icons/btn_restore.png")))
                    {
                        main_grid.Margin = new Thickness(5);
                        window_state = Classes.Enums.WindowState.Maximilize;
                        img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_restore.png"));
                    }
                    break;

                case WindowState.Normal:
                    if (img_btn_max_min.Source != new BitmapImage(new Uri("pack://application:,,,/Icons/btn_maximilize.png")))
                    {
                        main_grid.Margin = new Thickness(0);
                        window_state = Classes.Enums.WindowState.Normal;
                        img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_maximilize.png"));
                    }
                    break;
            }
        }

        //Собитие изменения размеров окна
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            newWindowHeight = e.NewSize.Height;
            newWindowWidth = e.NewSize.Width;
        }

        //Контекстное меню
        private void btn_file_l_click(object sender, RoutedEventArgs e)
        {
            btn_file_cm.IsOpen = true;
        }

        private void btn_file_r_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void btn_main_r_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        /*private void btn_insert_l_click(object sender, RoutedEventArgs e)
        {
            btn_insert_cm.IsOpen = true;
        }*/

        private void btn_insert_r_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            newWindowPosX = this.Left;
            newWindowPosY = this.Top;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
