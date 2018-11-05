using EFCache;
using System.Web.Http;

namespace WebApiWithEFCache
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static readonly InMemoryCache Cache = new InMemoryCache();
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
