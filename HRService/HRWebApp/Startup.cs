using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HRWebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(
                sharedOptions => { sharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; });
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(50),
                SlidingExpiration = false
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                AuthenticationScheme = "AAD",
                AutomaticAuthenticate = true,
                ClientId = "<HR-APP-AAD-CLIENT-ID>",
                ClientSecret = "<HR-APP-AAD-CLIENT-SECRET>",
                Authority =
                    "https://login.microsoftonline.com/<TENANT-DOMAIN-OR-GUID>",
                ResponseType = OpenIdConnectResponseType.Code,
                // SaveTokens is not required
                // SaveTokens = true,
                Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var credentials = new ClientCredential("<HR-APP-AAD-CLIENT-ID>", "<HR-APP-AAD-CLIENT-SECRET>");
                        var authContext =
                            new AuthenticationContext("https://login.microsoftonline.com/<TENANT-DOMAIN-OR-GUID>");

                        // On-behalf-of auth token request call
                        var authResult = await authContext.AcquireTokenAsync("<RESOURCE-URI-OF-VACATION-API>",
                            credentials,
                            new UserAssertion(context.TokenEndpointResponse.IdToken));

                        context.Properties.Items.Add("vacation.api.token", authResult.AccessToken);
                    }
                }
            });

            app.UseMvc();
        }
    }
}