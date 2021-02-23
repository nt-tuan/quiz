using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Quiz.DBContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using ThanhTuan.Quiz.Repositories;
using ThanhTuan.Quiz.Services;

namespace ThanhTuan.Quiz
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }
    public IConfiguration Configuration { get; }
    public static readonly ILoggerFactory factory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(name: "all", builder =>
              {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
              });
      });
      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseNpgsql(Configuration.GetConnectionString("DatabaseURL"));
      });
      services.AddHttpContextAccessor();
      services.AddControllers();
      services.AddScoped<ExamRepository>();
      services.AddScoped<Authorizer>();
      services.AddSwaggerGen(c =>
        {
          var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
          var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
          c.IncludeXmlComments(xmlPath);
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      using (var scope = app.ApplicationServices.CreateScope())
      {
        var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
        db.Database.Migrate();
      }

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });

      app.UseRouting();
      app.UseCors("all");
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDefaultControllerRoute();
      });
    }
  }
}
