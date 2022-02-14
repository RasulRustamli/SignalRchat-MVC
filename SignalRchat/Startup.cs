
using Database;
using Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRchat.Models;

namespace SignalRchat
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddSignalR();
        }

        [System.Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
