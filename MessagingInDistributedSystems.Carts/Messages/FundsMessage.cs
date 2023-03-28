using MessagingInDistributedSystems.Shared;

namespace MessagingInDistributedSystems.Carts.Messages;

public record FundsMessage(long CustomerId, decimal CurrentFunds) : IMessage;