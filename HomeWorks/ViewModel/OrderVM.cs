using HomeWorks.Models;

namespace HomeWorks.ViewModel
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public OrderDetail OrderDetail { get; set; }

        public Member? saleMember { get; set; }

        public Member? buyMember { get; set; }
    }
}
