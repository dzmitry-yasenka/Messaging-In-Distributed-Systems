using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessagingInDistributedSystems.Shared.Subscribers;

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
        _channel.ExchangeDeclare(exchange, "topic", false, false, null);
        _channel.QueueDeclare(queue, false, false, false);
        _channel.QueueBind(queue, exchange, routingKey);
        
        _channel.BasicQos(0, 4, false);
        
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, basicDeliverEventArgs) =>
        {
            var message = JsonSerializer.Deserialize<TMessage>(Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray()));
            if (message != null)
            {
                await handler(message, basicDeliverEventArgs);
                _channel.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
            }
        };
        
        _channel.BasicConsume(queue, false, consumer);

        return this;
    }
}