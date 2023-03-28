using MessagingInDistributedSystems.Shared.Connections;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MessagingInDistributedSystems.Shared;

public static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        
        var connection = factory.CreateConnection();
        
        services.AddSingleton(connection);
        services.AddSingleton<ChannelAccessor>();
        services.AddSingleton<IChannelFactory, ChannelFactory>();

        return services;
    }
}