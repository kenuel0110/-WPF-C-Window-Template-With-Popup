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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowTemplateWithPopup.Popups
{
    /// <summary>
    /// Interaction logic for Popup_question.xaml
    /// </summary>
    public partial class Popup_question : Page
    {
        #region local_value
        private Funcs.Popups_Funcs popups_funcs = new Funcs.Popups_Funcs();
        private MainWindow mainWindow = App.Current.MainWindow as MainWindow;
        #endregion

        public Popup_question()
        {
            InitializeComponent();
        }

        private void btn_accept_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.popup_frame.Visibility = Visibility.Hidden;
            mainWindow.deblurBackground();
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            mainWindow.SetDialogResult_popup(true);
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.popup_frame.Visibility = Visibility.Hidden;
            mainWindow.deblurBackground();
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            ((MainWindow)Application.Current.MainWindow).SetDialogResult_popup(false);
        }

        private void btn_popup_close_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.popup_frame.Visibility = Visibility.Hidden;
            mainWindow.deblurBackground();
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            ((MainWindow)Application.Current.MainWindow).SetDialogResult_popup(false);
        }
    }
}
