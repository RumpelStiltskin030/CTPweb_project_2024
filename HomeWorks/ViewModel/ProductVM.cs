using HomeWorks.Models;

namespace HomeWorks.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public Order? Order { get; set; }
        public List<ProductDetail>? ProductDetail { get; set; }
    }
}
