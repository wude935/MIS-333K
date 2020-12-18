using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WU_DEREK_HW5.Models
{
    public class OrderDetail
    {
        public Int32 OrderDetailID { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 1000, ErrorMessage = "Number of books must be between 1 and 1000")]
        public Int32 Quantity { get; set; }

        [Display(Name = "Product price")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public Decimal ProductPrice { get; set; }

        [Display(Name = "Extended price")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public Decimal ExtendedPrice { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
