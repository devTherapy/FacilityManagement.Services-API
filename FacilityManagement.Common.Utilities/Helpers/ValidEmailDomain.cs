using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FacilityManagement.Common.Utilities.Helpers
{
    public class ValidEmailDomain : ValidationAttribute
    {
        private readonly string[] _allowedDomains = {"decagon.dev"};

        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || !value.ToString().Contains('@')) return false;

            // get the inputted email domain
            var inputEmailDomain = Utilities.GetEmailDomain(value.ToString());

            // check if email domain is included in the allowed domains
            return _allowedDomains.Any(allowedDomain => 
                string.Equals(allowedDomain, inputEmailDomain, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
