using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WU_DEREK_HW5.Models
{
    public class Supplier
    {
        [Display(Name = "SupplierID")]
        public Int32 SupplierID { get; set; }

        [Display(Name = "Supplier name")]
        [Required(ErrorMessage = "Supplier name is required.")]
        public String SupplierName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        public String Email { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone number is required)")]
        public String PhoneNumber { get; set; }

        public List<ProductSupplier> ProductSuppliers { get; set; }

    }
}
