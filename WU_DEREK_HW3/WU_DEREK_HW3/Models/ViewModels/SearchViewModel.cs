using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WU_DEREK_HW3.Models
{ 
    public enum SalaryComparison { greaterThan, lessThan }
    public class SearchViewModel
    {
        public String? Title { get; set; }
        public String? Description { get; set; }
        public Int32? Category { get; set; }
        public Int32? SalaryAmount { get; set; }
        public SalaryComparison? SalaryComparison { get; set; }
        public DateTime? PostedDate { get; set; }
    }
}
