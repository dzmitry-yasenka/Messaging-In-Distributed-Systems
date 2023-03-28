using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessagingInDistributedSystems.Shared;

internal sealed class MessageSubscriber : IMessageSubscriber
{
    private readonly IModel _channel;
    
    public MessageSubscriber(IChannelFactory channelFactory)
    {
        _channel = channelFactory.CreateChannel();
    }
    
    public IMessageSubscriber SubscribeMessage<TMessage>(string exchange, string routingKey, string queue, 
        Func<TMessage, BasicDeliverEventArgs, Task> handler) where TMessage : class, IMessage
    {
        _channel.QueueDeclare(queue, true, false, false);
        _channel.QueueBind(queue, exchange, routingKey);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, basicDeliverEventArgs
        ) =>
        {
            var message = JsonSerializer.Deserialize<TMessage>(Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray()));
            if (message != null) 
                await handler(message, basicDeliverEventArgs);
        };
        
        _channel.BasicConsume(queue, true, consumer);

        return this;
    }
}