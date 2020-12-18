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
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(c => c.ProductSuppliers)
                .ThenInclude(c => c.Supplier)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            //TODO: ADD INCLUDE STATEMENTS

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.AllSuppliers = GetAllSuppliers();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ProductID,Name,Description,Price")] Product product, int[] SelectedSuppliers)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.AllSuppliers = GetAllSuppliers();
                return View(product);
            }

            _context.Add(product);
            await _context.SaveChangesAsync();
            foreach (int supplierID in SelectedSuppliers)
            {
                //find the department associated with that id
                Supplier dbSupplier = _context.Suppliers.Find(supplierID);

                //create a new course department object to make the association
                ProductSupplier cd = new ProductSupplier();

                //set the properties of the course department object
                cd.Product = product; //this is the course object that was passed into
                                      //the method as part of the HTTP reqest

                cd.Supplier = dbSupplier; //this is the department we found above

                //add the course department to the database and save changes
                _context.ProductSuppliers.Add(cd);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.AllSuppliers = GetAllSuppliers(product.ProductID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Description,Price")] Product product, int[] SelectedSuppliers)
        {
            {
                if (id != product.ProductID)
                {
                    return NotFound();
                }
                if (ModelState.IsValid == false) //there is something wrong
                {
                    ViewBag.AllSuppliers = GetAllSuppliers(product.ProductID);
                    return View(product);
                }

                //if code gets this far, attempt to edit the product
                try
                {
                    //Find the course to edit in the database and include relevant 
                    //navigational properties
                    Product dbProduct = _context.Products
                        .Include(cd => cd.ProductSuppliers).ThenInclude(cd => cd.Supplier)
                        .FirstOrDefault(c => c.ProductID == product.ProductID);

                    //create a list of departments that need to be removed
                    List<ProductSupplier> SuppliersToRemove = new List<ProductSupplier>();

                    //find the departments that should no longer be selected because the
                    //user removed them
                    //remember, SelectedDepartments = the list from the HTTP request (listbox)
                    foreach (ProductSupplier cd in dbProduct.ProductSuppliers)
                    {
                        //see if the new list contains the department id from the old list
                        if (SelectedSuppliers.Contains(cd.Supplier.SupplierID) == false)//this department is not on the new list
                        {
                            SuppliersToRemove.Add(cd);
                        }
                    }

                    //remove the departments you found in the list above
                    //this has to be 2 separate steps because you can't iterate (loop)
                    //over a list that you are removing things from
                    foreach (ProductSupplier cd in SuppliersToRemove)
                    {
                        //remove this course department from the database
                        _context.ProductSuppliers.Remove(cd);
                        _context.SaveChanges();
                    }

                    //add the departments that aren't already there
                    foreach (int supplierID in SelectedSuppliers)
                    {
                        if (dbProduct.ProductSuppliers.Any(d => d.Supplier.SupplierID == supplierID) == false)//this department is NOT already associated with this course
                        {
                            //create a new instance of the bridge table class
                            ProductSupplier cd = new ProductSupplier();
                            cd.Supplier = _context.Suppliers.Find(supplierID);
                            cd.Product = dbProduct;

                            //add the new instance to the database
                            _context.ProductSuppliers.Add(cd);
                            _context.SaveChanges();
                        }
                    }

                    //update the course's scalar properties
                    dbProduct.Name = product.Name;
                    dbProduct.Price = product.Price;
                    dbProduct.Description = product.Description;

                    //save the changes
                    _context.Products.Update(dbProduct);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    return View("Error", new string[] { "There was an error editing this product.", ex.Message });
                }

                //if code gets this far, everything is okay
                //send the user back to the page with all the courses
                return RedirectToAction(nameof(Index));
            }
        }

        private MultiSelectList GetAllSuppliers()
        {
            List<Supplier> allSuppliers = _context.Suppliers.ToList();

            MultiSelectList mslAllSuppliers = new MultiSelectList(allSuppliers.OrderBy(d => d.SupplierName), "SupplierID", "SupplierName");

            return mslAllSuppliers;
        }

        private MultiSelectList GetAllSuppliers(int productID)
        {

            List<Supplier> allSuppliers = _context.Suppliers.ToList();
            List<ProductSupplier> productSuppliers = _context.ProductSuppliers
                                                 .Where(cd => cd.Product.ProductID == productID)
                                                 .ToList();
            List<Int32> selectedSupplierIDs = new List<Int32>();

            foreach (ProductSupplier cd in productSuppliers)
            {
                selectedSupplierIDs.Add(cd.Supplier.SupplierID);
            }
            MultiSelectList mslAllSuppliers = new MultiSelectList(allSuppliers.OrderBy(d => d.SupplierName), "SupplierID", "SupplierName", selectedSupplierIDs);

            return mslAllSuppliers;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }


    }
}
