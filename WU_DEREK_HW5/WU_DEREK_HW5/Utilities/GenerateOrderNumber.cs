using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WU_DEREK_HW5.DAL;

namespace WU_DEREK_HW5.Utilities
{
    public static class GenerateOrderNumber
    {
        public static Int32 GetNextCourseNumber(AppDbContext _context)
        {
            //Set a number where the course numbers should start
            const Int32 START_NUMBER = 7000;

            Int32 intMaxOrderNumber; //the current maximum course number
            Int32 intNextOrderNumber; //the course number for the next class

            if (_context.Orders.Count() == 0) //there are no courses in the database yet
            {
                intMaxOrderNumber = START_NUMBER; //course numbers start at 3001
            }
            else
            {
                intMaxOrderNumber = _context.Orders.Max(c => c.OrderNumber); //this is the highest number in the database right now
            }

            //You added courses before you realized that you needed this code
            //and now you have some course numbers less than 3000
            if (intMaxOrderNumber < START_NUMBER)
            {
                intMaxOrderNumber = START_NUMBER;
            }

            //add one to the current max to find the next one
            intNextOrderNumber = intMaxOrderNumber + 1;

            //return the value
            return intNextOrderNumber;
        }

    }
}
