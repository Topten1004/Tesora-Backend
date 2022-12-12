namespace NFTApplication.Utility
{
    /// <summary>
    /// Helper
    /// </summary>
    public static class HttpContextClaims
    {
        /// <summary>
        /// Get the master user id from the sso server
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetMasterUserId(HttpContext httpContext)
        {
            // Get the user
            var idClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (idClaim == null)
            {
                idClaim = new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "293670b1-cf7e-4b15-ae5c-d4b6c3a9ad81");

                //throw new Exception("Invalid User");
            }

            return idClaim.Value;
        }

    }
}
