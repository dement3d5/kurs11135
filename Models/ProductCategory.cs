﻿using System;
using System.Collections.Generic;

namespace Kurs1135.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
