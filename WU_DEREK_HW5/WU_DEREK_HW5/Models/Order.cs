using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WU_DEREK_HW5.Models
{
    public class Order
    {
        const Decimal TAX_RATE = 0.0825m;

        [Display(Name = "OrderID")]
        public Int32 OrderID { get; set; }

        [Display(Name = "Order number")]
        public Int32 OrderNumber { get; set; }

        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Order notes")]
        public String OrderNotes { get; set; }

        [Display(Name = "Order Subtotal")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Subtotal
        {
            get { return OrderDetails.Sum(od => od.ExtendedPrice); }

        }

        [Display(Name = "Sales Tax")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal SalesTax
        {
            get { return TAX_RATE * Subtotal; }
        }

        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderTotal
        {
            get { return Subtotal + SalesTax; }
        }
        public AppUser AppUser { get; set; }

        public Product Product { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            if (OrderDetails == null)
            {
                OrderDetails = new List<OrderDetail>();
            }
        }
    }
}
