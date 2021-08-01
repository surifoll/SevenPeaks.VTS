using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SevenPeaks.VTS.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SevenPeaks.VTS.Persistence.ExtensionMethods;
using SevenPeaks.VTS.Infrastructure.Interfaces;
using SevenPeaks.VTS.Web.AuthService;
using Microsoft.AspNetCore.Http;
using SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles;
using SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition;
using SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Services;
using SevenPeaks.VTS.Services.Interfaces;

namespace SevenPeaks.VTS.Web
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddPgsqlPersistence(Configuration);
            services.AddTransient<IAuthenticatedUser, AuthenticatedUser>();
            services.AddTransient<IGetVehiclesQuery, GetVehiclesQuery>();
            services.AddTransient<IAddVehicleCommand, AddVehicleCommand>();
            services.AddTransient<IAddVehiclePositionCommand, AddVehiclePositionCommand>();
            services.AddTransient<IGetVehiclePositionsQuery, GetVehiclePositionsQuery>();
           // services.AddTransient<IStandardRabbitMq, StandardRabbitMq>();
            
            var settings = new RabbitMqSettings();
            Configuration.GetSection("RabbitMqSettings").Bind(settings);
             
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
            services.AddTransient<IStandardRabbitMq>(o => new StandardRabbitMq(settings));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddSwaggerGen(options =>  
            {  
                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo  
                {  
                    Title = "SevenPeaks.VTS",  
                    Version = "v1",  
                    Description = "Service for SevenPeaks.VTS",  
                });  
            });  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "SevenPeaks.VTS Services"));
        }
    }
}
