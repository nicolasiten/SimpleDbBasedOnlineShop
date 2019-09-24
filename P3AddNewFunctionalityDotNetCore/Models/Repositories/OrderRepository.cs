using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly P3Referential _context;

        public OrderRepository(P3Referential context)
        {
            _context = context;
        }

        public void Save(Order order)
        {
            if (order != null && OrderValid(order))
            {
                _context.Order.Add(order);
                _context.SaveChanges();
            }
        }

        public async Task<Order> GetOrder(int? id)
        {
            var orderEntity = await _context.Order.Include(x => x.OrderLine)
                .ThenInclude(product => product.Product).SingleOrDefaultAsync(m => m.Id == id);
            return orderEntity;
        }

        public async Task<IList<Order>> GetOrders()
        {
            var orders = await _context.Order.Include(x => x.OrderLine)
                .ThenInclude(product => product.Product).ToListAsync();
            return orders;
        }

        private bool OrderValid(Order order)
        {
            foreach (OrderLine line in order.OrderLine)
            {
                if (line.ProductId < 1 || 
                    !_context.Product.Any(p => p.Id == line.ProductId) || 
                    _context.Product.First(p => p.Id == line.ProductId).Quantity < line.Quantity ||
                    line.Quantity < 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}