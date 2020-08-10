using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;
using System.ComponentModel;
using System.IO;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using dmc_auth.Data;
using dmc_auth.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;

namespace dmc_auth
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
                options.UseLoggerFactory(factory);
            });

            services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
            .AddJwtBearer(config =>
            {
                config.Authority = Environment.GetEnvironmentVariable(Constant.ENV_PUBLIC_AUTH_URL);
                config.Audience = "idp";
                var httpHandler = new HttpClientHandler();
                httpHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // DEBUGGING ONLY
                config.BackchannelHttpHandler = httpHandler;
                config.RequireHttpsMetadata = false;
                var jwtHandler = new JwtSecurityTokenHandler();
                jwtHandler.InboundClaimTypeMap.Clear();
                config.SecurityTokenValidators.Add(jwtHandler);
            });
            // .AddOpenIdConnect(config =>
            // {
            //     var httpHandler = new HttpClientHandler();
            //     httpHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // DEBUGGING ONLY
            //     config.BackchannelHttpHandler = httpHandler;
            //     config.Authority = Environment.GetEnvironmentVariable(Constant.ENV_PUBLIC_AUTH_URL);
            //     config.ClientId = Environment.GetEnvironmentVariable(Constant.ENV_CLIENT_ID);
            //     config.ClientSecret = Environment.GetEnvironmentVariable(Constant.ENV_CLIENT_SECRET);
            // });
            services.AddControllers();
            services.AddSwaggerGen();
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
                var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
                dbContext.Database.Migrate();
                var seeder = new DataSeeder(dbContext, userManager, roleManager);
                seeder.Seed();

            }
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();
            app.UseCors("all");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
