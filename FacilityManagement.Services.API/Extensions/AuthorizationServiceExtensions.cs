using FacilityManagement.Services.API.Policy;
using Microsoft.Extensions.DependencyInjection;

namespace FacilityManagement.Services.API.Extensions
{
    /// <summary>
    /// AuthorizationServiceExtensions class
    /// </summary>
    public static class AuthorizationServiceExtensions
    {
        /// <summary>
        /// AddAuthorizationConfiguring method
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationConfiguring(this IServiceCollection services)
        {
            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.Decadev, Policies.DecadevPolicy());
                config.AddPolicy(Policies.Vendor, Policies.VendorPolicy());
                config.AddPolicy(Policies.FacilityManager, Policies.FacilityPolicy());
            });
        }
    }
}
