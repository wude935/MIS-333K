using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WU_DEREK_HW5.Models
{
    public class Product
    {
        [Display(Name = "ProductID")]
        public Int32 ProductID { get; set; }

        [Display(Name = "Product name")]
        [Required(ErrorMessage = "Product name is required.")]
        public String Name { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [Required(ErrorMessage = "Price is required.")]
        public Decimal Price { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public List<ProductSupplier> ProductSuppliers { get; set; }

    }
}
