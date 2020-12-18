using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WU_DEREK_HW2.Models
{
    public enum CustomerType { Walkup, Wholesale };

    public abstract class Order
    {
        const decimal HARDBACK_PRICE = 19m;
        const decimal PAPERBACK_PRICE = 7m;

        // Model properties
        [Display(Name = "Customer Type:")]
        public CustomerType CustomerType { get; set; }
        
        [Display(Name = "Number of Hardbacks:")]
        [Range(minimum: 0, maximum: 1000)]
        public int NumberOfHardbacks { get; set; }
        
        [Display(Name = "Number of Paperbacks:")]
        [Range(minimum: 0, maximum: 1000)]
        public int NumberOfPaperbacks { get; set; }
        
        [Display(Name = "Hardback Subtotal:")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal HardbackSubtotal { get; set; }
        
        [Display(Name = "Paperback Subtotal:")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal PaperbackSubtotal { get; set; }
        
        [Display(Name = "Subtotal:")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Subtotal { get; set; }
        
        [Display(Name = "Total:")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Total { get; set; }
        
        [Display(Name = "Total Items:")]
        public decimal TotalItems { get; set; }

        // Calculates TotalItems, HardbackSubtotal, PaperbackSubtotal, and Subtotal
        public void CalcSubtotals()
        {
            HardbackSubtotal = NumberOfHardbacks * HARDBACK_PRICE;
            PaperbackSubtotal = NumberOfPaperbacks * PAPERBACK_PRICE;
            Subtotal = HardbackSubtotal + PaperbackSubtotal;
            TotalItems = NumberOfHardbacks + NumberOfPaperbacks;
        }

    }
}
