using HomeWorks.Models;
using HomeWorks.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeWorks.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogInController
        private readonly RS0605Context _context;

        public LogInController(RS0605Context context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: LogInController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LogInController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LogInController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LogInController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LogInController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LogInController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LogInController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: LogInController/Login
        [HttpPost]
        public async Task<IActionResult> Login(Member member)
        {
            string account = member.MeId;
            string pwd = member.MePassword;

            var user = await _context.Members
                .FirstOrDefaultAsync(x => x.MeId == account && x.MePassword == pwd);

            if (user == null)
            {
                ViewData["ErrorMsg"] = "帳號/密碼輸入錯誤";
                return View("Index");
            }

            // 在用戶存在時設置 session
            var photoPath = user.PhotoPath ?? "/photo/ready.png"; // 使用默認值防止 null
            HttpContext.Session.SetString("PhotoPath", photoPath);
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("LoggedInID", account);
            HttpContext.Session.SetString("UserRole", user.PreId); // 例如，設定為 B

            // 設置認證
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, account),
        new Claim("Role", "Admin"),  // 根據實際需求調整角色
        new Claim("Subscription", "Premium") // 根據實際需求調整訂閱類型
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(Url.Content("~/"));
        }



    

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
