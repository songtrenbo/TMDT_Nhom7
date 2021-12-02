using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDT.Models
{
    public class CartItem
    {
        public SanPham _sanPham { get; set; }
        public int _quantity { get; set; }
    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public void Add_Product_Cart(SanPham _sp, int _quan = 1)
        {
            var item = Items.FirstOrDefault(s => s._sanPham.MaSanPham == _sp.MaSanPham);
            if (item == null)
                items.Add(new CartItem
                {
                    _sanPham = _sp,
                    _quantity = _quan
                });
            else
                item._quantity += _quan;
        }
        public int Total_quantity()
        {
            return items.Sum(s => s._quantity);
        }
        public decimal Total_money()
        {
            var total = items.Sum(s => s._quantity * s._sanPham.GiaBan);
            return (decimal)total;
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
}