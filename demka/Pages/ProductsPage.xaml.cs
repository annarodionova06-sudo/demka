using demka.AppData;
using System;
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


        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.user?.id_role != 1)
            {
                MessageBox.Show("Удаление доступно только администратору.",
                    "Доступ ограниечен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(!(listProduct.SelectedItems is products p)) return;

            if (MessageBox.Show($"Удалить «{p.name_product}»?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes) return;
            try
            {
                AppConnect.Model1.products.Remove(p);
                AppConnect.Model1.SaveChanges();
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.user == null)
            {
                MessageBox.Show("Авторизуйте",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            AppFrame.mainFrame.Navigate(new CartPage());
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            if(AppConnect.user == null || AppConnect.user.id_role == 3)
            {
                MessageBox.Show("Раздел доступен менеджеру и администратору.",
                    "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.user = null;
            AppFrame.mainFrame.Navigate(new AuthorizationPage());
        }

        private void Filters_Changed(object sender, SelectionChangedEventArgs e) => Apply();

        private void Filters_Changed(object sender, TextChangedEventArgs e)
        {
            Apply();
        }
    }
}
