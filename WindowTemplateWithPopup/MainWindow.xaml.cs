﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        #region public_varibles
        //Language strings
        public string systemLanguage;
        public List<string> availableCultures;

        //hot keys
        public static RoutedCommand MyCommand = new RoutedCommand();
        #endregion

        #region private_varibles
        private Funcs.MainWindow_Funcs mainwindow_funcs = new Funcs.MainWindow_Funcs();
        private Classes.Constants constants = new Classes.Constants();

        private double newWindowHeight;
        private double newWindowWidth;

        private double newWindowPosX;
        private double newWindowPosY;

        private Classes.Enums.WindowState window_state;

        

        //local keys for popup
        private bool setdialogResult_popup;
        private TaskCompletionSource<bool> tcs;
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
            
            //get list available languages
            availableCultures = App.availableCultures();

            mainwindow_funcs.chkFirstStart(Classes.Constants.settings, Classes.Constants.settings_json);
            mainwindow_funcs.create_json(Classes.Constants.temp_json);

            Classes.Data_Classes.JSON_Setting setting = mainwindow_funcs.openJSONSetting();

            App.SelectCulture(setting.window_language);

            //manual update elements text
            title_window.Text = FindResource(constants.window_title).ToString();
            btn_file.Content = FindResource(constants.window_btn_file).ToString();
            btn_file_new.Header = FindResource(constants.window_menu_new).ToString();
            btn_file_open.Header = FindResource(constants.window_menu_open).ToString();

            WindowSizeState(setting.maximilize_window);
            System.Windows.Application.Current.MainWindow.Height = setting.size_window[0];
            System.Windows.Application.Current.MainWindow.Width = setting.size_window[1];

            this.Left = setting.position_window[0];
            this.Top = setting.position_window[1];

            newWindowHeight = setting.size_window[0];
            newWindowWidth = setting.size_window[1];

            newWindowPosX = setting.position_window[0];
            newWindowPosY = setting.position_window[1];

            //hot key
            MyCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));

            main_frame.NavigationService.Navigate(new Pages.Page_main());
        }

        //hot key funcs
        private async void MyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            bool result = await open_popup();
            if (result == true)
            {
                MessageBox.Show(FindResource(constants.popup_accept).ToString());
            }
            else
            {
                MessageBox.Show(FindResource(constants.popup_cancel).ToString());
            }
        }

        //open Popup func
        public async Task<bool> open_popup()
        {
            blurBackground();
            popup_frame.Visibility = Visibility.Visible;
            Popups.Popup_question popup_question = new Popups.Popup_question();
            popup_frame.NavigationService.Navigate(popup_question);
            tcs = new TaskCompletionSource<bool>();

            // wait, until varible changed
            bool result = await tcs.Task;
            return result;
        }

        //set popup result
        internal void SetDialogResult_popup(bool result)
        {
            setdialogResult_popup = result;
            if (result == true)
                tcs.SetResult(true);
            else
                tcs.SetResult(false);
            tcs.TrySetCanceled();
        }

        //blur background
        public void blurBackground()
        {
            border_shadow.Visibility = Visibility.Visible;
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 0;
            main_frame.Effect = blurEffect;

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 5;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation animation_shadow = new DoubleAnimation();
            animation_shadow.From = 0.0;
            animation_shadow.To = 0.4;
            animation_shadow.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Children.Add(animation_shadow);

            Storyboard.SetTarget(blurEffect, main_frame);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Effect).(BlurEffect.Radius)"));

            Storyboard.SetTarget(animation_shadow, border_shadow);
            Storyboard.SetTargetProperty(animation_shadow, new PropertyPath("Opacity"));

            // Запустите анимацию
            storyboard.Begin(main_frame);
            border_shadow.Visibility = Visibility.Visible;
        }

        //deblur background
        public void deblurBackground()
        {
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 5;
            main_frame.Effect = blurEffect;

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 5;
            animation.To = 0;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation animation_shadow = new DoubleAnimation();
            animation_shadow.From = 0.4;
            animation_shadow.To = 0.0;
            animation_shadow.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Children.Add(animation_shadow);

            Storyboard.SetTarget(blurEffect, main_frame);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Effect).(BlurEffect.Radius)"));

            Storyboard.SetTarget(animation_shadow, border_shadow);
            Storyboard.SetTargetProperty(animation_shadow, new PropertyPath("Opacity"));

            // Запустите анимацию
            storyboard.Begin(main_frame);
            border_shadow.Visibility = Visibility.Hidden;
        }

        //event closing window
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainwindow_funcs.removeTemp();
            mainwindow_funcs.saveJSONSetting(
                new Classes.Data_Classes.JSON_Setting
                {
                    maximilize_window = window_state,
                    size_window = new List<double>() { newWindowHeight, newWindowWidth },
                    position_window = new List<double>() { newWindowPosX, newWindowPosY }
                });
        }

        //Change state window
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

        //Move window
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

        //Main buttons of the window like close / hide and maximizate
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

        //Event change state window
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

        //Event change size window
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            newWindowHeight = e.NewSize.Height;
            newWindowWidth = e.NewSize.Width;
        }

        //Context menu button "file"
        private void btn_file_l_click(object sender, RoutedEventArgs e)
        {
            btn_file_cm.IsOpen = true;
        }

        // Turn off right mouse button for "file" button
        private void btn_file_r_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        //Event changed location window
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            newWindowPosX = this.Left;
            newWindowPosY = this.Top;
        }

        //translate window on other language
        public void localization(string language_state) 
        {
            App.SelectCulture(language_state);
            mainwindow_funcs.saveJSONSetting(
                new Classes.Data_Classes.JSON_Setting
                {
                    maximilize_window = window_state,
                    size_window = new List<double>() { newWindowHeight, newWindowWidth },
                    position_window = new List<double>() { newWindowPosX, newWindowPosY },
                    window_language = language_state
                });

            //reastart app
            System.Windows.Application.Current.Shutdown();
            Thread.Sleep(100);
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
        }
    }
}
