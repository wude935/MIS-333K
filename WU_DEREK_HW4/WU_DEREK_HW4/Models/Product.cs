using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WU_DEREK_HW4.Models
{
    public class Product
    {
        public Int32 ProductID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public String Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "Price is required.")]
        public Decimal Price { get; set; }
    }
}
