using Atlob_Dent.Data;
using Atlob_Dent.Helpers;
using Atlob_Dent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Services
{
    public class OrdersService
    {
        private Atlob_dent_Context _context { get;}
        private CartCheckoutModel _cartCheckoutModel { get; set; }
        private Customer _orderMakerCustomer { get; set; }
        public  OrdersService(Atlob_dent_Context context,CartCheckoutModel cartCheckoutModel)
        {
            _context = context;
            _cartCheckoutModel = cartCheckoutModel;
        }
        public async Task<ResponseResult> ProcessOrderRequest()
        {
            var registerNewCustomerTransAct = ServiceHelper.GetTransactionHelper();
            try
            {
                var customer = await CustomerHelper.RegisterCustomerIfNotExists(this._cartCheckoutModel.customer,this._cartCheckoutModel.orders.Count);
                if (!await RegisterOrdersForCustomer(_cartCheckoutModel, customer))
                {
                    registerNewCustomerTransAct.RollBackChanges();
                    return new BadResponseResult{message="order didn't successfully precessed"};
                }
                    registerNewCustomerTransAct.CommitChanges();
                return new SuccessResponseResult { message = "order has been successfully created" };
            }
            catch(Exception ex)
            {
                registerNewCustomerTransAct.RollBackChanges();
                return new BadResponseResult {message=ex.Message };
            }

        }
        private async Task<bool> RegisterOrdersForCustomer(CartCheckoutModel cartCheckoutModel, Customer customer)
        {          
            foreach (var orderModel in cartCheckoutModel.orders)
            {
                var product = _context.Products.Find(orderModel.productId);
                product.ordersCount += 1;
                product.consumedCount += orderModel.quantity;
                _context.Entry(product).Property(p => p.ordersCount).IsModified = true;
                _context.Entry(product).Property(p => p.consumedCount).IsModified = true;
                var newOrder = new Order
                {
                    id = Guid.NewGuid(),
                    address = cartCheckoutModel.customer.address,
                    customerId = customer.id,
                    productId = orderModel.productId,
                    quantity = orderModel.quantity,
                    sizeIndex = orderModel.sizeIndex
                };
                _context.Orders.Add(newOrder);
            }
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
