using HomeWorks.Models;
using HomeWorks.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static NuGet.Packaging.PackagingConstants;

namespace HomeWorks.Controllers
{
    public class MeController : Controller
    {
        private readonly RS0605Context _context;

        public MeController(RS0605Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail()
        {
            string id = HttpContext.Session.GetString("LoggedInID");

            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MeId == id);
            if (member == null)
            {
                return NotFound();
            }
            var permission = await _context.Permissions.FirstOrDefaultAsync(vo => member.PreId == vo.PreId);

            memberVM memberVM = new memberVM();
            memberVM.member = member;
            memberVM.preName = permission.PreName;

            return View(memberVM);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            var permission = await _context.Permissions.FirstOrDefaultAsync(vo => member.PreId == vo.PreId);

            memberVM memberVM = new memberVM();
            memberVM.member = member;
            memberVM.preName = permission.PreName;

            return View(memberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MeId,MeName,MeTel,MeEmail")] Member member, IFormFile photo)
        {
            string memberId = member.MeId.Trim();
            if (id != memberId)
            {
                return NotFound();
            }

            var memberInfo = await _context.Members.FindAsync(memberId);

            try
            {
                // 處理圖像檔案
                if (photo != null && photo.Length > 0)
                {
                    // 檢查圖像檔案類型
                    if (photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
                    {
                        ViewData["Message"] = "請上傳 jpg 或 png 檔案";
                        return View(member);
                    }

                    // 生成唯一的檔案名稱
                    string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(photo.FileName)}";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photo", uniqueFileName);

                    // 上傳圖像檔案
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    // 更新會員資料中的圖像路徑
                    memberInfo.PhotoPath = $"/photo/{uniqueFileName}";
                }

                memberInfo.MeEmail = member.MeEmail;
                memberInfo.MeName = member.MeName;
                memberInfo.MeTel = member.MeTel;

                // 更新資料庫
                _context.Update(memberInfo);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("PhotoPath", memberInfo.PhotoPath);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(member.MeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            

            var permission = await _context.Permissions.FirstOrDefaultAsync(vo => memberInfo.PreId == vo.PreId);

            memberVM memberVM = new memberVM();
            memberVM.member = memberInfo;
            memberVM.preName = permission != null? permission.PreName : "沒有資料";

            return View(memberVM);
        }

        private bool MemberExists(string id)
        {
            return _context.Members.Any(e => e.MeId == id);
        }

        public async Task<IActionResult> ChangePermission(string id, string newPermissionId)
        {
            // 檢查是否有空的 id 或 newPreId
            id = id.Trim();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newPermissionId))
            {
                return BadRequest("Invalid parameters.");
            }

            // 查找會員
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            // 更新會員的權限 ID
            member.PreId = newPermissionId;

            try
            {
                // 更新會員資料
                _context.Update(member);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // 檢查會員是否存在
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    // 重新拋出異常以便進一步處理
                    throw;
                }
            }

            // 操作成功，返回某個視圖或重定向
            return RedirectToAction("Edit", "Me", new { id = id });
        }

        public async Task<IActionResult> OrderHis(string id)
        {
            List<Order> orderList = _context.Orders.Where(o => o.MeId == id).ToList();
            return View(orderList);
        }

        public IActionResult OrderMng(string id) 
        {
            return View();
        }

        public async Task<IActionResult> OrderDet(string id)
        {
            OrderVM orderVM = new OrderVM();
            var order =  _context.Orders
                .Where(o => o.OrdId == id)
                .Include(o => o.Me)
                .Include(o => o.OrderDetails)
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            var saler = await _context.Members.FirstOrDefaultAsync(x => x.MeId == order.SeleId);
            var buy = await _context.Members.FirstOrDefaultAsync(x => x.MeId == order.MeId);

            orderVM.Order = order;
            orderVM.saleMember =  saler;
            orderVM.buyMember = buy;


            return PartialView(orderVM);

        }

        // GET: Products/Delete/5
        public async Task<IActionResult> OrderDel(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(p => p.Me)
                .FirstOrDefaultAsync(m => m.OrdId == id);
            if (order == null)
            {
                return NotFound();
            }

            return PartialView("OrderDel", order);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("OrderDel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderDelConfirmed(string id, [Bind("OrdId")] Order orders)
        {

            var order = await _context.Orders
            .Include(p => p.OrderDetails) // 包含產品細節
            .FirstOrDefaultAsync(m => m.OrdId == orders.OrdId);

            if (order != null)
            {
                // 移除產品細節
                _context.OrderDetails.RemoveRange(order.OrderDetails);
                // 移除產品
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                var productInfo = _context.Products.FirstOrDefault(x => x.ProId == order.OrderDetails.First().ProId);
                productInfo.Status = "A";
                _context.Products.Update(productInfo);
                await _context.SaveChangesAsync();

            }



            string meId = HttpContext.Session.GetString("LoggedInID");

            return RedirectToAction("OrderHis", new { id = meId });
        }

        public async Task<IActionResult> OrderSale(string id)
        {
            var rS0605Context = _context.Orders
                .Where(p => p.SeleId == id)
                .Include(p => p.OrderDetails);
            List<OrderVM> orderVMs = new List<OrderVM>();
            foreach (var item in rS0605Context) 
            {
                OrderVM orderVM = new OrderVM();
                orderVM.Order = item;
                orderVM.OrderDetail = item.OrderDetails.First();
                orderVMs.Add(orderVM);
            }

            return View(orderVMs);
        }


        public async Task<IActionResult> Sale(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return PartialView(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sale(string id, DateTime DateLine)
        {
            // 檢查傳入的日期是否在 SQL Server 支持的範圍內
            if (DateLine < new DateTime(1753, 1, 1) || DateLine > new DateTime(9999, 12, 31))
            {
                ModelState.AddModelError("", "日期超出範圍。");
                return BadRequest(ModelState);
            }

            // 從數據庫中查找對應的訂單及其明細
            var order = await _context.Orders
                .Where(p => p.OrdId == id)
                .Include(p => p.OrderDetails)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound(); // 如果訂單未找到，返回 404 Not Found
            }

            // 更新訂單的寄出時間和其他必要的屬性
            order.DateLine = DateLine; // 設置寄出時間
            order.Status = "C"; // 設置訂單狀態為 'C'

            // 遍歷訂單明細，更新產品狀態
            foreach (var detail in order.OrderDetails)
            {
                var product = await _context.Products.FindAsync(detail.ProId);
                if (product != null)
                {
                    product.Status = "C";
                    _context.Products.Update(product);
                }
            }

            try
            {
                // 標記訂單和產品為已更新
                _context.Update(order);

                // 保存更改到數據庫
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // 捕獲異常並記錄詳細信息
                // 你可以記錄錯誤，並返回適當的錯誤消息
                Console.WriteLine($"更新資料時發生錯誤: {ex.Message}");
                return StatusCode(500, "更新資料時發生錯誤。"); // 返回 500 內部服務器錯誤
            }

            // 獲取登錄用戶的 ID
            string meId = HttpContext.Session.GetString("LoggedInID");

            // 重定向到 OrderSale 操作
            return RedirectToAction("OrderSale", new { id = meId });
        }


    }
}
