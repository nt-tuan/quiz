using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using dmc_auth.Data;
using dmc_auth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dmc_auth
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // services.AddDbContext<ApplicationDbContext>(options =>
      //     options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL")));
      services.AddDbContext<ApplicationDbContext>(options =>
      options.UseNpgsql(Configuration.GetConnectionString("DatabaseURL")));


      services.AddDefaultIdentity<ApplicationUser>()
          .AddEntityFrameworkStores<ApplicationDbContext>();

      services.AddControllersWithViews();
      services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      using (var scope = app.ApplicationServices.CreateScope())
      {
        scope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
      }

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });
    }
  }
}
