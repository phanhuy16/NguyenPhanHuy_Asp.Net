using System;
using System.Collections.Generic;
using System.Linq;

namespace NguyenPhanHuy_2122110062.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; }
        public ShoppingCart()
        {
            this.Items = new List<CartItem>();
        }

        public void AddToCart(CartItem item, int Quantity)
        {
            var checkExist = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExist != null)
            {
                checkExist.Quantity += Quantity;
                checkExist.TotalPrice = checkExist.Quantity * checkExist.Price;
            }
            else
            {
                item.Quantity = Quantity;
                item.TotalPrice = item.Price * item.Quantity;
                Items.Add(item);
            }
        }

        public void Delete(Guid id)
        {
            var checkExist = Items.FirstOrDefault(x => x.ProductId == id);
            if (checkExist != null)
            {
                Items.Remove(checkExist);
            }
        }

        public void Update(Guid id, int Quantity)
        {
            var checkExist = Items.FirstOrDefault(x => x.ProductId == id);
            if (checkExist != null)
            {
                checkExist.Quantity = Quantity;
                checkExist.TotalPrice = checkExist.Quantity * checkExist.Price;
            }
        }

        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.TotalPrice);
        }

        public int GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }

        public void ClearCart()
        {
            Items.Clear();
        }

    }
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string CategoryName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}