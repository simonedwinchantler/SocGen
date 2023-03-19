using mdx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace mdx
{
    internal class mdxConnections
    {
        private Dictionary<string, mdxConnection> dicActiveConnections = new Dictionary<string, mdxConnection>();

        public mdxConnection RegisterConnection(string newHost, string newappName)
        {
            var conn = new mdxConnection
            {
                appName = newappName,
                Host = newHost,
                Identifier = Guid.NewGuid().ToString()
            };
            dicActiveConnections.Add(conn.Identifier, conn);
            

            return conn;
        }

        public bool IsActive(string conn)
        {
            return dicActiveConnections.ContainsKey(conn);
        }
    }

    internal class mdxConnection
    {
        public string Identifier { get; set; }
        public string appName { get; set; }
        public string Host { get; set; }
    }
}
