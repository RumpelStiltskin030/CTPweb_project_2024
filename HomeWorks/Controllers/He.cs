using Microsoft.AspNetCore.Mvc;
using HomeWorks.Models;
using HomeWorks.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWorks.Controllers
{
    public class HeController : Controller
    {
        private readonly RS0605Context _context;

        public HeController(RS0605Context context)
        {
            _context = context;
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
    }
}
