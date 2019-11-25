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
        public static void SeedToAllTables()
        {
            SeedCustomers().Wait();
            SeedComments();
            SeedCompanies();
            SeedCategories();
            SeedProducts();
            SeedOnSales();
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
            List<Customer> customers =
                JsonConvert.DeserializeObject<List<Customer>>(jsonData);
            foreach (var customer in customers)
            {
                var newUser = new ApplicationUser {
                PhoneNumber=customer.phone,
                SecurityStamp=Guid.NewGuid().ToString(),
                UserName=customer.phone
                };
                await _userManager.AddPasswordAsync(newUser, "Customer@123");
                await _userManager.CreateAsync(newUser);
                await _userManager.AddToRoleAsync(newUser, GlobalVariables.CustomerRole);
                var newCustomer = new Customer {
                    id = newUser.Id,
                    phone = customer.phone,
                    consumedProducts = customer.consumedProducts,
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
