using HomeWorks.Models;

namespace HomeWorks.ViewModel
{
    public class MemberProductsViewModel
    {
        public Member Member { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
    }
}
