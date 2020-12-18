using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WU_DEREK_HW2.Models;

namespace WU_DEREK_HW2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckoutWalkup()
        {
            return View();
        }

        public IActionResult CheckoutWholesale()
        {
            return View();
        }

        //1. Validate the WalkupOrder object to make sure all of the data meets the requirements from the business rules
        //2. If the model state is not valid, send the user back to the CheckoutWalkup view.Be sure to include the WalkupOrder object when you return the view.
        //3. If the model state is valid, do the following
        //    a.Set the customer type
        //    b.Call the method to calculate the totals for the
        //    customer
        //    c.Display the WalktupTotals view (Views/Home/WalkupTotals.cshtml)
        public IActionResult WalkupTotals(WalkupOrder walkupOrder)
        {
            walkupOrder.CustomerType = CustomerType.Walkup;
            TryValidateModel(walkupOrder);

            if (!ModelState.IsValid)
            {
                return View("CheckoutWalkup", walkupOrder);
            }

            walkupOrder.CalcTotals();
            return View("WalkupTotals", walkupOrder);

        }

        //1. Validate the wholesaleOrder object to make sure all of the data meets the business rules
        //2. If the model state is not valid, send the user back to the CheckoutWholesale view.Be sure to include the wholesaleOrder object when you return the view.
        //3. If the model state is valid, do the following
        //    a.Set the customer type
        //    b.Call the method to calculate the totals
        //    c. Display the WholesaleTotals view (Views/Home/WholesaleTotals.cshtml)
        public IActionResult WholesaleTotals(WholesaleOrder wholesaleOrder)
        {
            wholesaleOrder.CustomerType = CustomerType.Wholesale;
            TryValidateModel(wholesaleOrder);

            if (!ModelState.IsValid)
            {
                return View("CheckoutWholesale", wholesaleOrder);
            }

            wholesaleOrder.CalcTotals(wholesaleOrder.DeliveryFee);
            return View("WholesaleTotals", wholesaleOrder);
        }
    }
}
