using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeWorks.Models;
using HomeWorks.ViewModel;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HomeWorks.Controllers
{
    public class ProductsController : Controller
    {
        private readonly RS0605Context _context;

        public ProductsController(RS0605Context context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var rS0605Context = _context.Products.Include(p => p.Me).Include(p=>p.ProductDetails);
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


        // GET: Products/Details/5
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

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["MeId"] = new SelectList(_context.Members, "MeId", "MeId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile[] photo)
        {
            Console.WriteLine(product);
            Console.WriteLine("接到的field長度:" + photo.Length);

            try
            {
                string id = fairy.GenerateUniqueId();
                string meId = HttpContext.Session.GetString("LoggedInID") ?? "";

                product.ProId = id;
                product.MeId = meId;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                string productId = product.ProId;
                // 處理圖像檔案
                if (photo != null && photo.Length > 0)
                {
                    foreach (var photoItem in photo)
                    {
                        // 檢查圖像檔案類型
                        if (photoItem.ContentType != "image/jpeg" && photoItem.ContentType != "image/png")
                        {
                            ViewData["Message"] = "請上傳 jpg 或 png 檔案";
                            return View(product);
                        }

                        // 生成唯一的檔案名稱
                        string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(photoItem.FileName)}";
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                        // 上傳圖像檔案
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photoItem.CopyToAsync(stream);
                        }


                        var productDetail = new ProductDetail
                        {
                            ProId = productId,  // 根據你的實際需求設定
                            Fieldpath = $"/images/{uniqueFileName}"
                        };

                        _context.ProductDetails.Add(productDetail);

                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create錯誤: " + ex);
                return RedirectToAction("Index");
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["MeId"] = new SelectList(_context.Members, "MeId", "MeId", product.MeId);
            return PartialView("Edit", product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProId,MeId,ProName,ProPrice")] Product product)
        {
            if (product.ProId != product.ProId)
            {
                return NotFound();
            }

                try
                {
                    string meId = HttpContext.Session.GetString("LoggedInID") ?? "";
                    product.MeId = meId;
                    product.ProId = product.ProId;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
        }

        

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Me)
                .FirstOrDefaultAsync(m => m.ProId == id);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView("Delete", product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, [Bind("ProId")] Product products)
        {
            var product = await _context.Products
            .Include(p => p.ProductDetails) // 包含產品細節
            .FirstOrDefaultAsync(m => m.ProId == products.ProId);

            if (product != null)
            {
                // 移除產品細節
                _context.ProductDetails.RemoveRange(product.ProductDetails);
                // 移除產品
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    


        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.ProId == id);
        }
    }
}
