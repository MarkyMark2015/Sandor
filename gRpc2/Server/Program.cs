using Grpc.Core;
using Routeguide;
using System;
using Types.Protos;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            // var features = RouteGuideUtil.ParseFeatures(RouteGuideUtil.DefaultFeaturesFile);
            Grpc.Core.Server server = new Grpc.Core.Server()
            { 
                Services = { RouteGuide.BindService(new RouteGuideImpl()) },
                Ports = { new ServerPort(Constants.Address, Constants.Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("RouteGuide server listening");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
