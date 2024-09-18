using Microsoft.AspNetCore.Mvc;
using HomeWorks.Models;
using HomeWorks.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using HomeWorks.Configuration;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HomeWorks.Controllers
{
    public class HeController : Controller
    {
        private readonly RS0605Context _context;

        public HeController(RS0605Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var me = await _context.Members.Where(p => p.PreId == "B").ToListAsync();
            return View(me);
        }

        // GET: Members
        public async Task<IActionResult> HeIndex(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get member details
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MeId == id);

            if (member == null)
            {
                return NotFound();
            }

            // Get products for the member
            var products = await _context.Products
                .Include(p => p.ProductDetails)
                .Where(p => p.MeId == id)
                .ToListAsync();

            // Map products to ProductVM
            var productVMs = products.Select(p => new ProductVM
            {
                Product = p,
                ProductDetail = p.ProductDetails.ToList()
            }).ToList();

            // Create a view model containing both member and products
            var viewModel = new MemberProductsViewModel
            {
                Member = member,
                Products = productVMs
            };

            return View(viewModel);
        }

        public IActionResult OrderHe()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderHe(OrderVM orderVm, string id, IFormFile photo)
        {
            var meid = HttpContext.Session.GetString("LoggedInID") ?? "";
            var memberInfo = await _context.Members.FindAsync(meid);
            if (memberInfo == null)
            {
                ViewData["Message"] = "會員資料未找到";
                return View();
            }

            string orderId = fairy.CreartOrderId();
            string proId = fairy.GenerateUniqueId();

            // 處理圖像檔案
            if (photo != null && photo.Length > 0)
            {
                // 檢查圖像檔案類型
                if (photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
                {
                    ViewData["Message"] = "請上傳 jpg 或 png 檔案";
                    return View();
                }

                // 生成唯一的檔案名稱
                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(photo.FileName)}";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "seed", uniqueFileName);

                // 上傳圖像檔案
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                // 處理產品、訂單和訂單明細
                try
                {
                    // 查找產品是否存在，如果存在則更新，否則新增
                    var existingProduct = await _context.Products.FindAsync(proId);
                    if (existingProduct != null)
                    {
                        // 更新產品
                        existingProduct.Status = "D";
                        existingProduct.ProPrice = 0; // 設定價格為0
                        existingProduct.MeId = id;
                        existingProduct.ProName = memberInfo.MeName;
                        _context.Products.Update(existingProduct);
                    }
                    else
                    {
                        // 新增產品
                        var product = new Product
                        {
                            ProId = proId,
                            Status = "D",
                            ProPrice = 0,
                            MeId = id,
                            ProName = memberInfo.MeName,
                        };
                        _context.Products.Add(product);
                    }
                    await _context.SaveChangesAsync();

                    // 新增訂單
                    var order = new Order
                    {
                        OrdId = orderId,
                        OrdDate = DateTime.Now,
                        MeId = meid,
                        SeleId = id,
                        Status = "D"
                    };
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    // 新增訂單明細
                    var orderDetail = new OrderDetail
                    {
                        Fieldpath = $"/seed/{uniqueFileName}",
                        OrdId = orderId,
                        Pricing = 0,
                        ProId = proId,
                        Specification = orderVm.OrderDetail?.Specification // 使用 Null Conditional Operator
                    };
                    _context.OrderDetails.Add(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // 捕捉並處理並發錯誤
                    Console.WriteLine($"Concurrency error: {ex.Message}");
                    ViewData["Message"] = "資料已被修改或刪除，請重試。";
                    return View();
                }
                catch (Exception ex)
                {
                    // 捕捉其他可能的錯誤
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    ViewData["Message"] = $"發生錯誤: {ex.Message}";
                    return View();
                }
            }
            else
            {
                ViewData["Message"] = "請上傳圖像檔案";
                return View();
            }

            return Redirect($"/He/HeIndex/{id}");
        }

    }
}

