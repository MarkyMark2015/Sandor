using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Timers;
using Grpc.Core;
using Routeguide;


namespace Server
{
    public class RouteGuideImpl : RouteGuide.RouteGuideBase
    {
        private int _count = 0;
        private Timer _tim = new Timer(500);
        private static BlockingCollection<int> _bc = new BlockingCollection<int>();

        public RouteGuideImpl()
        {
            Console.WriteLine("***Create");
            _tim.Elapsed += _tim_Elapsed;
            _tim.Start();
        }

        private void _tim_Elapsed(object sender, ElapsedEventArgs e)
        {
            _count++;
            _bc.Add(_count);
        }

        public override Task<Feature> GetFeature(Point request, Grpc.Core.ServerCallContext context)
        {
            _count++;
            var f = new Feature() {
                Location = new Point { Latitude = _count, Longitude = 2 * _count },
                Name = $"Anna {_count}"
            };
            // Console.WriteLine(_count);
            return Task.FromResult(f);
        }

        private int _id = 0;
        public override async Task ListFeatures(Rectangle request, IServerStreamWriter<Feature> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"ListFeatures {context.Peer} {context.Method}");
            foreach (var b in _bc.GetConsumingEnumerable())
            {
                _id++;
                var msg = $"Feature {b}";
                Console.WriteLine(msg);
                await responseStream.WriteAsync(new Feature() { Name = msg, Location = null });
            }
        }

        public override Task<RouteSummary> RecordRoute(IAsyncStreamReader<Point> requestStream, ServerCallContext context)
        {
            return base.RecordRoute(requestStream, context);
        }

        public override Task RouteChat(IAsyncStreamReader<RouteNote> requestStream, IServerStreamWriter<RouteNote> responseStream, ServerCallContext context)
        {
            return base.RouteChat(requestStream, responseStream, context);
        }

    }


}
