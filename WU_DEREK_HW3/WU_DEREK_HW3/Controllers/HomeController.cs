using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WU_DEREK_HW3.DAL;
using WU_DEREK_HW3.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace WU_DEREK_HW3.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;

        public HomeController(AppDbContext DbContext)
        {
            _context = DbContext;
        }

        public IActionResult Index(String SearchString)
        {
            var query = from jp in _context.JobPostings
                        select jp;
            if (SearchString != null)
            {
                query = query.Where(jp => jp.Title.Contains(SearchString) || jp.Description.Contains(SearchString));
            }
            List<JobPosting> SelectedJobPostings = query.Include(jp => jp.Category).ToList();
            //Populate the view bag with a count of all job postings
            ViewBag.AllJobs = _context.JobPostings.Count();
            //Populate the view bag with a count of selected job postings
            ViewBag.SelectedJobs = SelectedJobPostings.Count();
            return View(SelectedJobPostings.OrderByDescending(jp => jp.PostedDate));
        }

        public IActionResult Details(int? id)
        {
            if (id == null) //JobPostingID not specified
            {
                return View("Error", new String[] { "JobPostingID not specified - which job posting do you want to view?" });
            }
               
            JobPosting jobPosting = _context.JobPostings.Include(j => j.Category).FirstOrDefault(j => j.JobPostingID == id);
                
            if (jobPosting == null) //Job posting does not exist in database
            {
                return View("Error", new String[] { "Job posting not found in database" });
            }
            
            //if code gets this far, all is well
            return View(jobPosting);
            }

        public IActionResult DetailedSearch()
        {
            ViewBag.AllCategories = GetAllCategories();
            return View();
        }

        public IActionResult DisplaySearchResults(SearchViewModel svm)
        {
            var query = from jp in _context.JobPostings
                        select jp;

            if (svm.Title != null && svm.Title != "")
            {
                query = query.Where(jp => jp.Title.Contains(svm.Title));
            }

            if (svm.Description != null && svm.Description != "")
            {
                query = query.Where(jp => jp.Description.Contains(svm.Description));
            }

            if (svm.Category != 0)
            {
                query = query.Where(jp => jp.Category.CategoryID == svm.Category);
            }

            if (svm.SalaryAmount >= 0)
            {
                switch(svm.SalaryComparison)
                {
                    case SalaryComparison.greaterThan:
                        query = query.Where(jp => jp.MinimumSalary > svm.SalaryAmount);
                        break;
                    case SalaryComparison.lessThan:
                        query = query.Where(jp => jp.MinimumSalary <= svm.SalaryAmount);
                        break;
                }
            }

            if (svm.PostedDate != null)
            {
                query = query.Where(jp => jp.PostedDate >= svm.PostedDate);
            }

            List<JobPosting> SelectedJobPostings = query.Include(jp => jp.Category).ToList();
            //Populate the view bag with a count of all job postings
            ViewBag.AllJobs = _context.JobPostings.Count();
            //Populate the view bag with a count of selected job postings
            ViewBag.SelectedJobs = SelectedJobPostings.Count();
            return View("Index", SelectedJobPostings.OrderByDescending(jp => jp.PostedDate));
        }

        private SelectList GetAllCategories()
        {
            //Get the list of categories from the database
            List<Category> categoryList = _context.Categories.ToList();

            //add a dummy entry so the user can select all categories
            Category SelectNone = new Category() { CategoryID = 0, CategoryName = "All Categories" };
            categoryList.Add(SelectNone);

            //convert the list to a SelectList by calling SelectList constructor
            //categoryID and categoryName are the names of the properties on the Month class
            //categoryID is the primary key
            SelectList categorySelectList = new SelectList(categoryList.OrderBy(m => m.CategoryID), "CategoryID", "CategoryName");

            //return the SelectList
            return categorySelectList;
        }

        }
}
