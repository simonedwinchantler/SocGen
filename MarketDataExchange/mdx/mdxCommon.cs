using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static log4net.Appender.RollingFileAppender;

namespace mdx
{
    public class mdxItem
    {   
        public mdxItem(string newIndentifier, XElement newContent)
        {
            itemHeader = new mdxHeader(newIndentifier);
            itemContent = new mdxContent();

            Identifier = newIndentifier;

            itemContent.Content = newContent;
        }
        public string Identifier { get; set; }
        public mdxHeader itemHeader { get; set; }
        public mdxContent itemContent { get; set; }
    }

    public class mdxHeader
    {
        public mdxHeader(string newIndentifier)
        {
            UniqueIndentifier = Guid.NewGuid().ToString();
            ValueDate = DateTime.ParseExact(newIndentifier.Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1], "yyyyMMdd", CultureInfo.InvariantCulture);
            Type = newIndentifier.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public DateTime ValueDate { get; set; }
        public int Version { get; set; }

        public string Type { get; set; }
        public string UniqueIndentifier { get; set; }
    }
    public class mdxContent
    {
        public XElement Content { get; set; }
    }

    public class mdxWriteResult
    {
        public mdxWriteResult(string writeStatus, string writeMessage, mdxHeader header)
        {
            Status = writeStatus;
            Message = writeMessage;
            writtenHeader = header;
        }

        public string Status { get; set; }
        public string Message { get; set; }

        public mdxHeader writtenHeader { get; set; }

    }
    
}
