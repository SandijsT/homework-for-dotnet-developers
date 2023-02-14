using System;
using System.Collections.Generic;

namespace OrderAPI.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public int OrderAmount { get; set; }

    public DateTime ExpectedDeliveryDate { get; set; }

    public decimal Price { get; set; }
}
