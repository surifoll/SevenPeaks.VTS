using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle;
using SevenPeaks.VTS.Application.Vehicle.Commands.UpdateVehicle;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicle;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles;
using SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition;
using SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Interfaces;
using SevenPeaks.VTS.Infrastructure.Services;
using SevenPeaks.VTS.Persistence.ExtensionMethods;
using SevenPeaks.VTS.Services.Interfaces;
using SevenPeaks.VTS.Web.AuthService;
using SevenPeaks.VTS.Web.BackgroundServices;
using SevenPeaks.VTS.Web.Data;
using SevenPeaks.VTS.Web.Validations;

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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSqlPersistence(Configuration);
            services.AddTransient<IAuthenticatedUser, AuthenticatedUser>();
            services.AddTransient<IGetVehiclesQuery, GetVehiclesQuery>();
            services.AddTransient<IAddVehicleCommand, AddVehicleCommand>();
            services.AddTransient<IAddVehiclePositionCommand, AddVehiclePositionCommand>();
            services.AddTransient<IGetVehicleQuery, GetVehicleQuery>();
            services.AddTransient<IGetVehiclePositionsQuery, GetVehiclePositionsQuery>();
            services.AddTransient<IUpdateVehicleCommand, UpdateVehicleCommand>();
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
            services.AddSingleton(o => settings);
            services.AddTransient<IStandardRabbitMq>(o => new StandardRabbitMq(settings));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews().AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<VehiclePositionValidator>());
            ;
            services.AddHostedService<Worker>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "SevenPeaks.VTS",
                    Version = "v1",
                    Description = "Service for SevenPeaks.VTS"
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
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "SevenPeaks.VTS Services"));
        }
    }
}