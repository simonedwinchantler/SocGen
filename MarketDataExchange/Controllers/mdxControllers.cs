using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Web.Http.Results;
using mdx;
using System.Xml.Linq;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net;

namespace MDXServer.Controllers
{
    public class mdxServerController : ApiController
    {
        [Route("mdxserver/read/")]
        [HttpGet]
        public IHttpActionResult Read(string connection, string identifier)
        {
            if (mdxGlobal.activeConnections.IsActive(connection))
            {
                var result = mdxGlobal.loadedCachedItems.Read(identifier);

                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<XElement>(result.itemContent.Content,
                          new System.Net.Http.Formatting.XmlMediaTypeFormatter
                          {
                              UseXmlSerializer = true
                          })
                });
            }
            else
            {
                return InternalServerError();
            }
        }

        [Route("mdxserver/write")]
        [HttpPost]
        public IHttpActionResult Write(string connection, string identifier, [FromBody] XElement content)
        {
            if (mdxGlobal.activeConnections.IsActive(connection))
            {
                var result = mdxGlobal.loadedCachedItems.Write(new mdxItem(identifier, content));
                return Ok<mdxWriteResult>(result);
            }
            else
            {
                return InternalServerError();
            }
        }

        [Route("mdxserver/connect")]
        [HttpPost]
        public IHttpActionResult Connect(string host, string appName)
        {
            try
            {
                var conn = mdxGlobal.activeConnections.RegisterConnection(host, appName);
                //return Ok<string>(JsonSerializer.Serialize(conn));
                return Ok<mdxConnection>(conn);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("mdxserver/status")]
        [HttpPost]
        public IHttpActionResult Status(string connection)
        {
            try
            {
                return Ok<string>(JsonSerializer.Serialize("Server is online. " + connection + " is alive."));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
