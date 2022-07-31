namespace Banking.Account.Command.Application.Features.BankAccount.Commands.WithdrawFunds;

public class WithdrawFundsRequest
{
    public string Id { get; set; } = null!;
    public double Amount { get; set; }
}