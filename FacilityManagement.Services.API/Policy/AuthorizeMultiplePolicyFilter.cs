using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace FacilityManagement.Services.API.Policy
{
    /// <summary>
    /// AuthorizeMultiplePolicyFilter class
    /// </summary>
    public class AuthorizeMultiplePolicyFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorization;
        /// <summary>
        /// policies
        /// </summary>
        public string[] _policies { get; private set; }
        /// <summary>
        /// AuthorizeMultiplePolicyFilter constructor
        /// </summary>
        public AuthorizeMultiplePolicyFilter(string[] policies, IAuthorizationService authorization)
        {
            _policies = policies;
            _authorization = authorization;
        }

        /// <summary>
        /// OnAuthorizationAsync method
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            foreach( var policy in _policies)
            {
                var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                if(authorized.Succeeded)
                {
                    return;
                }
            }

            context.Result = new ForbidResult();
            return;
        }
    }
}