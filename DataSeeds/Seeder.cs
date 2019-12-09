using Atlob_Dent.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class Seeder
    {
        private static readonly Atlob_dent_Context context = ServiceHelper.GetDbContext();
        private static readonly UserManager<ApplicationUser> _userManager = ServiceHelper.GetUserManager();
        private class customerType {
            public string fullName { get; set; }
            public string email { get; set; }
        };
        public static void SeedToAllTables()
        {
            SeedCustomers().Wait();
            SeedComments();
            SeedCompanies();
            SeedCategories();
            SeedProducts();
            SeedOnSales();
            SeedOrders();
        }       
        private static void SeedProducts()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "products.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Product> Products =
                JsonConvert.DeserializeObject<List<Product>>(jsonData);
            if (!context.Products.Any())
            {
                context.Products.AddRange(Products);
                context.SaveChanges();
            }
        }
        private static void SeedOrders()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "orders.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Order> Orders =
                JsonConvert.DeserializeObject<List<Order>>(jsonData);
            int i = 0;
            var customersId = context.Customers.Select(c => c.id).ToList();
            foreach (var order in Orders)
            {
                order.customerId = customersId.ElementAt(i);
                ++i;
                if (customersId.Count-1 < i) i = 0;
            }
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(Orders);
                context.SaveChanges();
            }
        }
        private static void SeedCategories()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "categories.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Category> Categories =
                JsonConvert.DeserializeObject<List<Category>>(jsonData);
            if (!context.Categories.Any())
            {               
                context.Categories.AddRange(Categories);
                context.SaveChanges();
            }
        }
        private static void SeedOnSales()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "onSales.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<OnSale> OnSales =
                JsonConvert.DeserializeObject<List<OnSale>>(jsonData);
            if (!context.OnSales.Any())
            {
                context.OnSales.AddRange(OnSales);
                context.SaveChanges();
            }
        }
        private static void SeedCompanies()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "companies.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Company> Companies =
                JsonConvert.DeserializeObject<List<Company>>(jsonData);
            if (!context.Companies.Any())
            {
                context.Companies.AddRange(Companies);
                context.SaveChanges();
            }
        }
        private static async Task SeedCustomers()
        {
            if (context.Customers.Any()) return;
                var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "customers.json");
            var jsonData = File.ReadAllText(fileJsonPath);
                var customers=JsonConvert.DeserializeObject<List<customerType>>(jsonData);
            var isConfimed = false;
            foreach (var customer in customers)
            {
                isConfimed = !isConfimed;
                var newUser = new ApplicationUser {
                Email= customer.email,
                UserName=customer.email,
                //SecurityStamp=Guid.NewGuid().ToString(),
                EmailConfirmed=isConfimed
                };
                await _userManager.CreateAsync(newUser);
                await _userManager.AddPasswordAsync(newUser, "Customer@123");               
                await _userManager.AddToRoleAsync(newUser, GlobalVariables.CustomerRole);
                var newCustomer = new Customer {
                    id = newUser.Id,
                    fullName= customer.fullName,
                    imgSrc="/images/Users",
                    User=newUser
                };
                context.Customers.Add(newCustomer);
            }
            context.SaveChanges();
        }
        private static void SeedComments()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "comments.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Comment> Comments =
                JsonConvert.DeserializeObject<List<Comment>>(jsonData);
            if (!context.Comments.Any())
            {
                context.Comments.AddRange(Comments);
                context.SaveChanges();
            }
        }
    }
}
