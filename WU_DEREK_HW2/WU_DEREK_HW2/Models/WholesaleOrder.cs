using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WU_DEREK_HW2.Models
{
    public class WholesaleOrder : Order
    {
        // Model properties
        [Display(Name = "Customer Code:")]
        [StringLength(4, MinimumLength = 2, ErrorMessage = "Invalid Customer Code, please enter a value be between 2 and 4 charactes.")]
        [RegularExpression("^[a-zA-z]*$", ErrorMessage = "Invalid Customer Code, please enter a LETTER code between 2 and 4 characters." )]
        public string CustomerCode { get; set; }
        
        [Display(Name = "Delivery Fee:")]
        [Range(minimum: 0, maximum: 175, ErrorMessage = "Invalid Delivery Fee, please enter a value be between 0 and 175.")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal DeliveryFee { get; set; }
        
        [Display(Name = "Is this a preferred customer?")]
        public Boolean PreferredCustomer { get; set; }

        //Calls CalcSubtotals on base class
        //Sets delivery fee property.The value will be zero if the customer is a preferred customer
        //Calculate Total(Subtotal + DeliveryFee)
        public void CalcTotals(decimal decDeliveryFee)
        {
            CalcSubtotals();
            
            if (PreferredCustomer)
            {
                DeliveryFee = 0;
            }
            else
            {
                DeliveryFee = decDeliveryFee;
            }

            Total = Subtotal + DeliveryFee;
        }
    }
}
