﻿using System;
using System.Collections.Generic;

namespace Kurs1135.Models
{
    public partial class OrderProduct
    {
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string? Count { get; set; }
        public int? CategoryId { get; set; }
        public int Id { get; set; }

        public virtual ProductCategory? Category { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
