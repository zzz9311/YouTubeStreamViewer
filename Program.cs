using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YouTubeViewer
{
    class Program
    {
        static string StreamId;

        static int CurrentCountViewers;
        static string StreamTitle;

        static int ThreadCount;

        static int Errors;
        static int BottedViewers;

        static List<string> UserAgents = new List<string>() { "Mozilla/5.0 (iPhone; CPU iPhone OS 6_1_4 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10B350 Safari/8536.25", "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A405 Safari/8536.25", "Mozilla/5.0 (iPhone; CPU iPhone OS 7_0 like Mac OS X) AppleWebKit/537.51.1 (KHTML, like Gecko) Version/7.0 Mobile/11A465 Safari/9537.53", "Mozilla/5.0 (iPhone; CPU iPhone OS 7_0 like Mac OS X) AppleWebKit/537.51.1 (KHTML, like Gecko) Version/7.0 Mobile/11A465 Safari/9537.53" };

        static ProxyType ProxyType;

        static void Main(string[] args)
        {
            Console.Title = "Setting...";
            Console.WriteLine("Chose proxy type\n1) Http Proxy\n2) Socks4 proxy\n3) Socks 5 proxy");
            var ProxyChose = Console.ReadLine();

            switch (ProxyChose)
            {
                case "1": 
                    {
                        ProxyType = ProxyType.HTTP;
                    }
                    break;
                case "2":
                    {
                        ProxyType = ProxyType.Socks4;
                    }
                    break;
                case "3":
                    {
                        ProxyType = ProxyType.Socks5;
                    }
                    break;
            }
            Console.WriteLine("Enter the Stream Id");
            StreamId = Console.ReadLine();
            Console.WriteLine("Enter thread count");
            ThreadCount = Convert.ToInt32(Console.ReadLine());
        }

        static void Log()
        {
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"\r\nBotted: {BottedViewers}\r\nErrors: {Errors}\r\nThreads: {ThreadCount}\r\nTitle: {StreamTitle}\r\nViewers: {CurrentCountViewers}\r\n");
                Thread.Sleep(250);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GenerateUserAgent()
        {
            Random rand = new Random();
            return UserAgents[rand.Next(UserAgents.Count)];
        }

        static void GetUsersToStream()
        {
            while (true)
            {
                try
                {
                    using (var Request = new Leaf.xNet.HttpRequest()
                    {
                        Proxy = null
                    })
                    {
                        Request.SslProtocols = SslProtocols.Default | SslProtocols.Tls11 | SslProtocols.Tls12;
                        Request.UserAgent = GenerateUserAgent();
                    }



                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref Errors);
                }
            }
        }

    }
}
