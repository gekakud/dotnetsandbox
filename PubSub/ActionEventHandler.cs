using System;

namespace PubSub
{
    internal class ActionEventHandler<TEventData> : IEventHandler<TEventData>
    {
        public Action<TEventData> Action { get; }

        public ActionEventHandler(Action<TEventData> action)
        {
            Action = action;
        }

        public void HandleEvent(TEventData eventData)
        {
            Action(eventData);
        }
    }
}