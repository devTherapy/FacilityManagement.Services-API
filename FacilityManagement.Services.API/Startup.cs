using FacilityManagement.Common.Utilities.Helpers;
using FacilityManagement.Services.API.Extensions;
using FacilityManagement.Services.API.Middleware;
using FacilityManagement.Services.Data;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FacilityManagement.Services.API
{
    /// <summary>
    /// Start up class
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Start up constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="webHostEnvironment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = HerokuDatabaseSetup.DatabaseConnectionString(_webHostEnvironment, _configuration);

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

            services.AddIdentityConfiguring();
            services.AddAuthenticationConfiguring(_configuration);
            services.AddAuthorizationConfiguring();
            services.AddDependencyInjection();
            services.AddConfigureSwagger();
            services.AddConfigureMailing(_configuration);
            services.AddCors();
            services.AddConfigureSettings(_configuration);
            
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="ctx"></param>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            DataContext ctx,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager
            )
        {

            app.UseMiddleware<ExceptionMiddleware>();
                //.UseMiddleware<ApiKeyMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FacilityManagementApp API v1"));

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            PreSeederSetup.ConfigurePreSeeder(ctx, roleManager, userManager).Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}