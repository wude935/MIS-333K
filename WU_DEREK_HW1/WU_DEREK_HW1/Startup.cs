using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

//TODO: Make this namespace match your project - be sure to remove the []
namespace WU_DEREK_HW1
{
    public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {

            //This adds the MVC engine and Razor code
            services.AddControllersWithViews();
            //Configures hot reloading
            services.AddRazorPages().AddRazorRuntimeCompilation();

            //TODO: (For HW4 and beyond) Add a connection string here once you have created it on Azure
            //String connectionString = "";

            //TODO: Uncomment this line once you have your connection string
            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            //TODO: Uncomment these lines once you have added Identity to your project
            ////NOTE: This is where you would change your password requirements
            /*services.AddIdentity<AppUser, IdentityRole>(opts => {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();*/
        }

        public void Configure(IApplicationBuilder app)
    {
        //Configures hot reloading
        app.UseBrowserLink();
        //These lines allow you to see more detailed error messages
        app.UseDeveloperExceptionPage();
        app.UseStatusCodePages();

        //TODO: Once you have added Identity into your project, you will need to uncomment this line
        //app.UseAuthentication();

        //This line allows you to use static pages like style sheets and images
        app.UseStaticFiles();

        //This marks the position in the middleware pipeline where a routing decision
        //is made for a URL.
        app.UseRouting();

        //This method maps the controllers and their actions to a patter for
        //requests that's known as the default route. This route identifies
        //the Home controller as the default controller and the Index() action
        //method as the default action. The default route also identifies a 
        //third segment of the URL that's a parameter named id.
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}


}