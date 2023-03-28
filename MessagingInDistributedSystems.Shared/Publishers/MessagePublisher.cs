using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace MessagingInDistributedSystems.Shared.Publishers;

internal sealed class MessagePublisher : IMessagePublisher
{

    private readonly IModel _channel;
    
    public MessagePublisher(IChannelFactory channelFactory)
    {
        _channel = channelFactory.CreateChannel();
    }
    
    public Task PublishAsync<TMessage>(string exchange, string routingKey, TMessage message) where TMessage : class, IMessage
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        
        _channel.ExchangeDeclare(exchange, "topic", true);
        
        _channel.BasicPublish(exchange, routingKey, properties, body);
        return Task.CompletedTask;
    }
}