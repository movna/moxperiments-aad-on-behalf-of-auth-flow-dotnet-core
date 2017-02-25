using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                SaveTokens = true
            });

            app.UseMvc();
        }
    }
}