using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WU_DEREK_HW5.Models
{
    public class AppUser : IdentityUser
    {
        //TODO: Add any additional fields you need for your user here
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required.")]
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        public String LastName { get; set; }

        public List<Order> Orders { get; set; }

        public String FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public AppUser()
        {
            if (Orders == null)
            {
                Orders = new List<Order>();
            }
        }
    }
}
