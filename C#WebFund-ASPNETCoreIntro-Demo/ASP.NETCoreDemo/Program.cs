namespace ASP.NETCoreDemo
{
    using ASP.NETCoreDemo.Services;
    using ASP.NETCoreDemo.Services.Interfaces;
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
// Singleton -> One instance when first asked,then every time we receive this instance
// Scoped -> New instance every time new scope is open. New instance every time when new request comes.
// Transient -> New instance every time is asked
builder.Services.AddScoped<ICarService, CarService>();
            var connectionString = builder.Configuration.GetConnectionString(name: "DefaultConnection");
                
                                        // Until here -> Configure DependencyInjection and Services (order does not matter)
            var app = builder.Build(); //------------------------
                                      //  Configure middleware and filter (oder does matter)

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}