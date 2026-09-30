using demka.AppData;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            _all = AppConnect.Model1.products.ToList();

            cmbSort.Items.Clear();
            cmbSort.Items.Add("Без сортировки");
            cmbSort.Items.Add("Цена ↑");
            cmbSort.Items.Add("Цена ↓");
            cmbSort.SelectedIndex = 0;

            cmbFilter.Items.Clear();
            cmbFilter.Items.Add("Все категории");
            foreach (var c in AppConnect.Model1.categorys.ToList())
                cmbFilter.Items.Add(c.category);
            cmbFilter.SelectedIndex = 0;

            Apply();
        }
        private void Apply()
        {
            var list = _all.AsEnumerable();

            string q = txtSearch.Text.Trim().ToLower();
            if (q.Length > 0)
                list = list.Where(p =>
                p.name_product.ToLower().Contains(q) ||
                (p.description ?? "").ToLower().Contains(q));
            if (cmbFilter.SelectedIndex > 0)
            {
                string cat = cmbFilter.SelectedItem.ToString();
                list = list.Where(p =>
                p.categorys.category == cat);
            }
            if (cmbSort.SelectedIndex == 1)
                list = list.OrderBy(p => p.price);
            if (cmbSort.SelectedIndex == 2)
                list = list.OrderByDescending(p => p.price);

            listProduct.ItemsSource = list.ToList();
        }


        private void listProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(listProduct.SelectedItems is products p)) return;

            if (AppConnect.SelectProductMode)
            {
                AppFrame.mainFrame.Navigate(new ProductDetailsPage(p, AppConnect.EditingOrder));
                return;
            }
            if (AppConnect.user == null)
            {
                MessageBox.Show("Авторизуйтесь.");
                return;
            }
            AppFrame.mainFrame.Navigate(new ProductDetailsPage(p));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Filters_Changed(object sender, TextChangedEventArgs e) => Apply();

        private void Del_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Filters_Changed(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
