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

namespace WindowTemplateWithPopup.Pages
{
    /// <summary>
    /// Interaction logic for Page_main.xaml
    /// </summary>
    /// 

    public partial class Page_main : Page
    {

        #region local_value
        private Funcs.Popups_Funcs popups_funcs = new Funcs.Popups_Funcs();
        private MainWindow mainWindow = App.Current.MainWindow as MainWindow;
        private string language_state;
        #endregion

        public Page_main()
        {
            InitializeComponent();
            language_state = mainWindow.systemLanguage;
        }

        private void btn_change_language_Click(object sender, RoutedEventArgs e)
        {
            if (language_state == "en-EN") 
            {
                App.SelectCulture("ru-RU");
                language_state = "ru-RU";
            }
            else if (language_state == "ru-RU")
            {
                App.SelectCulture("en-EN");
                language_state = "en-EN";
            }
        }

        private void btn_open_popup_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_open_small_popup_Click(object sender, RoutedEventArgs e)
        {
            popups_funcs.showslidedownpopup(Classes.Enums.Popups.SlideDown, FindResource("popup_greeteng").ToString());
        }
    }
}
