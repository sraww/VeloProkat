using System;
using System.Collections.Generic;

namespace VeloProkat.Models;

public partial class Product
{
    public virtual string? ImagePath { get { return System.IO.Path.Combine(Environment.CurrentDirectory, $"images/{Photo}"); } }
    public int Id { get; set; }
    public string? ArticleNumber { get; set; }

    public string? Name { get; set; }

    public string? Unit { get; set; }

    public decimal Cost { get; set; }

    public string? Manufacturer { get; set; }

    public string? Supplier { get; set; }

    public string? Category { get; set; }

    public decimal DiscountAmount { get; set; }

    public int QuantityInStock { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}
