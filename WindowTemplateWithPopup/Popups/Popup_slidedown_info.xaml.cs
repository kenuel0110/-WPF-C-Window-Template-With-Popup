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
    /// Interaction logic for Popup_slidedown_info.xaml
    /// </summary>

    public partial class Popup_slidedown_info : Page
    {
        #region private_varibles
        private Funcs.Popups_Funcs popups_funcs = new Funcs.Popups_Funcs();
        #endregion

        public Popup_slidedown_info(string info)
        {
            InitializeComponent();
            lbl_info_popup.Text = info;
        }

        private void btn_close_info_Click(object sender, RoutedEventArgs e)
        {
            popup_info.Visibility = Visibility.Hidden;
            popups_funcs.hideslidedownpopup();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            popup_info.Visibility = Visibility.Visible;
        }
    }
}
