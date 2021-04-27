using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Services.API.Policy
{
    /// <summary>
    /// AuthorizeMultiplePolicyAttribute class
    /// </summary>
    public class AuthorizeMultiplePolicyAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// AuthorizeMultiplePolicyAttribute constructor
        /// </summary>
        /// <param name="policies"></param>
        public AuthorizeMultiplePolicyAttribute(string[] policies) : base(typeof(AuthorizeMultiplePolicyFilter)) 
        {
            Arguments = new object[] { policies };
        }
    }
}
