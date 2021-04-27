using Microsoft.AspNetCore.Authorization;

namespace FacilityManagement.Services.API.Policy
{
    /// <summary>
    /// Policies according to roles
    /// </summary>
    public static class Policies
    {
        /// <summary>
        /// Admin role for our policy
        /// </summary>
        public const string Admin = "admin";
        /// <summary>
        /// Decadev role for our policy
        /// </summary>
        public const string Decadev = "decadev";
        /// <summary>
        /// Vendor role for our policy
        /// </summary>
        public const string Vendor = "vendor";
        /// <summary>
        /// FacilityManager role for our policy
        /// </summary>
        public const string FacilityManager = "facilityManager";

        /// <summary>
        /// Grants Admin right to User
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }

        /// <summary>
        /// Grants Decadev Access to a User
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy DecadevPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Decadev).Build();
        }

        /// <summary>
        /// Facility Policy
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy FacilityPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(FacilityManager).Build();
        }

        /// <summary>
        /// Vendor Policy
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy VendorPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Vendor).Build();
        }
    }
}