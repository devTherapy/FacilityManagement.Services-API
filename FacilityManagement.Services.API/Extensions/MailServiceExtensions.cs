using FacilityManagement.Services.Models.AppSettingModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FacilityManagement.Services.API.Extensions
{
    /// <summary>
    /// Mail service extensions configuration
    /// </summary>
    public static class MailServiceExtensions
    {
        /// <summary>
        /// Configures the application to use sendGrid mailing service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConfigureMailing(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(section);
        }
    }
}
