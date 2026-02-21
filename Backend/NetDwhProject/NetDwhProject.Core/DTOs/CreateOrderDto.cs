using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDwhProject.Core.DTOs;
public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<CreateOrderDetailDto> OrderDetails { get; set; } = new();
}
