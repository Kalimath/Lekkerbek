using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Controllers;
using Lekkerbek.Web.Models.Identity;
using Lekkerbek.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web
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
            services.AddDbContext<IdentityContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("GipTeam11"));
            });

            services.AddDefaultIdentity<Gebruiker>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<Role>()
                .AddEntityFrameworkStores<IdentityContext>();

            services.AddControllersWithViews();
            services.AddControllersWithViews().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("GipTeam11"));
            }, ServiceLifetime.Transient);
            services.AddScoped<IdentityContext>();
            services.AddTransient<IBestellingService, BestellingService>();
            services.AddTransient<IGebruikerService, GebruikerService>();
            services.AddTransient<IBeoordelingService, BeoordelingService>();
            services.AddTransient<IGerechtService, GerechtService>();
            services.AddTransient<ICategorieService, CategorieService>();

            services.AddTransient<ITijdslotService, TijdslotService>();
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Password.RequiredLength = 1;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            
            CreateRoles(serviceProvider).Wait();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<Gebruiker>>();

            IdentityResult roleResult;
            //here in this line we are adding the Roles
            foreach (string role in Enum.GetValues<RollenEnum>().Cast<RollenEnum>().Select(v => v.ToString()).ToList())
            {
                var roleCheck = await RoleManager.RoleExistsAsync(role);
                if (!roleCheck)
                {
                    //here in this line we are creating admin role and seed it to the database
                    roleResult = await RoleManager.CreateAsync(new Role(role));
                }
            }

            //here we are assigning the Admin role to the User that we have registered above 
            //Now, we are assinging admin role to this user("Ali@gmail.com"). When will we run this project then it will
            //be assigned to that user.
            /*Gebruiker user = await UserManager.FindByEmailAsync("mathieu_broe@yahoo.com");
            if (user != null)
            {
                foreach (string role in Enum.GetValues<RollenEnum>().Cast<RollenEnum>().Select(v => v.ToString()).ToList())
                {
                    await UserManager.AddToRoleAsync(user, role);
                }
            }*/
        }
    }
}
