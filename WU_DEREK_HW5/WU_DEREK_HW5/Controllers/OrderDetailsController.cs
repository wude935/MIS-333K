using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WU_DEREK_HW5.DAL;
using WU_DEREK_HW5.Models;

namespace WU_DEREK_HW5.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public OrderDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public IActionResult Index(int? orderID)
        {
            if (orderID == null)
            {
                return View("Error", new String[] { "Please specify a registration to view!" });
            }
            List<OrderDetail> ods = _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.Order.OrderID == orderID)
                .Include(od => od.Order)
                .ToList();

            return View(ods);
        }

        // GET: OrderDetails/Create
        public IActionResult Create(int orderID)
        {
            OrderDetail od = new OrderDetail();

            Order dbOrder = _context.Orders.Find(orderID);

            od.Order = dbOrder;

            ViewBag.AllProducts = GetAllProducts();
            return View(od);
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetail orderDetail, int SelectedProduct)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.AllProducts = GetAllProducts();
                return View(orderDetail);
            }

            Product dbProduct = _context.Products.Find(SelectedProduct);

            orderDetail.Product = dbProduct;

            Order dbOrder = _context.Orders.Find(orderDetail.Order.OrderID);
            orderDetail.Order = dbOrder;

            orderDetail.ProductPrice = dbProduct.Price;

            orderDetail.ExtendedPrice = orderDetail.Quantity * orderDetail.ProductPrice;

            //add the registration detail to the database
            _context.Add(orderDetail);
            await _context.SaveChangesAsync();

            //send the user to the details page for this registration
            return RedirectToAction("Details", "Orders", new { id = orderDetail.Order.OrderID });
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            OrderDetail orderDetail = await _context.OrderDetails
                                                .Include(od => od.Product)
                                                .Include(od => od.Order)
                                                .FirstOrDefaultAsync(od => od.OrderDetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailID, Quantity")] OrderDetail orderDetail)
        //TODO: SEE IF I NEED TO ADD SELECTEDPRODUCT IN 
        {
            if (id != orderDetail.OrderDetailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {
                orderDetail = await _context.OrderDetails
                                              .Include(od => od.Product)
                                              .Include(od => od.Order)
                                              .FirstOrDefaultAsync(od => od.OrderDetailID == id);
                return View(orderDetail);
            }

            //create a new registration detail
            OrderDetail dbOD;
            //if code gets this far, update the record
            try
            {
                //find the existing registration detail in the database
                //include both registration and course
                dbOD = _context.OrderDetails
                      .Include(rd => rd.Product)
                      .Include(rd => rd.Order)
                      .FirstOrDefault(od => od.OrderDetailID == orderDetail.OrderDetailID);

                //update the scalar properties
                dbOD.Quantity = orderDetail.Quantity;
                dbOD.ProductPrice = dbOD.Product.Price;
                dbOD.ExtendedPrice = dbOD.Quantity * dbOD.ProductPrice;

                //save changes
                _context.Update(dbOD);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return View("Error", new String[] { "There was a problem editing this record", ex.Message });
            }

            //if code gets this far, go back to the registration details index page
            return RedirectToAction("Details", "Orders", new { id = dbOD.Order.OrderID });
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrderDetail orderDetail = await _context.OrderDetails
                                                         .Include(o => o.Order)
                                                        .FirstOrDefaultAsync(m => m.OrderDetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = await _context.OrderDetails
                                                              .Include(od => od.Order)
                                                              .FirstOrDefaultAsync(od => od.OrderDetailID == id);
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Orders", new { id = orderDetail.Order.OrderID });
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailID == id);
        }

        private SelectList GetAllProducts()
        {
            //create a list for all the courses
            List<Product> allProducts = _context.Products.ToList();

            //the user MUST select a course, so you don't need a dummy option for no course

            //use the constructor on select list to create a new select list with the options
            SelectList slAllProducts = new SelectList(allProducts, nameof(Product.ProductID), nameof(Product.Name));

            return slAllProducts;
        }
    }
}
