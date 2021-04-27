namespace FacilityManagement.Common.Utilities.Helpers
{
    public static class Utilities
    {
        /// <summary>
        /// Split email and get the domain
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetEmailDomain(string email)
        {
            return email.Split('@')[1];
        }
    }
}
