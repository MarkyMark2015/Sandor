using Grpc.Core;
using Routeguide;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types.Protos;
using static Routeguide.RouteGuide;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = $"{Constants.Address}:{Constants.Port}";
            // .NET Standard
            //Console.WriteLine("**** Standard");
            //TestStandard(server);
            // Normal
            //Console.WriteLine("**** Normal");
            //TestNormal(server);
            // Stream
            TestStream(server);
            Console.ReadLine();
        }

        private async static void TestStream(string server)
        {
            while (true)
            {
                var channel = new Grpc.Core.Channel(server, ChannelCredentials.Insecure);
                var client = new RouteGuideClient(channel);
                using (var call = client.ListFeatures(new Rectangle()))
                {
                    try
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            Feature feature = call.ResponseStream.Current;
                            Console.WriteLine($"Received {feature}");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        System.Threading.Thread.Sleep(1000);
                        ; // loop
                    }
                }
            channel.ShutdownAsync().Wait();
            }
            Console.WriteLine("Finished TestStream");
        }

        //private RouteGuideClient GetClient()
        //{
        //    var channel = new Grpc.Core.Channel(server, ChannelCredentials.Insecure);
        //    var client = new RouteGuideClient(channel);
        //    using (var call = client.ListFeatures(new Rectangle()))
        //    {
        //        while (true)
        //        {
        //            try
        //            {
        //                while (await call.ResponseStream.MoveNext())
        //                {
        //                    Feature feature = call.ResponseStream.Current;
        //                    Console.WriteLine($"Received {feature}");
        //                }
        //                break;
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"{ex.Message}");
        //                System.Threading.Thread.Sleep(1000);
        //                ; // loop
        //            }
        //        }
        //    }
        //    Console.WriteLine("Finished TestStream");
        //    channel.ShutdownAsync().Wait();
        //}

        private static void TestStandard(string server)
        {
            ClientPOrt.Main.Run(server);
        }

        private static void TestNormal(string server)
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
            Console.WriteLine($".NET Elapsed {sw.ElapsedMilliseconds}");

            Console.WriteLine("Press any key to exit...");
            channel.ShutdownAsync().Wait();
        }
    }
}
