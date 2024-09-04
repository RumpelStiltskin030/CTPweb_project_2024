using HomeWorks.Models;
using HomeWorks.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace HomeWorks.Controllers
{
    public class ProController : Controller
    {
        private readonly RS0605Context _context;

        public ProController(RS0605Context context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var rS0605Context = _context.Products.Include(p => p.Me).Include(p => p.ProductDetails);
            List<ProductVM> productVMs = new List<ProductVM>();
            foreach (var item in rS0605Context.ToList())
            {
                ProductVM tempvm = new ProductVM();
                tempvm.Product = item;
                tempvm.ProductDetail = item.ProductDetails.ToList();
                productVMs.Add(tempvm);
            }
            return View(productVMs);
        }

        public async Task<IActionResult> Details(string id)
        {
            var rS0605Context = _context.Products.Include(p => p.Me).Include(p => p.ProductDetails);
            List<ProductVM> productVMs = new List<ProductVM>();
            foreach (var item in rS0605Context.ToList())
            {
                if (item.ProId == id)
                {
                    ProductVM tempvm = new ProductVM();
                    tempvm.Product = item;
                    tempvm.ProductDetail = item.ProductDetails.ToList();
                    productVMs.Add(tempvm);
                }
            }
            return View(productVMs);

        }

        public IActionResult Orders(string id)
        {
            var rS0605Context = _context.Products.Include(p => p.Me).Include(p => p.ProductDetails).FirstOrDefault(x => x.ProId == id);
            var productVMs = new ProductVM();
            productVMs.Product = rS0605Context;
            productVMs.ProductDetail = rS0605Context?.ProductDetails?.ToList();
            
            return View(productVMs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Orders(string id, Product product)
        {
            var meid = HttpContext.Session.GetString("LoggedInID") ?? "";
            var orderId = fairy.CreartOrderId();
            if (product != null)
            {
                var productInfo = _context.Products.FirstOrDefault(x => x.ProId == product.ProId);
                productInfo.Status = "B";
                _context.Products.Update(productInfo);
                await _context.SaveChangesAsync();

                var order = new Order
                {
                    OrdId = orderId,
                    OrdDate = DateTime.Now,
                    MeId = meid,
                    SeleId = productInfo.MeId,
                    Status = "B"
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                var orderDetail = new OrderDetail
                {
                    OrdId = orderId,
                    Pricing = productInfo.ProPrice,
                    ProId = productInfo.ProId,
                    Specification = productInfo.ProName
                };
                _context.OrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        
    }
}
