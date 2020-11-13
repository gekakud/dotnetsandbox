using System;
using System.Reflection;

namespace PubSub
{
    internal static class EventHandlerExtention
    {
        public static bool HandleEvent(this IEventHandler eventHandler, IEventData eventData, Type eventType)
        {
            if (eventHandler == null)
            {
                //s_logger.Warn($"Registered event handler for event type {eventType.Name} does not implement IEventHandler< {eventType.Name} > interface!");
                return false;
            }

            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            try
            {
                //s_logger.Info($"Start EventBus {handlerType} handle event");

                handlerType
                    .GetMethod("HandleEvent", BindingFlags.Public | BindingFlags.Instance, null, new[] { eventType }, null)
                    .Invoke(eventHandler, new object[] { eventData });

                return true;
            }
            catch (TargetInvocationException)
            {
                //s_logger.Warn($"Registered event handler for event type {eventType.Name} failed to invoke HandleEvent");
            }
            catch (Exception)
            {
                //s_logger.Warn($"Registered event handler for event type {eventType.Name} failed to invoke HandleEvent");
            }

            return false;
        }
    }
}