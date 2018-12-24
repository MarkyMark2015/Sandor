using Grpc.Core;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static Routeguide.RouteGuide;

namespace ClientPOrt
{
    public class Main
    {
        public static void Run(string server)
        {
            var channel = new Grpc.Core.Channel(server, ChannelCredentials.Insecure);
            var client = new RouteGuideClient(channel);

            // Looking for a valid feature
            var pt = new Routeguide.Point();
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 20; i++)
            {
                    var f = client.GetFeature(pt);
                    //Console.WriteLine($"  {f}  {sw.ElapsedMilliseconds}");
            }
            sw.Stop();
            Console.WriteLine($".NET Standard Elapsed {sw.ElapsedMilliseconds}");

            channel.ShutdownAsync().Wait();
        }
    }
}
