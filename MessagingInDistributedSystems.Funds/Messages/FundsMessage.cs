using MessagingInDistributedSystems.Shared;

namespace MessagingInDistributedSystems.Funds.Messages;

public record FundsMessage(long CustomerId, decimal CurrentFunds) : IMessage;