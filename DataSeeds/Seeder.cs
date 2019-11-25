using Atlob_Dent.Data;
using Microsoft.AspNetCore.Hosting;
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
        public static void SeedToAllTables()
        {
            SeedUsers();
            SeedAdmins();
            SeedComments();
            SeedCompanies();
            SeedCategories();
            SeedProducts_secondWay();
            //SeedProducts_firstWay();
            SeedOnSales();
        }
        public static void SeedProducts_firstWay(List<Guid> categorysId, List<Guid> companiesId)
        {
            List<Product> products = new List<Product>() { };
            int i = 0;
            foreach (var categoryId in categorysId)
            {
                foreach (var companyId in companiesId)
                {
                    i += 2;
                    products.AddRange(new List<Product>() {
                    new Product
                     {
                         id=Guid.NewGuid(),
                         name=string.Format("product{0}",i),
                         categoryId=categoryId,
                         prices="[7,8]", 
                         seen=1,
                         images_url="",
                         companyId=companyId
                     },
                    new Product
                     {
                         id=Guid.NewGuid(),
                         name=string.Format("product{0}",i+1),
                         categoryId=categoryId,
                         prices="[1,4]",
                         seen=7,
                         images_url="",
                         companyId=companyId
                     }
                    });
                }
            }
            var context = ServiceHelper.GetDbContext();
            if (!context.Products.Any())
            {
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
        public static void SeedProducts_secondWay()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "products.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Product> Products =
                JsonConvert.DeserializeObject<List<Product>>(jsonData);
            var context = ServiceHelper.GetDbContext();
            if (!context.Products.Any())
            {
                context.Products.AddRange(Products);
                context.SaveChanges();
            }
        }
        public static void SeedCategories()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "categories.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Category> Categories =
                JsonConvert.DeserializeObject<List<Category>>(jsonData);
            var context = ServiceHelper.GetDbContext();
            if (!context.Categories.Any())
            {               
                context.Categories.AddRange(Categories);
                context.SaveChanges();
            }
        }
        public static void SeedOnSales()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "onSales.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<OnSale> OnSales =
                JsonConvert.DeserializeObject<List<OnSale>>(jsonData);
            var context = ServiceHelper.GetDbContext();
            if (!context.OnSales.Any())
            {
                context.OnSales.AddRange(OnSales);
                context.SaveChanges();
            }
        }
        public static void SeedCompanies()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "companies.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Company> Companies =
                JsonConvert.DeserializeObject<List<Company>>(jsonData);
            var context = ServiceHelper.GetDbContext();
            if (!context.Companies.Any())
            {
                context.Companies.AddRange(Companies);
                context.SaveChanges();
            }
        }
        public static void SeedUsers()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "users.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<User> Users =
                JsonConvert.DeserializeObject<List<User>>(jsonData);
            var context = ServiceHelper.GetDbContext();
            if (!context.Users.Any())
            {
                context.Users.AddRange(Users);
                context.SaveChanges();
            }
        }
        public static void SeedAdmins()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "admins.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Admin> Admins =
                JsonConvert.DeserializeObject<List<Admin>>(jsonData);
            var context = ServiceHelper.GetDbContext();
            if (!context.Admins.Any())
            {
                context.Admins.AddRange(Admins);
                context.SaveChanges();
            }
        }
        public static void SeedComments()
        {
            var fileJsonPath = Path.Combine(ServiceHelper.GetHostingEnv().ContentRootPath, "DataSeeds", "comments.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<Comment> Comments =
                JsonConvert.DeserializeObject<List<Comment>>(jsonData);
            var context = ServiceHelper.GetDbContext();
            if (!context.Comments.Any())
            {
                context.Comments.AddRange(Comments);
                context.SaveChanges();
            }
        }
    }
}
