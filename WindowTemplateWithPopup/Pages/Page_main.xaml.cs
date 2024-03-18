using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowTemplateWithPopup.Pages
{
    /// <summary>
    /// Interaction logic for Page_main.xaml
    /// </summary>
    /// 

    public partial class Page_main : Page
    {

        #region private_varibles
        private Funcs.Popups_Funcs popups_funcs = new Funcs.Popups_Funcs();
        private Funcs.MainWindow_Funcs window_funcs = new Funcs.MainWindow_Funcs();
        private MainWindow mainWindow = App.Current.MainWindow as MainWindow;
        private List<string> availableCultures = new List<string>();
        private Classes.Constants constants = new Classes.Constants();
        #endregion

        public Page_main()
        {
            InitializeComponent();
            availableCultures = mainWindow.availableCultures;
            availableCultures.Insert(0, FindResource(constants.cb_select_language).ToString());
            cb_change_language.ItemsSource = availableCultures;
        }

        private void btn_open_popup_Click(object sender, RoutedEventArgs e)
        {
            popups_funcs.showpopup(Classes.Enums.Popups.Popup, FindResource(constants.popup_accept) + " " + FindResource(constants.popup_cancel));
        }

        private void btn_open_small_popup_Click(object sender, RoutedEventArgs e)
        {
            popups_funcs.showpopup(Classes.Enums.Popups.SlideDown, FindResource(constants.popup_greeteng).ToString());
        }

        private void cb_change_language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cb_change_language.SelectedIndex != 0)
            {
                mainWindow.localization(cb_change_language.SelectedItem.ToString());
            }
        }

    }
}
