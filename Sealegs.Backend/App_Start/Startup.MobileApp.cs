using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.Tracing;

using Owin;

using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Tables.Config;

using Sealegs.DataObjects;
using Sealegs.Backend.Identity;
using Sealegs.Backend.Models;
using Sealegs.Backend.Controllers;

namespace Sealegs.Backend
{
    public partial class Startup
    {
        #region ConfigureMobileApp

        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute("SealegsAuth", ".auth/login/SealegsAuth", new { controller = "SealegsAuth" });
            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{action}");

            // For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Info;

            // Same as adding UseDefaultConfiguration
            new MobileAppConfiguration()
                .AddMobileAppHomeController()             // from the Home package
                .MapApiControllers()
                .AddTables(                               // from the Tables package
                    new MobileAppTableConfiguration()
                        .MapTableControllers()
                        .AddEntityFramework()             // from the Entity package
                 )
                .AddPushNotifications()                   // from the Notifications package
                .MapLegacyCrossDomainController()         // from the CrossDomain package
                .ApplyTo(config)
                ;

            // Use Entity Framework Code First to create database tables based on your DbContext
            //Database.SetInitializer(new SealegsContextInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            Database.SetInitializer<SealegsContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // This middleware is intended to be used locally for debugging. By default, HostName will
            // only have a value when running in an App Service application.
            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new Microsoft.Azure.Mobile.Server.Authentication.AppServiceAuthenticationOptions
                {
                    SigningKey = GetSigningKey,
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },

                    // Generally, we would not need to do that, because it’s a default token handler.But we need to provide other authentication 
                    // settings (above) to SDK, so we have to provide a(default) token handler as well, because it’s a part of AppServiceAuthenticationOption
                    TokenHandler = new SealegsAppServiceTokenHandler(config)
                });
            }
            else
            {
                // we are in app service ideally, we just need to inject our own AppServicetokenHandler
                // but when we do that we also have to provide other settings. SetAppServiceTokenHandler extension doesn't seem to be working.
                var signingKey = GetSigningKey;
                string hostName = GetHostName(settings);

                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = signingKey,
                    ValidAudiences = new[] { hostName },
                    ValidIssuers = new[] { hostName },
                    TokenHandler = new SealegsAppServiceTokenHandler(config)
                });
            }

            app.UseWebApi(config);
            
            // Swagger
            ConfigureSwagger(config);
        }

        #endregion

        #region GetSigningKey

        // Check for the App Service Auth environment variable WEBSITE_AUTH_SIGNING_KEY,
        // which holds the signing key on the server. If it's not there, check for a SigningKey
        // app setting, which can be used for local debugging.
        private static string GetSigningKey => Environment.GetEnvironmentVariable(SealegsAuthController.AuthSigningKeyVariableName) ?? ConfigurationManager.AppSettings[SealegsAuthController.SealegsAuthApiKeyName];

        #endregion

        #region GetHostName

        private static string GetHostName(MobileAppSettingsDictionary settings) => $"https://{settings.HostName}/";

        #endregion
    }
}

