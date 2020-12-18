using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace fa20IdentityTemplate.Models
{
    public class AppUser : IdentityUser
    {
        //TODO: Add any additional fields you need for your user here
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required.")]
        public String FirstName { get; set; }
    }
}
