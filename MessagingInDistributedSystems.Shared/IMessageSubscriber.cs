using RabbitMQ.Client.Events;

namespace MessagingInDistributedSystems.Shared;

public interface IMessageSubscriber
{
    IMessageSubscriber SubscribeMessage<TMessage>(string exchange, string routingKey, string queue, 
        Func<TMessage, BasicDeliverEventArgs, Task> handler) where TMessage : class, IMessage;
}