using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong.Models
{
    public class CartItem
    {
        public SANPHAM product { get; set; }
        public int quantity { get; set; }
        public Capacity capacity { get; set; }
        public Color color { get; set; }
        public decimal Gia { get; set; }
    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public void Add_Product_Cart(SANPHAM pro, Capacity cap, Color cl, decimal gia, int quant = 1)
        {
            var item = Items.FirstOrDefault(s => s.product.MaSP == pro.MaSP);
            var dungluong = Items.FirstOrDefault(s => s.capacity.CapacityID == cap.CapacityID);
            var mau = Items.FirstOrDefault(s => s.color.ColorID == cl.ColorID);
            if (item == null)
            {
                items.Add(new CartItem { product = pro, quantity = quant, capacity = cap, color = cl, Gia = gia });
            }
            else
            {
                if (dungluong == null)
                    items.Add(new CartItem { product = pro, quantity = quant, capacity = cap, color = cl, Gia = gia });
                else if (mau == null)
                    items.Add(new CartItem { product = pro, quantity = quant, capacity = cap, color = cl, Gia = gia });
                else
                    item.quantity += quant;
            }
        }
        public int Total_quantity()
        {
            return Items.Sum(s => s.quantity);
        }
        public decimal Total_money()
        {
            return (decimal)Items.Sum(s => s.Gia * s.quantity);
        }
        public decimal Total_money_SP(int id)
        {
            decimal total = 0;
            foreach (var itemWithIndex in Items.Select((item, index) => new { Item = item, Index = index }))
            {
                var capacitySP = itemWithIndex.Item;
                int index = itemWithIndex.Index;
                if (index == id)
                {
                    total += capacitySP.Gia * capacitySP.quantity;
                }
            }
            return total;
        }
        public void Update_quantity(int id, int new_quan)
        {
            //var item = items.Find(s => s.product.MaSP == id);
            //if (item != null)
            //{
            //    item.quantity = new_quan;
            //}
            foreach (var itemWithIndex in Items.Select((item, index) => new { Item = item, Index = index }))
            {
                var capacitySP = itemWithIndex.Item;
                int index = itemWithIndex.Index;
                if (index == id)
                {
                    capacitySP.quantity = new_quan;
                }
            }
        }
        public void Remove_CartItem(int id)
        {
            items.RemoveAt(id);
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
}