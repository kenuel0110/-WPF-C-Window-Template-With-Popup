using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace WindowTemplateWithPopup.Funcs
{
    class Popups_Funcs
    {
        public MainWindow mainWindow = App.Current.MainWindow as MainWindow;

        public async void showpopup(Classes.Enums.Popups popup = Classes.Enums.Popups.None, string info = "") 
        {
            switch (popup) 
            {
                case Classes.Enums.Popups.SlideDown:
                    mainWindow.popup_slide_frame.Visibility = Visibility.Visible;
                    mainWindow.popup_slide_frame.NavigationService.Navigate(new Popups.Popup_slidedown_info(info));
                    break;

                case Classes.Enums.Popups.Popup:
                    bool result = await mainWindow.open_popup();
                    if (result == true)
                    {
                        showpopup(Classes.Enums.Popups.SlideDown, info.Split()[0].ToString());
                    }
                    else
                    {
                        showpopup(Classes.Enums.Popups.SlideDown, info.Split()[1].ToString());
                    }
                    break;
            }
        }

        public void hidelidedownpopup()
        {
            mainWindow.popup_slide_frame.Visibility = Visibility.Hidden;
        }

        //blur main frame content
        public void blurBackground()
        {
            mainWindow.border_shadow.Visibility = Visibility.Visible;
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 0;
            mainWindow.main_frame.Effect = blurEffect;

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

            Storyboard.SetTarget(blurEffect, mainWindow.main_frame);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Effect).(BlurEffect.Radius)"));

            Storyboard.SetTarget(animation_shadow, mainWindow.border_shadow);
            Storyboard.SetTargetProperty(animation_shadow, new PropertyPath("Opacity"));

            // Запустите анимацию
            storyboard.Begin(mainWindow.main_frame);
            mainWindow.border_shadow.Visibility = Visibility.Visible;
        }

        //deblur main frame content
        public void deblurBackground()
        {
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 5;
            mainWindow.main_frame.Effect = blurEffect;

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

            Storyboard.SetTarget(blurEffect, mainWindow.main_frame);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Effect).(BlurEffect.Radius)"));

            Storyboard.SetTarget(animation_shadow, mainWindow.border_shadow);
            Storyboard.SetTargetProperty(animation_shadow, new PropertyPath("Opacity"));

            storyboard.Begin(mainWindow.main_frame);
            mainWindow.border_shadow.Visibility = Visibility.Hidden;
        }
    }
}
