using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WU_DEREK_HW2.Models
{
    public class WalkupOrder : Order
    {
        const decimal SALES_TAX_RATE = 8.75m;

        // Model properties
        [Display(Name = "Customer Name:")]
        public string CustomerName { get; set; }
        
        [Display(Name = "Sales Tax:")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal SalesTax { get; set; }

        //Calls CalcSubtotals on base class
        //Calculates SalesTax and Total
        public void CalcTotals()
        {
            CalcSubtotals();

            SalesTax = Subtotal * .0875m;
            Total = Subtotal + SalesTax;
        }
    }
}
