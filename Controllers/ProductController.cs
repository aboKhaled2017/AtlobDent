using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlob_Dent.Data;
using Atlob_Dent.Helpers;
using Atlob_Dent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlob_Dent.Controllers
{
    [Route("api/Atlob_product/[action]")]
    [ApiController]
    public class ProductController : MainController
    {
        public ProductController(Atlob_dent_Context context, ILogger<MainController> logger) : base(context, logger)
        {
        }
        #region Get all products
        /// <summary>
        /// get all product data
        /// </summary>
        /// <example>dmain/api/Atlob_product/All</example>
        /// <returns>list of product</returns>
        public IActionResult All()
        {
            try
            {
                var data = _context.Products
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(HttpContext)
                 .ToList();
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        /// <summary>
        /// get particular page of the products
        /// </summary>
        /// <example>dmain/api/Atlob_product/AllWith?pageNumber=1&pageLenght=5</example>
        /// <param name="pageLength">page length</param>
        /// <param name="pageNumber">page number</param>
        /// <returns></returns>
        public IActionResult AllWith(int pageLength = 10, int pageNumber = 1)
        {
            try
            {
                var data = _context.Products
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(HttpContext);
                int total = data.Count();
                data = data.Skip((pageNumber - 1) * pageLength).Take(pageLength);
                return Ok(new
                {
                    data = data,
                    total = total,
                });
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        #endregion
        #region Get products of (specified category|specified company||specified company for specified category)
        /// <summary>
        /// get all products of specified category
        /// </summary>
        /// <example>dmain/api/Atlob_product/OfCategory/72c8b403-615e-4477-870e-fa7621b8b735</example>
        /// <param name="id">category id</param>
        /// <returns>list of products</returns>             
        [HttpGet("{id}")]
        public ActionResult OfCategory(Guid id)
        {
            try
            {
                var data = _context.Products
                 .Where(p => p.categoryId == id || p.category.SuperCategory.id == id)
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(HttpContext)
                 .ToList();
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        /// <summary>
        /// get particular page of products for specified category
        /// </summary>
        /// <example>domain/api/Atlob_product/OfCategory/72c8b403-615e-6666-870e-fa7621b8b735/with?pageLength=1&pageNumber=2</example>
        /// <param name="id">category id</param>
        /// <param name="pageLength">page length</param>
        /// <param name="pageNumber">page number</param>
        /// <returns></returns>
        [HttpGet("{id}/with")]
        public ActionResult OfCategory(Guid id, int pageLength = 10, int pageNumber = 1)
        {
            try
            {
                var data = _context.Products
                 .Where(p => p.categoryId == id || p.category.SuperCategory.id == id)
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(HttpContext);
                int total = data.Count();
                data = data.Skip((pageNumber - 1) * pageLength).Take(pageLength);
                return Ok(new
                {
                    data = data,
                    total = total,
                });
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        /// <summary>
        /// get product of specified company
        /// </summary>
        /// <example>domain/api/Atlob_product/OfCompany/72c8b403-615e-6666-870e-fa7621b8b735</example>
        /// <param name="id">company id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult OfCompany(Guid id)
        {
            try
            {
                var data = _context.Products
                 .Where(p => p.companyId == id)
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(HttpContext)
                 .ToList();
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        /// <summary>
        /// get specified size of products for particular company
        /// </summary>
        /// <example>domain/api/Atlob_product/OfCompany/72c8b403-615e-6666-870e-fa7621b8b735/with?pageNumber=2&pageLength=1</example>
        /// <param name="id">company id</param>
        /// <param name="pageLength">page lemght</param>
        /// <param name="pageNumber">page number</param>
        /// <returns></returns>
        [HttpGet("{id}/with")]
        public ActionResult OfCompany(Guid id, int pageLength = 10, int pageNumber = 1)
        {
            try
            {
                var data = _context.Products
                 .Where(p => p.companyId == id)
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(HttpContext);
                int total = data.Count();
                data = data.Skip((pageNumber - 1) * pageLength).Take(pageLength);
                return Ok(new
                {
                    data = data,
                    total = total,
                });
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        /// <summary>
        /// get products of particular category that made by particular company
        /// </summary>
        /// <example>domain/api/Atlob_product/OfCompanyForCategory/72c8b403-615e-6666-870e-fa7621b8b735/72c8b403-615e-6666-870e-fa7621b8b735</example>
        /// <param name="companyId">id of mader company</param>
        /// <param name="categoryId">category id</param>
        /// <returns></returns>
        [HttpGet("{companyId}/{categoryId}")]
        public ActionResult OfCompanyForCategory(Guid companyId, Guid categoryId)
        {
            try
            {
                var data = _context.Products
                 .Where(
                    p => p.companyId == companyId &&
                    (p.categoryId == categoryId ||
                    p.category.SubCategories.Any(sub => sub.id == categoryId)))
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(HttpContext)
                 .ToList();
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        ///<summary>
        /// get specified page of  products for particular category that made by particular company
        /// <example>domain/api/Atlob_product/OfCompanyForCategory/72c8b403-615e-6666-870e-fa7621b8b735/72c8b403-615e-6666-870e-fa7621b8b735/with?pageNumber=2&pageLength=2</example>
        /// </summary>
        /// <param name="companyId">id of mader company</param>
        /// <param name="categoryId">category id</param>
        /// <param name="pageLength">page length</param>
        /// <param name="pageNumber">page number</param>
        /// <returns></returns>
        [HttpGet("{companyId}/{categoryId}/with")]
        public ActionResult OfCompanyForCategory(Guid companyId, Guid categoryId, int pageLength = 10, int pageNumber = 1)
        {
            try
            {
                var data = _context.Products
                 .Where(
                    p => p.companyId == companyId &&
                    (p.categoryId == categoryId ||
                    p.category.SubCategories.Any(sub => sub.id == categoryId)))
                 ._selectCommonProductsStamp()
                 .GetExportedProductModels(HttpContext);
                int total = data.Count();
                data = data.Skip((pageNumber - 1) * pageLength).Take(pageLength);
                return Ok(new
                {
                    data = data,
                    total = total,
                });
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        #endregion
        #region Get All similar products
        /// <summary>
        /// get the similar products of the target product based on specified match
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="basedOn">the match based</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> OfSimilar(Guid id, SimilarProductsBasedOn basedOn = SimilarProductsBasedOn.type)
        {
            try
            {
                var product = (await _context.Products
                    .Include(p=>p.company)
                    .Include(p => p.category)
                    .ThenInclude(c => c.products)
                    .ToListAsync())//to load all products company
                    .FirstOrDefault(p => p.id == id);
                if (product == null)
                    return NotFound();
                var data = basedOn == SimilarProductsBasedOn.type
                    ? product._SimilarProductsByType()
                    : basedOn == SimilarProductsBasedOn.company
                    ? product._SimilarProductsByCompany()
                    : product._SimilarProductsByVersion();
                var typedData = data.AsQueryable().Include(p=>p.company)
                ._selectCommonProductsStamp()
                .GetExportedProductModels(HttpContext);
                return Ok(typedData);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        #endregion
        #region Get all products with specified criteria
        
        /// <summary>
        /// get the most ordered products
        /// </summary>
        /// <example>dmain/api/Atlob_product/MostBought</example>
        /// <param name="count">number of products to get most</param>
        /// <returns></returns>
        private IActionResult _MostBought(int by)
        {
            try
            {
                var data = _context.Products
                 .OrderByDescending(p => p.ordersCount)
                 .GetResponsePages(null, 0, by, HttpContext);
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        /// <summary>
        /// get specified page of the most ordered products
        /// </summary>
        /// <example>dmain/api/Atlob_product/MostBought?by=8&pageLength=4&pageNumber=2</example>
        /// <param name="by">the ceiling of got products to get most</param>
        /// <param name="pageLength"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]        
        public IActionResult MostBought(int by = 10, int? pageLength = null, int? pageNumber = null)
        {
            try
            {
                int totalOfProducts = _context.Products.Count();
                if (by > totalOfProducts) by = totalOfProducts;
                if (pageNumber==null&&pageLength==null) return _MostBought(by);
                 pageNumber =pageNumber ?? 1;
                 pageLength =pageLength ?? 10;
                if ((double)((pageNumber * pageLength) / pageLength) > Math.Ceiling((double)by / (int)pageLength))
                    return Ok("[]");
                if ((pageNumber * pageLength) > by) pageLength = by-(pageLength*(pageNumber-1));
                var data = _context.Products
                 .OrderByDescending(p => p.ordersCount)
                 .GetResponsePages(by, (((int)pageNumber - 1) * (int)pageLength), (int)pageLength, HttpContext);
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }

        /// <summary>
        /// get the most newlly created products
        /// </summary>
        /// <example>dmain/api/Atlob_product/NewllyCreated</example>
        /// <param name="count">the ceiling number of got the most newlly created products</param>
        /// <returns></returns>
        private IActionResult _NewllyCreated(int by)
        {
            try
            {
                var data = _context.Products
                 .OrderBy(p => p.createdDate)
                 .GetResponsePages(null, 0, by, HttpContext);
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        /// <summary>
        /// get specified page of the most newlly created products
        /// </summary>
        /// <example>dmain/api/Atlob_product/NewllyCreated?by=8&pageLength=4&pageNumber=2</example>
        /// <param name="by">the ceiling of got products to get most</param>
        /// <param name="pageLength"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult NewllyCreated(int by = 10, int? pageLength = null, int? pageNumber = null)
        {
            try
            {
                int totalOfProducts = _context.Products.Count();
                if (by > totalOfProducts) by = totalOfProducts;
                if (pageNumber == null && pageLength == null) return _NewllyCreated(by);
                pageNumber = pageNumber ?? 1;
                pageLength = pageLength ?? 10;
                if ((double)((pageNumber * pageLength) / pageLength) > Math.Ceiling((double)by / (int)pageLength))
                    return Ok("[]");
                if ((pageNumber * pageLength) > by) pageLength = by - (pageLength * (pageNumber - 1));
                var data = _context.Products
                 .OrderBy(p => p.ordersCount)
                 .GetResponsePages(by, (((int)pageNumber - 1) * (int)pageLength), (int)pageLength, HttpContext);
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        private IActionResult _OnSale()
        {
            try
            {
                var data = _context.OnSales
                 .OrderByDescending(p => p.discount)
                 ._selectCommonOnSaleProductsStamp()
                 .GetExportedOnSaleProductModels(HttpContext)
                 .ToList();
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        /// <summary>
        /// get all on sale products
        /// or get specified page of on sale products
        /// </summary>
        /// <example>dmain/api/Atlob_product/onSale</example>
        /// <example>dmain/api/Atlob_product/onSale?pageLength=4&pageNumber=2</example>
        /// <param name="pageLength"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public IActionResult OnSale(int? pageLength = null, int? pageNumber = null)
        {
            try
            { 
                if (pageNumber == null && pageLength == null) return _OnSale();
                int totalOfProducts = _context.OnSales.Count();
                pageNumber = pageNumber ?? 1;
                pageLength = pageLength ?? 10;
                var maxPageNumber = Math.Ceiling((totalOfProducts / (double)pageLength));
                if (pageNumber > maxPageNumber)
                    pageNumber = (int)maxPageNumber;
                var data = _context.OnSales
                 .OrderByDescending(p => p.discount)
                 .GetResponsePages(totalOfProducts,(((int)pageNumber - 1) * (int)pageLength), (int)pageLength, HttpContext);
                return Ok(data);
            }
            catch
            {
                return BadRequest(new BadResponseResult());
            }
        }
        #endregion
        #region Search for products
        /// <summary>
        /// helper function for searching products
        /// </summary>
        /// <param name="str">matching string</param>
        /// <returns></returns>
        private IEnumerable<Product> searchFor(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new List<Product>();
            if (_context.Products.AnySearchMatch(str))
                return _context.Products.SearchFor(str);
            if (str.Length > 0 && _context.Companies.AnySearchMatch(str))
                return _context.Companies
                       .Include(c => c.products)
                       .SearchFor(str);
            if (str.Length > 0 && _context.Categories
                                   .Include(c=>c.SubCategories)
                                   .ThenInclude(c=>c.products)
                                   .ToList()
                                  .AnySearchMatch(str))
                return _context.Categories
                       .Include(c => c.SubCategories)
                       .ThenInclude(c => c.products)
                       .SearchFor(str);
            return new List<Product>();


        }
        private IActionResult _BySearch(string s)
        {
            try
            {
                s = s.Trim();
                var data = searchFor(s)
                    ._selectCommonProductsStamp()
                    .GetExportedProductModels(HttpContext)
                    .ToList();
                return Ok(data);
            }
            catch
            {
                return Ok(new BadResponseResult());
            }
        }
        /// <summary>
        /// search for products by 
        /// company|category|product name
        /// </summary>
        /// <example>domain/api/Atlob_product/BySearch?s=product 1</example>
        /// <param name="s">matching string</param>
        /// <returns>set of matched products</returns>
        public IActionResult BySearch(string s, int? pageLength = null, int? pageNumber = null)
        {
            try
            {
                s = s.Trim();
                if (pageNumber == null && pageLength == null) return _BySearch(s);
                var data = searchFor(s).AsQueryable().OrderBy(p=>p.ordersCount);                   
                int totalOfProducts = data.Count();
                pageNumber = pageNumber ?? 1;
                pageLength = pageLength ?? 10;
                var maxPageNumber = Math.Ceiling((totalOfProducts / (double)pageLength));
                if (pageNumber > maxPageNumber)
                    pageNumber = (int)maxPageNumber;
                var obj = data.GetResponsePages(totalOfProducts, (((int)pageNumber - 1) * (int)pageLength), (int)pageLength, HttpContext);
                return Ok(obj);
            }
            catch
            {
                return Ok(new BadResponseResult());
            }
        }
        #endregion
    }
}
