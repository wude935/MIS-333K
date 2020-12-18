using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WU_DEREK_HW5.DAL;
using WU_DEREK_HW5.Models;
using Microsoft.AspNetCore.Authorization;

namespace WU_DEREK_HW5.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = "Customer, Admin")]

        public IActionResult Index()
        {
            List<Order> orders = new List<Order>();
            if (User.IsInRole("Admin"))
            {
                orders = _context.Orders.Include(o => o.OrderDetails).ToList();
            }
            else
            {
                orders = _context.Orders
                    .Include(ord => ord.OrderDetails)
                    .Where(o => o.AppUser.UserName == User.Identity.Name)
                    .ToList();
            } //TODO: MAKE SURE CUSTOMERS ONLY SEE WHAT THEY ARE AUTHORIZED TO SEE
            return View(orders);
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Customer, Admin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error", new String[] { "Please specify an order to view!" });
            }

            Order order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return View("Error", new String[] { "This order was not found!" });
            }

            if (User.IsInRole("Admin") == false && order.AppUser.UserName != User.Identity.Name)
            {
                return View("Error", new string[] { "You are not authorized to edit this order!" });
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Customer")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([Bind("OrderID,OrderNumber,OrderDate,OrderNotes")] Order order)
        {
            order.OrderNumber = Utilities.GenerateOrderNumber.GetNextCourseNumber(_context);
            order.OrderDate = DateTime.Now;
            order.AppUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            //TODO: SEE IF THIS CODE WORKS IN AUTOMATICALLY MAKING USER ASSOCIATED W ORDER THEY PLACE

            if (ModelState.IsValid == false)
            {
                return View(order);
            }

            _context.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Create", "OrderDetails", new { orderID = order.OrderID });

        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Customer")]

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", new String[] { "Please specify an order to edit" });
            }


            Order order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .Include(o => o.AppUser)
                .FirstOrDefault(m => m.OrderID == id);
            //TODO: SEE IF THIS INCLUDE STATEMENT WORKS OR IF IT NEEDS TO BE FINDASYNC

            if (order == null)
            {
                return View("Error", new String[] { "This order was not found in the database!" });
            }

            if (User.IsInRole("Customer") && order.AppUser.UserName != User.Identity.Name)
            {
                return View("Error", new String[] { "You are not authorized to edit this order!" });
            }

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderNotes")] Order order)
        {
            if (id != order.OrderID)
            {
                return View("Error", new String[] { "There was a problem editing this order. Try again!" });
            }

            if (ModelState.IsValid == false)
            {
                return View(order);
            }

            //if code gets this far, update the record
            try
            {
                //find the record in the database
                Order dbOrder = _context.Orders.Find(order.OrderID);

                //update the notes
                dbOrder.OrderNotes = order.OrderNotes;

                _context.Update(dbOrder);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return View("Error", new String[] { "There was an error updating this order!", ex.Message });
            }

            //send the user to the Registrations Index page.
            return RedirectToAction(nameof(Index));
        }



        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
