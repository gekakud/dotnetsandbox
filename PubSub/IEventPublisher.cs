using System;
using System.Threading.Tasks;

namespace PubSub
{
    public interface IEventPublisher : IDisposable
    {
        Task<bool> Post(Tuple<Type, IEventData> item);
    }
}