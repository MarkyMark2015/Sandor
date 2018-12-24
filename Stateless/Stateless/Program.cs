using Stateless.Graph;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateless
{
    class Program
    {
        enum State { OffHook, Ringing, Connected, OnHold }
        enum Trigger { CallDialled, CallConnected, LeftMessage, PlacedOnHold }

        static void Main(string[] args)
        {

            var phoneCall = new StateMachine<State, Trigger>(State.OffHook);

            phoneCall.Configure(State.OffHook)
                .Permit(Trigger.CallDialled, State.Ringing);

            phoneCall.Configure(State.Ringing)
                .OnActivate(() => Console.WriteLine(" -> Ringing"))
                .Permit(Trigger.CallConnected, State.Connected);

            phoneCall.Configure(State.Connected)
                .OnEntry(() => StartCallTimer())
                .OnExit(() => StopCallTimer())
                .Permit(Trigger.LeftMessage, State.OffHook)
                .Permit(Trigger.PlacedOnHold, State.OnHold);

            // ...

            Console.WriteLine(phoneCall.State);
            phoneCall.Fire(Trigger.CallDialled);
            Console.WriteLine(phoneCall.State);
            phoneCall.Fire(Trigger.CallConnected);
            Console.WriteLine(phoneCall.State);

            phoneCall.Fire(Trigger.LeftMessage);
            Console.WriteLine(phoneCall.State);
            Console.Read();
        }

        private static void StartCallTimer()
        {
            Console.WriteLine(" -> StartCallTimer");
        }

        private static void StopCallTimer()
        {
            Console.WriteLine(" -> StopCallTimer");
        }
    }
}
