using Atlob_Dent.Data;
using Atlob_Dent.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Helpers
{
    public static class CustomerHelper
    {
        private static readonly Atlob_dent_Context _context = ServiceHelper.GetDbContext();
        /// <summary>
        /// check if customer is already exists
        /// if not exists ,new customer will be created based on order data
        /// </summary>
        /// <param name="cartCustomerModel"></param>
        /// <returns>created or the alraedy customer</returns>
        public static async Task<Tuple<Customer, TransactionHelper>> RegisterCustomerIfNotExists(CartCustomerModel cartCustomerModel,int ordersCount)
        {
            var registerCustomerTransAct = new TransactionHelper(_context);           
            if (_context.Customers.Any(c=>c.phone==cartCustomerModel.phone))
            {
                var registeredCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.phone == cartCustomerModel.phone);
                registeredCustomer.consumedProducts += ordersCount;
                _context.Entry(registeredCustomer).Property(p => p.consumedProducts).IsModified = true;
                registerCustomerTransAct.TakeAction(_context => {
                     _context.SaveChangesAsync().Wait();
                });                
                return new Tuple<Customer, TransactionHelper>(registeredCustomer, registerCustomerTransAct);
            }
            var user = UserHelper.createCustomerUserByPhone(cartCustomerModel.phone);
            var customer = new Customer
            {
                id = user.Id,
                fullName = cartCustomerModel.fullName,
                phone = cartCustomerModel.phone,
                consumedProducts = ordersCount
            };
            _context.Customers.Add(customer);
            registerCustomerTransAct.TakeAction(_context => {
                _context.SaveChangesAsync().Wait();
            });
            return new Tuple<Customer, TransactionHelper>(customer, registerCustomerTransAct);
        }
    }
}
