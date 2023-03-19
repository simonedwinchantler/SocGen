using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Policy;
using System.Text.Json;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;

namespace mdx
{
    public class Program
    {
        static async Task<string> PostURI(Uri u, HttpContent c)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.PostAsync(u, c);

                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }

        static async Task<string> GetURI(Uri u)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Create a 'connection' to the server!");

            Uri u = new Uri("http://localhost:50061/mdxserver/connect?host=lonmw76440&appName=FXRatesSnapper");
            var v = Task.Run(() => PostURI(u, new StringContent(String.Empty)));
            v.Wait();
            var connectionIdentifer = JsonSerializer.Deserialize<mdxConnection>(v.Result).Identifier;
        
            Console.WriteLine("Conection identifier is: " + connectionIdentifer);

            Console.WriteLine("Writing first data");

            HttpContent body = new StringContent(System.IO.File.ReadAllText(@"..\..\MarketDataSourceFiles\FX1.xml"), Encoding.UTF8, "application/xml");
            
            var writeURI = "http://localhost:50061/mdxserver/write?connection=***CONN***&identifier=***IDENT***";
            writeURI = writeURI.Replace("***CONN***", connectionIdentifer).Replace("***IDENT***", "/fxrates/london/eod/GBPUSD@20230418");
            u = new Uri(writeURI);
            v = Task.Run(() => PostURI(u, body));
            v.Wait();
            var writeResult = JsonSerializer.Deserialize<mdxWriteResult>(v.Result);

            Console.WriteLine("Data " + writeResult.Message + ", Version=" + writeResult.writtenHeader.Version);

            Console.WriteLine("Writing second data");

            body = new StringContent(System.IO.File.ReadAllText(@"..\..\MarketDataSourceFiles\FX2.xml"), Encoding.UTF8, "application/xml");
            v = Task.Run(() => PostURI(u, body));
            v.Wait();
            writeResult = JsonSerializer.Deserialize<mdxWriteResult>(v.Result);

            Console.WriteLine("Data " + writeResult.Message + ", Version=" + writeResult.writtenHeader.Version);

            Console.WriteLine("Reading latest data for GBPUSD");

            var readURI = "http://localhost:50061/mdxserver/read?connection=***CONN***&identifier=***IDENT***";
            readURI = readURI.Replace("***CONN***", connectionIdentifer).Replace("***IDENT***", "/fxrates/london/eod/GBPUSD@20230418");
            u = new Uri(readURI);
            v = Task.Run(() => GetURI(u));
            v.Wait();
            var dat = v.Result;

            Console.WriteLine("Data read back is: " + System.Environment.NewLine + dat);

            Console.ReadLine();
        }
    }

    public class mdxHeader
    {
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
        public string Status { get; set; }
        public string Message { get; set; }
        public mdxHeader writtenHeader { get; set; }

    }

    internal class mdxConnection
    {
        public string Identifier { get; set; }
        public string appName { get; set; }
        public string Host { get; set; }
    }
}
