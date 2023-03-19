using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace mdxServer.Service
{
    public class mdxService
    {
        protected IDisposable WebApplication;
        public void Start()
        {
            mdxGlobal.log.Info("Server process starting");

            WebApplication = WebApp.Start<WebPipeline>("http://localhost:50061");

            mdxGlobal.log.Info("Server process startup complete");
        }

        public void Stop()
        {
            WebApplication.Dispose();
        }
    }


    public class WebPipeline
    {
        public void Configuration(IAppBuilder application)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            application.UseWebApi(httpConfiguration);
        }
    }

    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

        }
    }
}
