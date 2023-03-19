using mdx;
using mdxServer.Service;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Topshelf;

class mdxGlobal
{
    public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static  mdxCache loadedCachedItems = new mdxCache();

    public static mdxConnections activeConnections = new mdxConnections();
}


namespace mdxServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.SetServiceName("MDXServer");
                hostConfigurator.SetDisplayName("MDX Server");
                hostConfigurator.SetDescription("MDX server side components.");

                hostConfigurator.RunAsLocalSystem();

                hostConfigurator.Service<mdxService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(() => new mdxService());

                    serviceConfigurator.WhenStarted(service => service.Start());
                    serviceConfigurator.WhenStopped(service => service.Stop());
                });
            });
        }
    }
}
