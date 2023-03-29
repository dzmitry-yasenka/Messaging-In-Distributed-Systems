using MessagingInDistributedSystems.Carts.Messages;
using MessagingInDistributedSystems.Shared;

namespace MessagingInDistributedSystems.Carts.Services;

public class MessagingBackgroundService : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly ILogger<MessagingBackgroundService> _logger;

    public MessagingBackgroundService(IMessageSubscriber messageSubscriber, ILogger<MessagingBackgroundService> logger)
    {
        _messageSubscriber = messageSubscriber;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageSubscriber.SubscribeMessage<FundsMessage>("Funds", "FundsMessage.#", 
            "carts-service-funds-message-with-many-words-in-routing-key", 
             (message, args) =>
            {
                _logger.LogInformation("Received message for customer: {message} with {funds} with RoutingKey = {routingKey}", 
                    message.CustomerId, message.CurrentFunds, args.RoutingKey);
                return Task.CompletedTask;
            });
        _messageSubscriber.SubscribeMessage<FundsMessage>("Funds", "FundsMessage.*", 
            "carts-service-funds-message-with-single-word-in-routing-key", 
            (message, args) =>
            {
                _logger.LogInformation("Received message for customer: {message} with {funds} with RoutingKey = {routingKey}", 
                    message.CustomerId, message.CurrentFunds, args.RoutingKey);
                return Task.CompletedTask;
            });
    }
}