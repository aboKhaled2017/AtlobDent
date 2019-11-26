using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlob_Dent.Data;
using Atlob_Dent.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Atlob_Dent.Helpers
{
    public class OrderHelper 
    {
        private  Atlob_dent_Context _context { get; set; }
        private IDbContextTransaction _CreateCustomerOrdesTransaction { get; set; }
        public OrderHelper(Atlob_dent_Context context)
        {
            _context = context;
            _CreateCustomerOrdesTransaction = _context.Database.BeginTransaction();
        }
        
        public  async Task<bool> RegisterOrdersForCustomer(CartCheckoutModel cartCheckoutModel,Customer customer)
        {
            bool IsOrdersCreated = false;
            using(var CreateOrderTransaction=_context.Database.BeginTransaction())
            foreach (var orderModel in cartCheckoutModel.orders)
            {
                var newOrder = new Order { 
                id=Guid.NewGuid(),
                address=cartCheckoutModel.customer.address,
                customerId=customer.id,
                productId=orderModel.productId,
                quantity=orderModel.quantity,
                sizeIndex=orderModel.sizeIndex
                };
                _context.Orders.Add(newOrder);
            }
            IsOrdersCreated=(await _context.SaveChangesAsync())>0;
            _CreateCustomerOrdesTransaction.Commit();
            return IsOrdersCreated;
        }
        public void RollBack_OrdersForCustomer()
        {
            _CreateCustomerOrdesTransaction.Rollback();
        }
    }
}