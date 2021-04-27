using System.Text.RegularExpressions;

namespace FacilityManagement.Common.Utilities.Helpers
{
    public class MailValidations
    {
        public static bool IsMailValid(string emailaddress)
        {
            string pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            if (Regex.IsMatch(emailaddress, pattern))
            {
                return true;
            }
            return false;
        }


    }
}