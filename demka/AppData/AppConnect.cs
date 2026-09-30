using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demka.AppData
{
    internal class AppConnect
    {
        public static demkaEntities Model1;
        public static users user;

        public static bool SelectProductMode;
        public static orders EditingOrder;
    }
    public partial class users
    {
        public string FIO => $"{surname} {name} {patronymic}".Trim();
    }
    public partial class OrderItems
    {
        //public decimal Sum => (decimal)(Quantity )
    }
    public partial class products
    {
        public bool HasDiscount
        {
            get
            {
                DateTime from = DateTime.Now.AddMonths(-1);
                return !AppConnect.Model1.orders_main.Any(oi =>
                oi.stock_items.id_product == id &&
                oi.orders.date_order >= from &&
                oi.orders.date_order < DateTime.Now);
            }
        }
        public int TotalQuantity => (int)stock_items.Sum(s => s.quantity_for_order);
        public decimal FinalPrice => HasDiscount
           ? (decimal)(Math.Round(Convert.ToDouble(price * 0.75), 2))
            : (decimal)price;
        public string ImagePath => string.IsNullOrEmpty(image)
            ? "/Images/picture.png"
            : $"/Images/{image}";
    }
}
