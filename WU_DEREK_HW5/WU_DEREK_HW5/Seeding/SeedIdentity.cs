using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Change these using statements to match your project
using WU_DEREK_HW5.DAL;
using WU_DEREK_HW5.Models;


//TODO: Change this namespace to match your project
namespace WU_DEREK_HW5.Seeding
{
    //add identity data
    public static class SeedIdentity
    {
        public static async Task AddAdmin(IServiceProvider serviceProvider)
        {
            //Get instances of the services needed to add a user & add a user to a role
            AppDbContext _context = serviceProvider.GetRequiredService<AppDbContext>();
            UserManager<AppUser> _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //TODO: Add the needed roles
            //if the admin role doesn't exist, add it
            if (await _roleManager.RoleExistsAsync("Admin") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            //if the customer role doesn't exist, add it
            if (await _roleManager.RoleExistsAsync("Customer") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            //check to see if the admin has already been added
            AppUser newUser = _context.Users.FirstOrDefault(u => u.Email == "admin@example.com");

            //if admin hasn't been created, then add them
            if (newUser == null)
            {
                //create a new instance of the app user class
                newUser = new AppUser();

                //populate the user properties that are from the 
                //IdentityUser base class
                newUser.UserName = "admin@example.com";
                newUser.Email = "admin@example.com";
                newUser.PhoneNumber = "(512)555-1234";

                //TODO: Add additional fields that you created on the AppUser class
                //FirstName is included as an example
                newUser.FirstName = "Admin";
                newUser.LastName = "Admin";

                //create a variable for result
                IdentityResult result = new IdentityResult();
                try
                {
                    //NOTE: Attempt to add the user using the UserManager
                    //"Abc123!" is the password for this user
                    result = await _userManager.CreateAsync(newUser, "Abc123!");
                }
                catch (Exception ex)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.Append("There was an error adding the user with the email ");
                    msg.Append(newUser.Email);
                    msg.Append(". This often happens because you are missing a required field on AppUser");
                    throw new Exception(msg.ToString(), ex);
                }
                //if the user was not added succesfully, throw an exception
                //so the user knows what happened
                if (result.Succeeded == false)
                {
                    //Create a new string builder object to hold the error message(s)
                    StringBuilder msg = new StringBuilder();

                    //loop through all of the errors and add them to the message
                    foreach (var error in result.Errors)
                    {
                        msg.AppendLine(error.ToString());
                    }
                    
                    //throw a new exception to tell the user something is wrong
                    throw new Exception("This user can't be added:" + msg.ToString());
                }
                _context.SaveChanges();
                newUser = _context.Users.FirstOrDefault(u => u.UserName == "admin@example.com");
            }

            //Add the new user we just created to the Admin role
            if (await _userManager.IsInRoleAsync(newUser, "Admin") == false)
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");
            }

            //save changes
            _context.SaveChanges();
        }

    }
}