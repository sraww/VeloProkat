﻿using System;
using System.Collections.Generic;

namespace VeloProkat.Models;

public partial class Order
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public DateTime DeliveryDate { get; set; }
    public string PickupPoint { get; set; } = null!;
    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}
