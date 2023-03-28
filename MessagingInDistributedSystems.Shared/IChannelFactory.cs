using RabbitMQ.Client;

namespace MessagingInDistributedSystems.Shared;

public interface IChannelFactory
{
    IModel CreateChannel();
}