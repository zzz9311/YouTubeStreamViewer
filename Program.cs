using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
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

        static int Errors = 0;
        static int BottedViewers = 0;

        private static Random random = new Random();

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
            Console.Clear();
            Console.Title = "Started";
            List<Thread> Threads = new List<Thread>();

            Thread LogThread = new Thread(Log);
            LogThread.Start();
            Threads.Add(LogThread);
            for (int i = 0; i < ThreadCount; i++)
            {
                Thread Worker = new Thread(GetUsersToStream);
                Worker.Start();
                Threads.Add(Worker);
            }

            foreach (var el in Threads)
            {
                el.Join();
            }
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

        public static int GetNumberOfViewers(string result)
        {
            Regex NumbRegex = new Regex("\\d");
            var Number = NumbRegex.Match(result);
            int Viewers;
            if (int.TryParse(Number.Value, out Viewers))
            {
                return Viewers;
            }
            return 0;
        }


        static void GetUsersToStream()
        {
            string pProxy = "";
            string[] pArray = pProxy.Split(':');
            var aProxy = HttpProxyClient.Parse($"{pArray[0]}:{pArray[1]}");
            aProxy.Username = pArray[2];
            aProxy.Password = pArray[3];
            while (true)
            {
                try
                {
                    using (var Request = new Leaf.xNet.HttpRequest()
                    {
                        Proxy = aProxy
                    })
                    {
                        Request.SslProtocols = SslProtocols.Default | SslProtocols.Tls11 | SslProtocols.Tls12;
                        Request.UserAgent = GenerateUserAgent();
                        var RequestResult = Request.Get($"https://www.youtube.com/watch?v={StreamId}").ToString();
                        var StringViewers = RequestResult.Substring("Сейчас смотрят:", "expandedSubtitle\\");

                        CurrentCountViewers = GetNumberOfViewers(RegexHelper.Viewers.Match(StringViewers).Value);

                        var WatchTime = RequestResult.Substring("\"videostatsWatchtimeUrl\":{\"baseUrl\":\"", "\"") + "\"";

                        StreamTitle = RegexHelper.Title.Match(RequestResult).Groups[1].Value;

                        var ParametersUrl = RegexHelper.ViewUrl.Match(RequestResult).Groups[1].Value;
                        ParametersUrl = ParametersUrl.Replace(@"\u0026", "&").Replace("%2C", ",").Replace(@"\/", "/");

                        var query = System.Web.HttpUtility.ParseQueryString(ParametersUrl);
                        if (query.Count == 0)
                        {
                            return;
                        }

                        var cl = query.Get(query.AllKeys[0]);
                        var ei = query.Get("ei");
                        var of = query.Get("of");
                        var vm = query.Get("vm");
                        var cpn = GetCPN();

                        var start = DateTime.UtcNow;

                        var st = random.Next(1000, 10000);
                        var et = GetCmt(start);
                        var lio = GetLio(start);

                        var rt = random.Next(10, 200);

                        var lact = random.Next(1000, 8000);
                        var rtn = rt + 300;
                        var args = new Dictionary<string, string>
                        {
                            ["expire"] = "1628870357",
                            ["ns"] = "yt",
                            ["el"] = "detailpage",
                            ["cpn"] = cpn,
                            ["docid"] = StreamId,
                            ["ver"] = "2",
                            ["cmt"] = et.ToString(),
                            ["ei"] = ei,
                            ["fmt"] = "243",
                            ["fs"] = "0",
                            ["rt"] = rt.ToString(),
                            ["of"] = of,
                            ["euri"] = "",
                            ["lact"] = lact.ToString(),
                            ["live"] = "dvr",
                            ["cl"] = cl,
                            ["state"] = "playing",
                            ["vm"] = vm,
                            ["volume"] = "100",
                            ["cbr"] = "Firefox",
                            ["cbrver"] = "83.0",
                            ["c"] = "WEB",
                            ["cplayer"] = "UNIPLAYER",
                            ["cver"] = "2.20201210.01.00",
                            ["cos"] = "Windows",
                            ["cosver"] = "10.0",
                            ["cplatform"] = "DESKTOP",
                            ["delay"] = "5",
                            ["hl"] = "en_US",
                            ["rtn"] = rtn.ToString(),
                            ["aftm"] = "140",
                            ["rti"] = rt.ToString(),
                            ["muted"] = "0",
                            ["st"] = st.ToString(),
                            ["et"] = et.ToString(),
                            ["lio"] = lio.ToString()
                        };

                        Request.AddHeader(HttpHeader.Referer, "https://youtube.com/watch?v=" + StreamId);
                        Request.AddHeader(HttpHeader.Accept, "image/webp,image/apng,image/*,*/*;q=0.8");
                        Request.AddHeader(HttpHeader.AcceptLanguage, "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
                        Request.AddHeader(HttpHeader.DNT, "1");
                        Request.AddHeader(HttpHeader.Pragma, "no-cache");
                        Request.AddHeader(HttpHeader.CacheControl, "max-age=10");
                        Request.AddHeader("sec-fetch-dest", "document");
                        Request.AddHeader("sec-fetch-site", "none");
                        Request.AddHeader("sec-fetch-mode", "navigate");
                        string urlToGet = buildUrl(args);

                        Request.Get(urlToGet.Replace("watchtime", "playback"));

                        var check = Request.Get(urlToGet);

                        Interlocked.Increment(ref BottedViewers);
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref Errors);
                }
            }

        }
        public static double GetLio(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var start = date.ToUniversalTime() - origin;
            var value = start.TotalSeconds.ToString("#.000");
            return double.Parse(value);
        }
        public static double GetCmt(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var start = date.ToUniversalTime() - origin;
            var now = DateTime.UtcNow.ToUniversalTime() - origin;
            var value = (now.TotalSeconds - start.TotalSeconds).ToString("#.000");
            return double.Parse(value);
        }
        public static string GetCPN()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static string buildUrl(Dictionary<string, string> args)
        {
            var url = "https://s.youtube.com/api/stats/watchtime?";
            foreach (var arg in args)
            {
                url += $"{arg.Key}={arg.Value}&";
            }
            return url;
        }
    }
}
