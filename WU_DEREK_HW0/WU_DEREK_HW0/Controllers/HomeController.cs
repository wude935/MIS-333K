using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WU_DEREK_HW0.Models;

namespace WU_DEREK_HW0.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RsvpForm(GuestResponse guestResponse)
        {
            if (ModelState.IsValid)
            {
                Repository.AddResponse(guestResponse);
                return View("Thanks", guestResponse);
            }
            else
            {
                return View();
            }
        }
        public IActionResult ListResponses()
        {
            IEnumerable<GuestResponse> attendeeList =
                                            Repository.Responses.Where(r => r.WillAttend == true);
            return View(attendeeList);
        }
    }
}
