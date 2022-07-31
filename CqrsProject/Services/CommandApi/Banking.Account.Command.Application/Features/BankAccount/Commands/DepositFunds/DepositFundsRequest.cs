namespace Banking.Account.Command.Application.Features.BankAccount.Commands.DepositFunds;

public class DepositFundsRequest
{
    public string Id { get; set; } = null!;
    public double Amount { get; set; }

}