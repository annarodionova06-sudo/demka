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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();
            LoadOrders();
        }
        private void LoadOrders()
        { 
            ListOrders.ItemsSource = AppConnect.Model1.orders
                .Include("users")
                .Include("orders_main")
                .ToList()
                .OrderByDescending(o => o.date_order)
                .ToList();
        }
        private void listOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        => Edit_CLick(sender, e);

        private void Back_Click(object sender, StylusEventArgs e)
        {

        }

        private void Edit_CLick(object sender, RoutedEventArgs e)
        {
            if(AppConnect.user?.id_role != 1)
            {
                MessageBox.Show("Редоктирование доступно только администратору.",
                    "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;            
            }

            if (!(ListOrders.SelectedItems is orders o))
            {
                MessageBox.Show("Выберите заказ.", "Уведомление",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //AppFrame.mainFrame.Navigate(new AddOrderPage(o));
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(AppConnect.user?.id_role != 1 && AppConnect.user?.id_role != 2)
            {
                MessageBox.Show("Доступ ограничен", "Уведомление",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //AppFrame.mainFrame.Navigate(new AddOrderPage(null));
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if(AppConnect.user?.id_role != 1)
            {
                MessageBox.Show("Удаление доступно только администратору",
                    "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(!(ListOrders.SelectedItems is orders o)) return;

            if (MessageBox.Show($"Удалить заказ №{o.number_order}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes) return;

            try
            {
                foreach (var item in o.orders_main.ToList())
                {
                    var stock = AppConnect.Model1.stock_items.Find(item.id_stock_items);
                    if (stock != null) stock.quantity_for_order += item.quantity;
                }
                AppConnect.Model1.orders_main.RemoveRange(o.orders_main);
                AppConnect.Model1.orders.Remove(o);
                AppConnect.Model1.SaveChanges();
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
            => AppFrame.mainFrame.Navigate(new ProductsPage());
    }
}
