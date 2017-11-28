using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Swagger;

using Swashbuckle.Application;

namespace Sealegs.Backend
{
    public partial class Startup
    {
        /// <summary>
        /// https://github.com/Azure/azure-mobile-apps-net-server/wiki/Adding-Swagger-Metadata-and-Help-UI-to-a-Mobile-App
        /// </summary>
        /// <param name="config"></param>
        public static void ConfigureSwagger(HttpConfiguration config)
        {
            // Use the custom ApiExplorer that applies constraints. This prevents
            // duplicate routes on /api and /tables from showing in the Swagger doc.
            config.Services.Replace(typeof(IApiExplorer), new MobileAppApiExplorer(config));

            config
               .EnableSwagger(c =>
               {
                   c.SingleApiVersion("v1", "Sealegs.Backend");

                   // Tells the Swagger doc that any MobileAppController needs a
                   // ZUMO-API-VERSION header with default 2.0.0
                   c.OperationFilter<MobileAppHeaderFilter>();

                   // Looks at attributes on properties to decide whether they are readOnly.
                   // Right now, this only applies to the DatabaseGeneratedAttribute.
                   c.SchemaFilter<MobileAppSchemaFilter>();

                   // Without these lines Swagger will not work but will blow out with a 500 error
                   c.IncludeXmlComments(GetXmlCommentsPath());
                   c.ResolveConflictingActions(x => x.First());

                   // Fot authorization protected requests
                   c.AppServiceAuthentication("https://sealegs.azurewebsites.net/", "SealegsAuth");
               })
               .EnableSwaggerUi(c =>
               {
                   c.EnableOAuth2Support("na", "na", "na");

                   // Replaces some javascript files with specific logic to:
                   // 1. Do the OAuth flow using the App Service Auth parameters
                   // 2. Parse the returned token
                   // 3. Apply the token to the X-ZUMO-AUTH header
                   c.MobileAppUi(config);
               }); 
        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\Sealegs.Backend.XML",
                    System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}