using System;
using Autofac;

namespace PubSub
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Runner r = new Runner();
            r.Init();
            r.SubscribeToSomeEvents();
            r.PublishSomeEvents();

            Console.ReadKey();
        }
    }

    public class SomeEventSharedData : IEventData
    {
        public string Payload { get; set; }
    }
    
    internal class Runner
    {
        private IEventBusPublisher publisher;
        private IEventBusRegistrator registrator;
        
        public void Init()
        {
            IContainer container = new BootStrapper().Bootstrap();
            publisher = container.Resolve<IEventBusPublisher>();
            registrator = container.Resolve<IEventBusRegistrator>();
        }

        public void SubscribeToSomeEvents()
        {
            registrator.Subscribe<SomeEventSharedData>(data =>
            {
                Console.WriteLine(data.Payload);
            });
        }

        public void PublishSomeEvents()
        {
            publisher.Publish(new SomeEventSharedData {Payload = "Hello there"});
        }
    }
    
    internal class BootStrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EventBus>().As<IEventBusPublisher, IEventBusRegistrator>().SingleInstance();
            
            return builder.Build();
        }
    }
}