using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatePatternSimple
{
    class Program
    {
        static void Main(string[] args)
        {
            var stateMachine = new SoapProvider();

            //initial state
            //stateMachine.ProcessEvent(SoapProvider.Events.SendKeepAlivePing);
            //stateMachine.ProcessEvent(SoapProvider.Events.TryConnect);
            //stateMachine.ProcessEvent(SoapProvider.Events.SendKeepAlivePing);
            //stateMachine.ProcessEvent(SoapProvider.Events.OnConnected);
            //stateMachine.ProcessEvent(SoapProvider.Events.TryConnect);
            //stateMachine.ProcessEvent(SoapProvider.Events.SendKeepAlivePing);
            //stateMachine.ProcessEvent(SoapProvider.Events.SendKeepAlivePing);

            //stateMachine.ProcessEvent(SoapProvider.Events.Disconnect);
            //stateMachine.ProcessEvent(SoapProvider.Events.SendKeepAlivePing);
            //stateMachine.ProcessEvent(SoapProvider.Events.Disconnect);
            //stateMachine.ProcessEvent(SoapProvider.Events.OnConnected);
      
            Console.ReadKey();
        }

        class StateMachine
        {
            public States State { get; set; }

            private Action TryConnectCallback;
            private Action OnConnectedCallback;
            private Action CheckIsAliveCallback;
            private Action DisconnectCallbackCallback;
            private Action StubCallback;

            private Action[,] stateMachineActions;

            private StateMachine()
            {
                Console.WriteLine("private ctor");
            }

            public StateMachine(Action a, Action b, Action c, Action d, Action e):this()
            {
                Console.WriteLine("public ctor");
                TryConnectCallback = a;
                OnConnectedCallback = b;
                CheckIsAliveCallback = c;
                DisconnectCallbackCallback = d;
                StubCallback = e;

                stateMachineActions = new Action[3, 4] { 
                    //TryConnect,              OnConnected,               SendKeepAlivePing,           Disconnect
                    {TryConnectCallback,       StubCallback,              StubCallback,                StubCallback},                 //Disconnected state
                    {StubCallback,             OnConnectedCallback,       StubCallback,                DisconnectCallbackCallback},   //TryingToConnect state
                    {StubCallback,             StubCallback,              CheckIsAliveCallback,        DisconnectCallbackCallback} }; //Connected state
            }

            public void ProcessEvent(Events theEvent)
            {
                stateMachineActions[(int)State, (int)theEvent].Invoke();
            }
        }

        public enum States { Disconnected, TryingToConnect, Connected };
        
        public enum Events { TryConnect, OnConnected, SendKeepAlivePing, Disconnect };
        
        
        class SoapProvider
        {
            private StateMachine sm;

            public SoapProvider()
            {
                sm = new StateMachine(TryConnect,OnConnected, CheckIsAlive, Disconnect, Stub);
                sm.State = States.Disconnected;

                sm.ProcessEvent(Events.SendKeepAlivePing);
                sm.ProcessEvent(Events.TryConnect);
                sm.ProcessEvent(Events.SendKeepAlivePing);
                sm.ProcessEvent(Events.OnConnected);
                sm.ProcessEvent(Events.TryConnect);
                sm.ProcessEvent(Events.SendKeepAlivePing);
                sm.ProcessEvent(Events.SendKeepAlivePing);

                sm.ProcessEvent(Events.Disconnect);
                sm.ProcessEvent(Events.SendKeepAlivePing);
                sm.ProcessEvent(Events.Disconnect);
                sm.ProcessEvent(Events.OnConnected);
            }
            
            private void TryConnect()    
            { 
                sm.State = States.TryingToConnect;
                Console.WriteLine("trying to connect");
            }

            private void Disconnect()
            {
                sm.State = States.Disconnected;
                Console.WriteLine("disconnected");
            }

            private void OnConnected()
            {
                sm.State = States.Connected;
                Console.WriteLine("connected");
            }

            private void CheckIsAlive()
            {
                //no state transition here
                Console.WriteLine("ping sent");
            }

            private void Stub()
            {
                Console.WriteLine("cannot do that when " + sm.State);
            }
        }
    }
}
