using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Shared;

namespace HRWebApp.Controllers
{
    public class VacationController : Controller
    {
        [HttpGet("benefits/vacations/upcoming")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Provided same scheme as in OpenID Connect middleware
            var authInfo = await HttpContext.Authentication.GetAuthenticateInfoAsync("AAD");

            // Provided same key as in OnTokenValidated
            var userJwt = authInfo.Properties.GetTokenValue("id_token");

            var credentials = new ClientCredential("<HR-APP-AAD-CLIENT-ID>", "<HR-APP-AAD-CLIENT-SECRET>");
            var authContext = new AuthenticationContext("https://login.microsoftonline.com/<TENANT-DOMAIN-OR-GUID>");

            // On-behalf-of auth token request call
            var authResult = await authContext.AcquireTokenAsync("<RESOURCE-URI-OF-VACATION-API>", credentials,
                new UserAssertion(userJwt));

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "<UPCOMING-VACATIONS-API-ENDPOINT>");

                // Add the token to the request
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var upcomingVacations =
                        JsonConvert.DeserializeObject<IEnumerable<Vacation>>(await response.Content
                            .ReadAsStringAsync());
                    return View("UpcomingVacations", upcomingVacations);
                }
                return View("VacationFetchFailure");
            }
        }
    }
}