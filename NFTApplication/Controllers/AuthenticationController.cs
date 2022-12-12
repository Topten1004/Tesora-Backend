using System.Net.Http.Headers;
using System.Text.Json;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NFTApplication.Services;
using NFTDatabaseService;


namespace NFTApplication.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly INFTDatabaseService _db;
        private readonly ILogger<AuthenticationController> _logger;


        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public AuthenticationController(IOptions<AppSettings> appSettings, INFTDatabaseService db, ILogger<AuthenticationController> logger)
        {
            _appSettings = appSettings;
            _db = db;
            _logger = logger;   
        }


        /// <summary>
        /// Returns a JSON response determining session's authenticated status.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("authenticated")]
        public ActionResult Authenticated()
        {
            bool authenticated = false;

            if (HttpContext != null)
                if (HttpContext.User != null)
                    if (HttpContext.User.Identity != null)
                        authenticated = HttpContext.User.Identity.IsAuthenticated;

            return Ok(new { authenticated });
        }


        /// <summary>
        /// Logs a user into the system, if they don't exist they are added.
        /// </summary>
        [Authorize]
        [HttpGet("login")]
        public async Task<ActionResult> LogIn()
        {
            // var query = "";

            try
            {
                // Need to determine if we have a new user
                var claims = HttpContext.User.Claims;

                var nameIdentifier = claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

                var userId = nameIdentifier?.Value ?? "unknown";

                // Get their registered information to add them
                var userInfo = await UserInfoAsync();

                if (await _db.UserExists(userId) == false)
                {

                    var record = new NFTDatabaseEntities.User
                    {
                        FirstName = userInfo.FirstName ?? "unknown",
                        LastName = userInfo.LastName ?? "unknown",
                        Email = userInfo.Email,
                        Username = userInfo.UserName ?? "unknown",
                        Status = NFTDatabaseEntities.User.UserStatuses.active,
                        CreateDate = DateTime.UtcNow,
                        MasterUserId = userId ?? "unknown"
                    };

                    await _db.PostUser(record);
                }

                // query = $"?fn={userInfo.FirstName}&ln={userInfo.LastName}";
                // passed in parameter "redirect" += query;

                return Ok("User logged in");
            }
            catch (Exception ex)
            {
                var msg = $"Method: Login, Exception: {ex.Message}";

                _logger.LogError(msg);

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }


        /// <summary>
        /// Destroys the authenticated session and redirects to Home Page.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("logout")]
        public async Task LogOut(string redirect)
        {
            if (HttpContext != null)
            {
                if (HttpContext.Request != null)
                    if (HttpContext.Request.Cookies != null)
                        if (HttpContext.Request.Cookies.Count > 0)
                        {
                            var siteCookies = HttpContext.Request.Cookies.Where(c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));
                            foreach (var cookie in siteCookies)
                            {
                                Response.Cookies.Delete(cookie.Key);
                            }
                        }

                if (HttpContext?.User != null)
                    if (HttpContext.User.Identity != null)
                        if (HttpContext.User.Identity.IsAuthenticated)
                        {
                            var prop = new AuthenticationProperties()
                            {
                                RedirectUri = redirect
                            };

                            await HttpContext.SignOutAsync("Cookies", prop);
                            await HttpContext.SignOutAsync("oidc", prop);
                        }
            }

        }


        /// <summary>
        /// Get the logged in users information
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("userinfo")]
        public async Task<ActionResult> GetUserInfoAsync()
        {
            bool authenticated = false;

            if (HttpContext != null)
                if (HttpContext.User != null)
                    if (HttpContext.User.Identity != null)
                        authenticated = HttpContext.User.Identity.IsAuthenticated;

            if (authenticated && HttpContext != null)
            {
                var userInfo = await UserInfoAsync();

                return Ok(userInfo);
            }
            else 
                return Unauthorized();
        }

        private async Task<UserInfo> UserInfoAsync()
        {
            UserInfo userInfo = default;

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            using (var client = new HttpClient())
            {
                // Build token endpoint URL.
                string url = new Uri(new Uri(_appSettings.Value.AuthBaseUri), "connect/userinfo").ToString();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Fetch tokens from auth server.
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();

                    if (responseStream != null)
                        userInfo = JsonSerializer.Deserialize<UserInfo>(responseStream);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // refresh the token

                }
                else
                {
                    var error =await  response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }

            return userInfo;
        }

    }
}
