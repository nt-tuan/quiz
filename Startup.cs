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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using dmc_auth.Hydra;

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
            });

            services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager();

            services.AddAuthentication(options =>
      {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
            .AddJwtBearer(config =>
            {
                config.Authority = Environment.GetEnvironmentVariable(Constant.ENV_PUBLIC_AUTH_URL);
                config.Audience = Environment.GetEnvironmentVariable(Constant.ENV_CLIENT_ID);
                config.SaveToken = true;
                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // DEBUGGING ONLY
          };
                config.BackchannelHttpHandler = httpHandler;
                config.RequireHttpsMetadata = false;
                config.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
              {
              // var nameIdentifierClaim = ctx.Principal.FindFirst(ClaimTypes.NameIdentifier);
              // var extraClaim = ctx.Principal.Claims.FirstOrDefault(u => u.Type == "ext");
              // var rolesClaim = new List<Claim>();
              // if (extraClaim != null)
              // {
              //   try
              //   {
              //     var payload = JsonConvert.DeserializeObject<ConsentAcceptIDTokenBody>(extraClaim.Value);
              //     rolesClaim.AddRange(payload.roles.Select(role => new Claim(ClaimTypes.Role, role)));
              //   }
              //   catch { }
              // }
              // var idClaims = new List<Claim>();
              // if (nameIdentifierClaim != null)
              //   idClaims.Add(new Claim(ClaimTypes.Name, nameIdentifierClaim.Value));
              // idClaims.AddRange(rolesClaim);
              // ((ClaimsIdentity)ctx.Principal.Identity).AddClaims(idClaims);
              return Task.CompletedTask;
                },
                    OnAuthenticationFailed = ctx =>
              {
                    var logger = factory.CreateLogger<Startup>();
                    logger.LogInformation(ctx.Exception?.Message);
                    return Task.CompletedTask;
                }
                };
            });
            services.AddScoped<IHydra, Hydra.Hydra>();
            services.AddSingleton<AccessDecision.AccessDecision>();
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
                var seeder = new DataSeeder(userManager, roleManager);
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
