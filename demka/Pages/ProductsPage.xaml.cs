using demka.AppData;
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

namespace demka.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        private List<products> _all = new List<products>();
        public ProductsPage()
        {
            InitializeComponent();
            var mw = Application.Current.MainWindow as MainWindow;
            //mw.fio.Text = AppConnect.user?.FIO ?? "Гость";

            if (AppConnect.user == null || AppConnect.user.id_role == 3)
                filterPanel.Visibility = Visibility.Collapsed;

            Load();
        }
        private void Load()
        {

        }

        private void listProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
