﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WU_DEREK_HW5.Models
{
    public class ProductSupplier
    {
        public Int32 ProductSupplierID { get; set; }

        public Product Product { get; set; }

        public Supplier Supplier { get; set; }
    }
}
