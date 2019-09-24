using System;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace P3AddNewFunctionalityDotNetCore.Models
{
    public class Cart : ICart
    {
        private readonly List<CartLine> _cartLines;

        public Cart()
        {
            _cartLines = new List<CartLine>();
        }

        public void AddItem(Product product, int quantity)
        {
            CartLine line = _cartLines.FirstOrDefault(p => p.Product.Id == product.Id);

            if (line == null)
            {
                _cartLines.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product) => _cartLines.RemoveAll(l => l.Product.Id == product.Id);

        public double GetTotalValue()
        {
            double totalValue = 0;
            foreach (CartLine item in _cartLines)
            {
                totalValue += item.Product.Price * item.Quantity;
            }

            return totalValue;
        }

        /// <summary>
        /// Get average value of a cart
        /// </summary>
        public double GetAverageValue()
        {
            int numberOfProducts = 0;
            foreach (CartLine item in _cartLines)
            {
                numberOfProducts += item.Quantity;
            }

            return numberOfProducts > 0 ? GetTotalValue() / numberOfProducts : 0;
        }

        public void Clear() => _cartLines.Clear();

        public IEnumerable<CartLine> Lines => _cartLines;
    }

    public class CartLine
    {
        public int OrderLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
