using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDwhProject.Core.Entities.Oltp;
public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
